using System;
using Casual.Ravenhill.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Casual.Ravenhill {
    public class RavenhillGameModeService : GameModeService, ISaveable {
        public RoomData previousRoom { get; private set; }
        public RoomData currentRoom { get; private set; }
        public SearchSession searchSession { get; private set; } = new SearchSession();
        public RoomMode roomMode { get; private set; } = RoomMode.normal;
        public RoomManager roomManager { get; } = new RoomManager();
        public int searchCounter { get; private set; } = 0;
        public string lastSearchRoomId { get; set; } = string.Empty;

        private readonly Queue<CollectableData> receivedCollectables = new Queue<CollectableData>();
        private bool collectableViewStarted = false;
        private float updateCollectableTimer = 1.0f;
        private readonly DailyRewardManager dailyRewardManager = new DailyRewardManager();

        public override IRoomManager RoomManager {
            get {
                return roomManager;
            }
        }

        public override void Start() {
            base.Start();
            engine.GetService<ISaveService>().Register(this);
            dailyRewardManager.Start();
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.SearchSessionEnded += OnSearchSessionEnded;
            RavenhillEvents.InventoryItemAdded += OnInventoryItemAdded;
            RavenhillEvents.GameModeChanged += OnGameModeChanged;
            RavenhillEvents.PlayerLevelChanged += OnLevelChanged;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.SearchSessionEnded -= OnSearchSessionEnded;
            RavenhillEvents.InventoryItemAdded -= OnInventoryItemAdded;
            RavenhillEvents.GameModeChanged -= OnGameModeChanged;
            RavenhillEvents.PlayerLevelChanged -= OnLevelChanged;
        }

        private void OnLevelChanged(int oldLevel, int newLevel) {
            StartCoroutine(CorNewLevel(oldLevel, newLevel));
        }

        private System.Collections.IEnumerator CorNewLevel(int oldLevel, int newLevel ) {
            int currentLevel = oldLevel + 1;

            while (currentLevel <= newLevel) {
                viewService.ShowViewWithCondition(RavenhillViewType.level_up_view, () => {
                    return (gameModeName == GameModeName.map || gameModeName == GameModeName.hallway)
                    && viewService.noModals;
                }, currentLevel);
                yield return new WaitForEndOfFrame();
                currentLevel++;
            }

        }

        private void OnInventoryItemAdded(InventoryItemType type, string itemId, int count ) {
            if(type == InventoryItemType.Collectable ) {
                RavenhillResourceService resourceService = engine.GetService<IResourceService>().Cast<RavenhillResourceService>();
                CollectableData data = resourceService.GetCollectable(itemId);
                if(data != null ) {
                    receivedCollectables.Enqueue(data);
                    //StopCoroutine("CorShowReceivedCollectables");
                    //StartCoroutine(CorShowReceivedCollectables(data));
                }
            }
        }

        private void OnGameModeChanged(GameModeName oldName, GameModeName newName ) {
            if(newName == GameModeName.map || newName == GameModeName.hallway ) {
                viewService.ShowView(RavenhillViewType.buffs_view);
                viewService.ShowView(RavenhillViewType.screen_quest_list);
            } else {
                viewService.RemoveView(RavenhillViewType.buffs_view);
                viewService.RemoveView(RavenhillViewType.screen_quest_list);
            }
        }


        private System.Collections.IEnumerator CorShowReceivedCollectables(CollectableData data) {
            RavenhillViewService viewService = engine.GetService<IViewService>().Cast<RavenhillViewService>();
            yield return new WaitUntil(() => (!viewService.ExistView(RavenhillViewType.collectable_added_note_view)));

            viewService.ShowView(RavenhillViewType.collectable_added_note_view, data);
            yield return new WaitUntil(() => (!viewService.ExistView(RavenhillViewType.collectable_added_note_view)));
            yield return new WaitForSeconds(0.3f);
            collectableViewStarted = false;
        }

        public override void Update() {
            base.Update();
            updateCollectableTimer -= Time.deltaTime;
            if(updateCollectableTimer <= 0.0f ) {
                updateCollectableTimer += 1.0f;
                if(!collectableViewStarted && receivedCollectables.Count > 0 ) {
                    var data = receivedCollectables.Dequeue();
                    collectableViewStarted = true;
                    StartCoroutine(CorShowReceivedCollectables(data));
                }
            }
            dailyRewardManager.Update();
        }

        private void OnSearchSessionEnded(SearchSession session ) {
            if(session.isSearchSuccessed) {
                searchCounter++;
                lastSearchRoomId = session.roomId;

                RavenhillEvents.OnSearchCounterChanged(searchCounter);
            }
        }

        public void ResetSearchCounter() {
            if(searchCounter > 0 ) {
                searchCounter = 0;
                RavenhillEvents.OnSearchCounterChanged(searchCounter);
            }
        }


        public void SetRoomMode(RoomMode roomMode ) {
            RoomMode oldRoomMode = this.roomMode;
            this.roomMode = roomMode;

            if(oldRoomMode != this.roomMode ) {
                RavenhillEvents.OnRoomModeChanged(oldRoomMode, this.roomMode);
            }
        }

        public void StartSession(RoomInfo roomInfo ) {
            searchSession.StartSession(roomInfo);
        }

        public void EndSession(SearchStatus status, int time) {
            List<InventoryItem> drops = null;
            if(status == SearchStatus.success) {
                drops = GenerateRoomDrop(searchSession.roomData, GetRoomInfo(searchSession.roomData.id).roomLevel);

                var questItems = TryGenerateQuestItems();
                if(questItems.Count > 0 ) {
                    drops.AddRange(questItems);
                }

            } else {
                drops = new List<InventoryItem>();
            }

            searchSession.EndSession(status, time, drops);
        }

        private List<InventoryItem> TryGenerateQuestItems() {
            var questService = engine.GetService<IQuestService>().Cast<QuestService>();
            var resourceService = engine.GetService<IResourceService>().Cast<RavenhillResourceService>();

            List<InventoryItem> items = new List<InventoryItem>();

            foreach(QuestInfo quest in questService.startedQuestList ) {
                if(quest.IsValidData) {
                    if(quest.type == QuestType.find_collection_element ) {
                        LastSearchRoomCondition roomCondition = quest.GetCompleteCondition<LastSearchRoomCondition>();
                        HasCollectableCondition collectableCondtion = quest.GetCompleteCondition<HasCollectableCondition>();
                        if(collectableCondtion != null ) {
                            var collectableItem = resourceService.GetCollectable(collectableCondtion.id);
                            if(collectableItem != null ) {
                                if(roomCondition != null ) {
                                    if(searchSession.roomId == roomCondition.id ) {
                                        items.Add(new InventoryItem(collectableItem, 1));
                                    }
                                } else {
                                    items.Add(new InventoryItem(collectableItem, 1));
                                }
                            }
                        }
                    }
                }
            }
            return items;

        }

        public void ExitSessionRoom() {
            ApplySessionResults(searchSession);
            var searchManager = FindObjectOfType<SearchManager>();
            searchManager?.Exit();
        }

        public void ChangeRoom(string roomId ) {
            previousRoom = currentRoom;
            currentRoom = engine.GetService<IResourceService>().Cast<RavenhillResourceService>().GetRoomData(roomId);
        }

        public RoomInfo GetRoomInfo(string roomId) {
            return roomManager.GetRoomInfo(roomId);
        }

        public bool IsUnlocked(string roomId ) {
            return roomManager.IsUnlocked(roomId);
        }

        private float GetCollectableProb(CollectableData data) {
            return data.prob * (1.0f + engine.GetService<IPlayerService>().Cast<PlayerService>().GetValue(BonusType.collectable_prob));
        }

        private float GetIngredientProb(IngredientData data) {
            return data.prob * (1.0f + engine.GetService<IPlayerService>().Cast<PlayerService>().GetValue(BonusType.ingredient_prob));
        }
        //Algorithm of generation room drop...
        public List<InventoryItem> GenerateRoomDrop(RoomData roomData, RoomLevel roomLevel) {
                
            RavenhillResourceService resourceService = engine.GetService<IResourceService>() as RavenhillResourceService;
            List<InventoryItem> result = new List<InventoryItem>();

            List<CollectableData> collectables = resourceService.GetCollectables(roomData.id).FindAll((item) => {
                return ((int)roomLevel >= (int)item.roomLevel) &&
                    (UnityEngine.Random.value < GetCollectableProb(item));
            });
            result.AddRange(collectables.Select(c => new InventoryItem(c, 1)).ToList());

            List<WeaponData> weapons = resourceService.weaponList.FindAll((w) => UnityEngine.Random.value < w.prob);
            result.AddRange(weapons.Select(w => new InventoryItem(w, 1)).ToList());

            List<IngredientData> ingredients = resourceService.GetIngredients(roomData.id).FindAll(item => {
                return UnityEngine.Random.value < GetIngredientProb(item);
            });
            result.AddRange(ingredients.Select(i => new InventoryItem(i, 1)).ToList());

            List<ChargerData> chargers = resourceService.chargerList.FindAll(item => UnityEngine.Random.value < item.prob);
            result.AddRange(chargers.Select(c => new InventoryItem(c, 1)).ToList());

            return result;
        }

        public List<InventoryItem> FilterCollectableForRoom(RoomInfo roomInfo) {
            RavenhillResourceService resourceService = engine.GetService<IResourceService>() as RavenhillResourceService;
            return resourceService.GetCollectables(roomInfo.roomData.id).Where(collectable => {
                return ((int)collectable.roomLevel <= (int)roomInfo.roomLevel);
            }).Select(collectable => new InventoryItem(collectable, 1)).ToList();
        }

        private void ApplySessionResults(SearchSession session ) {
            if(session.searchStatus == SearchStatus.success ) {
                List<DropItem> dropItems = new List<DropItem> {
                    new DropItem(DropType.silver, GetModifiedSilverCount(session.roomId, session.roomData.silverReward)),
                    new DropItem(DropType.exp, GetModifiedExpCount(session.roomId, session.roomData.expReward))
                };
                foreach(InventoryItem item in session.roomDropList ) {
                    dropItems.Add(new DropItem(DropType.item, 1, item.data));
                }

                engine.Cast<RavenhillEngine>().DropItems(dropItems, null, () => {
                    return (gameModeName == GameModeName.map || gameModeName == GameModeName.hallway) &&
                        (!viewService.hasModals) && (viewService.ExistView(RavenhillViewType.hud));
                });

                roomManager.AddProgress(session.roomId);
                roomManager.RollSearchMode(session.roomId);

            }
        }

        public bool IsAlchemyReadyToCharge(BonusData bonus ) {
            bool alchemyReady = true;

            RavenhillResourceService resourceService = engine.GetService<IResourceService>().Cast<RavenhillResourceService>();
            PlayerService playerService = engine.GetService<IPlayerService>().Cast<PlayerService>();

            foreach(var pair in bonus.ingredients ) {
                int playerCount = playerService.GetItemCount(resourceService.GetIngredient(pair.Key));
                if(playerCount < pair.Value ) {
                    alchemyReady = false;
                    break;
                }
            }
            return alchemyReady;
        }

        public bool IsCollectionReadyToCharge(CollectionData collection ) {
            bool collectablesReady = true;

            RavenhillResourceService resourceService = engine.GetService<IResourceService>().Cast<RavenhillResourceService>();
            PlayerService playerService = engine.GetService<IPlayerService>().Cast<PlayerService>();

            foreach(string collectableId in collection.collectableIds ) {
                CollectableData data = resourceService.GetCollectable(collectableId);
                int playerCount = playerService.GetItemCount(data);
                if(playerCount <= 0 ) {
                    collectablesReady = false;
                    break;
                }
            }

            bool chargersReady = true;
            foreach(var info in collection.chargers) {
                ChargerData data = resourceService.GetCharger(info.id);
                int playerCount = playerService.GetItemCount(data);
                if(playerCount < info.count ) {
                    chargersReady = false;
                    break;
                }
            }

            return collectablesReady && chargersReady;
        }

        public bool IsStoryCollectionReadyToCharge(StoryCollectionData collection ) {
            bool hasCollectables = true;

            RavenhillResourceService resourceService = engine.GetService<IResourceService>().Cast<RavenhillResourceService>();
            PlayerService playerService = engine.GetService<IPlayerService>().Cast<PlayerService>();

            foreach (string collectableId in collection.collectables ) {
                StoryCollectableData collectableData = resourceService.GetStoryCollectable(collectableId);
                int playerCount = playerService.GetItemCount(collectableData);
                if(playerCount <= 0 ) {
                    hasCollectables = false;
                    break;
                }
            }

            bool hasCharger = true;
            StoryChargerData chargerData = resourceService.GetStoryCharger(collection.chargerId);
            int playerChargerCount = playerService.GetItemCount(chargerData);
            if(playerChargerCount <= 0 ) {
                hasCharger = false;
            }

            return hasCollectables && hasCharger;
        }

        public void ChargeAlchemy(BonusData bonus ) {

            RavenhillResourceService resourceService = engine.GetService<IResourceService>().Cast<RavenhillResourceService>();
            PlayerService playerService = engine.GetService<IPlayerService>().Cast<PlayerService>();

            foreach(var pair in bonus.ingredients ) {
                IngredientData ingredientData = resourceService.GetIngredient(pair.Key);
                playerService.RemoveItem(ingredientData.type, ingredientData.id, pair.Value);
            }

            playerService.AddItem(new InventoryItem(bonus, 1));
            //engine.Cast<RavenhillEngine>().DropItems(new List<DropItem> { new DropItem(DropType.item, 1, bonus )});
            RavenhillEvents.OnAlchemyCharged(bonus);
        }

        public void ChargeCollection(CollectionData collectionData) {
            PlayerService playerService = engine.GetService<IPlayerService>().Cast<PlayerService>();
            foreach (string collectableId in collectionData.collectableIds) {
                playerService.RemoveItem(InventoryItemType.Collectable, collectableId, 1);
            }

            foreach(var info in collectionData.chargers) {
                playerService.RemoveItem(InventoryItemType.Charger, info.id, info.count);
            }

            playerService.AddItem(new InventoryItem(collectionData, 1));
            engine.Cast<RavenhillEngine>().DropItems(collectionData.rewards, null, () => !viewService.hasModals);
        }

        public void ChargeStoryCollection(StoryCollectionData collectionData) {
            PlayerService playerService = engine.GetService<IPlayerService>().Cast<PlayerService>();
            RavenhillResourceService resourceService = engine.GetService<IResourceService>().Cast<RavenhillResourceService>();

            StoryChargerData chargerData = resourceService.GetStoryCharger(collectionData.chargerId);

            if(playerService.GetItemCount(chargerData) > 0 ) {
                playerService.RemoveItem(InventoryItemType.StoryCharger, chargerData.id, 1);
                playerService.AddItem(new InventoryItem(collectionData, 1));
                RavenhillEvents.OnStoryCollectionCharged(collectionData);
            }
        }

        public int CurrentRoomSearchTime {
            get {
                Debug.Log($"debuff on room search time {searchSession.roomId}:{engine.GetService<INpcService>().GetBuffValue(searchSession.roomId, "BE001")}");
                return Mathf.RoundToInt(
                    searchSession.roomInfo.currentRoomSetting.searchTime * 
                    (1.0f - engine.GetService<INpcService>().GetBuffValue(searchSession.roomId, "BE001"))
                    );
            }
        }

        public int GetModifiedExpCount(string roomId, int baseExp) {
            Debug.Log($"Exp debuff for room {roomId}: {engine.GetService<INpcService>().GetBuffValue(roomId, "BE002")}");
            return Mathf.RoundToInt(
                baseExp * (1.0f - engine.GetService<INpcService>().GetBuffValue(roomId, "BE002"))
                );
        }

        public int GetModifiedSilverCount(string roomId, int baseSilver ) {
            Debug.Log($"Silver debuff for room {roomId}: {engine.GetService<INpcService>().GetBuffValue(roomId, "BE003")}");
            return Mathf.RoundToInt(
                baseSilver * (1.0f - engine.GetService<INpcService>().GetBuffValue(roomId, "BE003"))
                );
        }

        public bool HasShuffleWordsDebuff(string roomId ) {
            return !Mathf.Approximately(engine.GetService<INpcService>().GetBuffValue(roomId, "BE004"), 0.0f);
        }

        public string ShufffleWord(string text) {
            List<string> words = text.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> result = new List<string>();
            foreach(string word in words) {
                char[] chArr = word.ToCharArray();
                chArr.ShuffleArray();
                string newStr = new string(chArr);
                result.Add(newStr);
            }
            return string.Join(" ", result);
        }


        #region ISaveable
        public string saveId => "gamemode";

        public bool isLoaded { get; private set; }

        public string GetSave() {
            UXMLWriteElement writeElement = new UXMLWriteElement(saveId);
            writeElement.AddAttribute("room_mode", roomMode.ToString());
            
            writeElement.AddAttribute("search_counter", searchCounter);
            writeElement.AddAttribute("last_search_room", lastSearchRoomId);

            writeElement.Add(roomManager.GetSave());
            writeElement.Add(dailyRewardManager.GetSave());

            return writeElement.ToString();
        }

        public bool Load(string saveStr) {
            if(string.IsNullOrEmpty(saveStr)) {
                InitSave();
            } else {
                UXMLDocument document = new UXMLDocument();
                document.Parse(saveStr);
                UXMLElement gameModeElement = document.Element(saveId);

                roomMode = gameModeElement?.GetEnum<RoomMode>("room_mode") ?? RoomMode.normal;

                UXMLElement roomsElement = gameModeElement.Element("rooms");
                if(roomsElement != null ) {
                    roomManager.Load(roomsElement);
                } else {
                    roomManager.InitSave();
                }

                UXMLElement dailyRewardElement = gameModeElement.Element("daily_reward");
                if(dailyRewardElement != null) {
                    dailyRewardManager.Load(dailyRewardElement);
                } else {
                    dailyRewardManager.InitSave();
                }

                searchCounter = gameModeElement.GetInt("search_counter", 0);
                lastSearchRoomId = gameModeElement.GetString("last_search_room", string.Empty);
                isLoaded = true;
            }
            return true;
        }

        public void InitSave() {
            roomMode = RoomMode.normal;
            roomManager.InitSave();
            dailyRewardManager.InitSave();
            searchCounter = 0;
            lastSearchRoomId = string.Empty;
            isLoaded = true;
        }

        public void OnRegister() {
            
        }

        public void OnLoaded() {
            
        } 
        #endregion
    }


}
