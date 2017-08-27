using System;
using Casual.Ravenhill.Data;
using System.Collections.Generic;
using System.Linq;

namespace Casual.Ravenhill {
    public class RavenhillGameModeService : GameModeService, ISaveable {
        public RoomData previousRoom { get; private set; }
        public RoomData currentRoom { get; private set; }
        public SearchSession searchSession { get; private set; } = new SearchSession();
        public RoomMode roomMode { get; private set; } = RoomMode.normal;
        public RoomManager roomManager { get; } = new RoomManager();

        public override void Start() {
            base.Start();
            engine.GetService<ISaveService>().Register(this);
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
            } else {
                drops = new List<InventoryItem>();
            }

            searchSession.EndSession(status, time, drops);
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

        //Algorithm of generation room drop...
        public List<InventoryItem> GenerateRoomDrop(RoomData roomData, RoomLevel roomLevel) {
                
            RavenhillResourceService resourceService = engine.GetService<IResourceService>() as RavenhillResourceService;
            List<InventoryItem> result = new List<InventoryItem>();

            List<CollectableData> collectables = resourceService.GetCollectables(roomData.id).FindAll((item) => {
                return ((int)roomLevel >= (int)item.roomLevel) &&
                    (UnityEngine.Random.value < item.prob);
            });
            result.AddRange(collectables.Select(c => new InventoryItem(c, 1)).ToList());

            List<WeaponData> weapons = resourceService.weaponList.FindAll((w) => UnityEngine.Random.value < w.prob);
            result.AddRange(weapons.Select(w => new InventoryItem(w, 1)).ToList());

            List<IngredientData> ingredients = resourceService.GetIngredients(roomData.id).FindAll(item => {
                return UnityEngine.Random.value < item.prob;
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
                    new DropItem(DropType.silver, session.roomData.silverReward),
                    new DropItem(DropType.exp, session.roomData.expReward)
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


        #region ISaveable
        public string saveId => "gamemode";

        public bool isLoaded { get; private set; }

        public string GetSave() {
            UXMLWriteElement writeElement = new UXMLWriteElement(saveId);
            writeElement.AddAttribute("room_mode", roomMode.ToString());
            writeElement.Add(roomManager.GetSave());
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

                isLoaded = true;
            }
            return true;
        }

        public void InitSave() {
            roomMode = RoomMode.normal;
            roomManager.InitSave();
            isLoaded = true;
        }

        public void OnRegister() {
            
        }

        public void OnLoaded() {
            
        } 
        #endregion
    }


}
