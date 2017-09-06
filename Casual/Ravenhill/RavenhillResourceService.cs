using Casual.Ravenhill.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Casual.Ravenhill {
    public class RavenhillResourceService : ResourceService {

        public class CachedSprite {
            private Sprite sprite = null;
            private string key;
            private string path;
            private ResourceObjectCache<string, Sprite> cache;

            public CachedSprite(string key, string path, ResourceObjectCache<string, Sprite> cache) {
                this.key = key;
                this.path = path;
                this.cache = cache;
            }

            public Sprite Sprite {
                get {
                    if (sprite == null) {
                        sprite = cache.GetObject(key, path);
                    }
                    return sprite;
                }
            }
        }

        private const string kResourceFile = "Data/Temp/resources";
        private readonly List<string> kStringAssets = new List<string> {
            "Data/Loc/str_achievements",
            "Data/Loc/str_gameobjects",
            "Data/Loc/str_interface",
            "Data/Loc/str_misc",
            "Data/Loc/str_quests",
            "Data/Loc/str_searchobjects",
            "Data/Loc/str_tutorial"
        };

        private Dictionary<string, string> resourcePathDictionary { get; } = new Dictionary<string, string>();
        private Dictionary<string, RoomData> roomDataDictionary { get; } = new Dictionary<string, RoomData>();
        private Dictionary<string, SearchObjectData> searchObjects { get; } = new Dictionary<string, SearchObjectData>();
        private StringResource stringResource { get; } = new StringResource();
        private ResourceObjectCache<string, GameObject> prefabObjectCache { get; } = new ResourceObjectCache<string, GameObject>();
        private ResourceObjectCache<string, Sprite> spriteObjectCache { get; } = new ResourceObjectCache<string, Sprite>();
        private Dictionary<string, ToolData> tools { get; } = new Dictionary<string, ToolData>();
        private Dictionary<string, BonusData> bonuses { get; } = new Dictionary<string, BonusData>();
        private Dictionary<string, FoodData> foods { get; } = new Dictionary<string, FoodData>();
        private Dictionary<string, WeaponData> weapons { get; } = new Dictionary<string, WeaponData>();
        private Dictionary<string, ChargerData> chargers { get; } = new Dictionary<string, ChargerData>();
        private Dictionary<string, StoryChargerData> storyChargers { get; } = new Dictionary<string, StoryChargerData>();
        private Dictionary<string, IngredientData> ingredients { get; } = new Dictionary<string, IngredientData>();
        private Dictionary<string, CollectableData> collectables { get; } = new Dictionary<string, CollectableData>();
        private Dictionary<string, BankProductData> bankProducts { get; } = new Dictionary<string, BankProductData>();
        private Dictionary<string, StoryCollectionData> storyCollections { get; } = new Dictionary<string, StoryCollectionData>();
        private Dictionary<string, StoryCollectableData> storyCollectables { get; } = new Dictionary<string, StoryCollectableData>();
        private Dictionary<string, JournalEntryData> journal { get; } = new Dictionary<string, JournalEntryData>();
        private Dictionary<string, QuestData> quests { get; } = new Dictionary<string, QuestData>();
        private Dictionary<string, QuestOwnerData> questOwners { get; } = new Dictionary<string, QuestOwnerData>();
        private Dictionary<string, StoreItemData> storeItems { get; } = new Dictionary<string, StoreItemData>();
        private Dictionary<string, VideoData> videos { get; } = new Dictionary<string, VideoData>();
        private Dictionary<string, BuffData> buffs { get; } = new Dictionary<string, BuffData>();

        public CachedSprite expSprite;
        public CachedSprite healthSprite;
        public CachedSprite maxHealthSprite;
        public CachedSprite silverSprite;
        public CachedSprite goldSprite;

        private Dictionary<RoomLevel, string> roomLevelNameTable { get; } = new Dictionary<RoomLevel, string> {
            [RoomLevel.Beginner] = "Loc_rank_beginner",
            [RoomLevel.Advanced] = "Loc_rank_advanced",
            [RoomLevel.Detective] = "Loc_rank_detective",
            [RoomLevel.Explorer] = "Loc_rank_explorer",
            [RoomLevel.Hunter] = "Loc_rank_hunter"
        };



        //loaded by last(after other inventory items)
        private Dictionary<string, CollectionData> collections { get; } = new Dictionary<string, CollectionData>();


        private List<RoomSettingData> roomSettings { get; } = new List<RoomSettingData>();
        public LevelExpTable levelExpTable { get; } = new LevelExpTable();
        private Dictionary<string, AvatarData> avatars { get; } = new Dictionary<string, AvatarData>();

        private Sprite m_TransparentSprite = null;

        public override Sprite transparent {
            get {
                if (m_TransparentSprite == null) {
                    m_TransparentSprite = spriteObjectCache.GetObject("transparent");
                }
                return m_TransparentSprite;
            }
        }


        private bool m_IsLoaded = false;

        public override bool isLoaded {
            get => m_IsLoaded;
        }

        public override void Load() {
            LoadResourcePath();
            PreloadPrefabs();
            PreloadSprites();
            LoadStrings();
            LoadRooms();
            LoadSearchObjects();
            LoadRoomSettings();
            LoadLevelExpTable();
            LoadAvatars();
            LoadTools();
            LoadBonuses();
            LoadFoods();
            LoadWeapons();
            LoadChargers();
            LoadStoryChargers();
            LoadIngredients();
            LoadCollectables();
            LoadBank();
            LoadStoryCollectables();
            LoadStoryCollections();
            LoadJournal();
            LoadQuestOwners();
            LoadQuests();
            LoadStoreItems();
            LoadVideos();
            LoadBuffs();

            LoadCollections();
            LoadMiscSprites();
            m_IsLoaded = true;
            RavenhillEvents.OnResourceLoaded();
        }

        public override void Setup(object data) {
            Load();
        }

        private void LoadMiscSprites() {
            expSprite = new CachedSprite("exp", "Sprites/Misc/exp", spriteObjectCache);
            healthSprite = new CachedSprite("health", "Sprites/Misc/health", spriteObjectCache);
            maxHealthSprite = new CachedSprite("maxhealth", "Sprites/Misc/health", spriteObjectCache);
            goldSprite = new CachedSprite("gold", "Sprites/Misc/gold", spriteObjectCache);
            silverSprite = new CachedSprite("silver", "Sprites/Misc/silver", spriteObjectCache);
        }

        private void PreloadPrefabs() {
            prefabObjectCache.Load(new Dictionary<string, string> {
                ["search_text"] = "Prefabs/UI/Misc/search_text",
                ["search_object_particles"] = "Prefabs/Effects/search_object_particles",
                ["found_search_object"] = "Prefabs/Effects/found_search_object",
                ["drop_object"] = "Prefabs/UI/Misc/drop_object"
            });
        }

        private void PreloadSprites() {
            spriteObjectCache.Load(new Dictionary<string, string> {
                ["transparent"] = "Sprites/transparent"
            });
        }

        private void LoadBuffs() {
            var document = new UXMLDocument(resourcePathDictionary["buffs"]);
            buffs.Clear();

            document.Element("buffs").Elements("buff").ForEach(element => {
                BuffData buffData = new BuffData();
                buffData.Load(element);
                buffs[buffData.id] = buffData;
            });
        }

        private void LoadVideos() {
            var document = new UXMLDocument(resourcePathDictionary["videos"]);
            videos.Clear();

            document.Element("videos").Elements("video").ForEach(element => {
                VideoData videoData = new VideoData();
                videoData.Load(element);
                videos[videoData.id] = videoData;
            });
        }

        private void LoadStoreItems() {
            var document = new UXMLDocument(resourcePathDictionary["store_items"]);
            storeItems.Clear();

            document.Element("store_items").Elements("item").ForEach(element => {
                StoreItemData data = new StoreItemData();
                data.Load(element);
                storeItems[data.id] = data;
            });

        }

        private void LoadQuestOwners() {
            var document = new UXMLDocument(resourcePathDictionary["owners"]);

            questOwners.Clear();
            document.Element("owners").Elements("owner").ForEach(ownerElement => {
                QuestOwnerData data = new QuestOwnerData();
                data.Load(ownerElement);
                questOwners[data.id] = data;
            });
        }

        private void LoadQuests() {
            var document = new UXMLDocument(resourcePathDictionary["quests"]);

            quests.Clear();
            document.Element("quests").Elements("quest").ForEach(questElement => {
                QuestData questData = new QuestData();
                questData.Load(questElement);
                quests[questData.id] = questData;
            });
            Debug.Log($"total quests loaded: {quests.Count}".Colored(ColorType.yellow));
        }

        private void LoadJournal() {
            var document = new UXMLDocument(resourcePathDictionary["journal"]);

            journal.Clear();
            document.Element("journal").Elements("entry").ForEach(entryElement => {
                JournalEntryData data = new JournalEntryData();
                data.Load(entryElement);
                journal[data.id] = data;
            });

        }

        private void LoadStoryCollectables() {
            UXMLDocument document = new UXMLDocument();
            document.Load(resourcePathDictionary["story_collectables"]);

            storyCollectables.Clear();
            document.Element("story_collectables").Elements("collectable").ForEach(ce => {
                StoryCollectableData data = new StoryCollectableData();
                data.Load(ce);
                storyCollectables[data.id] = data;
            });
        }

        private void LoadStoryCollections() {
            UXMLDocument document = new UXMLDocument();
            document.Load(resourcePathDictionary["story_collections"]);

            storyCollections.Clear();
            document.Element("story_collections").Elements("collection").ForEach(ce => {
                StoryCollectionData data = new StoryCollectionData();
                data.Load(ce);
                storyCollections[data.id] = data;
            });
        }

        private void LoadBank() {
            UXMLDocument document = new UXMLDocument();
            document.Load(resourcePathDictionary["bank"]);

            bankProducts.Clear();
            document.Element("bank").Elements("product").ForEach(productElement => {
                BankProductData data = new BankProductData();
                data.Load(productElement);
                bankProducts[data.id] = data;
            });
        }

        private void LoadCollections() {
            UXMLDocument document = new UXMLDocument();
            document.Load(resourcePathDictionary["collections"]);

            collections.Clear();
            document.Element("collections").Elements("collection").ForEach(collectionElement => {
                CollectionData collectionData = new CollectionData();
                collectionData.Load(collectionElement);
                collections[collectionData.id] = collectionData;
            });
        }

        private void LoadCollectables() {
            UXMLDocument document = new UXMLDocument();
            document.Load(resourcePathDictionary["collectables"]);

            collectables.Clear();
            document.Element("collectables").Elements("collectable").ForEach(collectableElement => {
                CollectableData collectableData = new CollectableData();
                collectableData.Load(collectableElement);
                collectables[collectableData.id] = collectableData;
            });
        }

        private void LoadIngredients() {
            UXMLDocument document = new UXMLDocument();
            document.Load(resourcePathDictionary["ingredients"]);

            ingredients.Clear();
            document.Element("ingredients").Elements("ingredient").ForEach((ingredientElement) => {
                IngredientData ingredientData = new IngredientData();
                ingredientData.Load(ingredientElement);
                ingredients[ingredientData.id] = ingredientData;
            });
        }



        private void LoadStoryChargers() {
            UXMLDocument document = new UXMLDocument();
            document.Load(resourcePathDictionary["story_chargers"]);

            storyChargers.Clear();
            document.Element("story_chargers").Elements("charger").ForEach(chargerElement => {
                StoryChargerData storyChargerData = new StoryChargerData();
                storyChargerData.Load(chargerElement);
                storyChargers[storyChargerData.id] = storyChargerData;
            });
        }

        private void LoadChargers() {
            UXMLDocument document = new UXMLDocument();
            document.Load(resourcePathDictionary["chargers"]);

            chargers.Clear();
            document.Element("chargers").Elements("charger").ForEach(chargerElement => {
                ChargerData chargerData = new ChargerData();
                chargerData.Load(chargerElement);
                chargers[chargerData.id] = chargerData;
            });
        }

        private void LoadWeapons() {
            UXMLDocument document = new UXMLDocument();
            document.Load(resourcePathDictionary["weapons"]);

            weapons.Clear();
            document.Element("weapons").Elements("weapon").ForEach(weaponElement => {
                WeaponData weaponData = new WeaponData();
                weaponData.Load(weaponElement);
                weapons[weaponData.id] = weaponData;
            });
        }

        private void LoadFoods() {
            UXMLDocument document = new UXMLDocument();
            document.Load(resourcePathDictionary["foods"]);

            foods.Clear();
            document.Element("foods").Elements("food").ForEach(foodElement => {
                FoodData foodData = new FoodData();
                foodData.Load(foodElement);
                foods[foodData.id] = foodData;
            });
        }

        private void LoadBonuses() {
            UXMLDocument document = new UXMLDocument();
            document.Load(resourcePathDictionary["bonuses"]);

            bonuses.Clear();
            document.Element("bonuses").Elements("bonus").ForEach(bonusElement => {
                BonusData bonusData = new BonusData();
                bonusData.Load(bonusElement);
                bonuses[bonusData.id] = bonusData;
                Debug.Log($"load bonus {bonusData.id}");
            });
        }

        private void LoadTools() {
            UXMLDocument document = new UXMLDocument();
            document.Load(resourcePathDictionary["tools"]);

            tools.Clear();
            document.Element("tools").Elements("tool").ForEach(toolElement => {
                ToolData toolData = new ToolData();
                toolData.Load(toolElement);
                tools[toolData.id] = toolData;
            });
        }

        private void LoadAvatars() {
            UXMLDocument document = new UXMLDocument();
            document.Load(resourcePathDictionary["avatars"]);

            avatars.Clear();
            document.Element("avatars").Elements("avatar").ForEach(avatarElement => {
                AvatarData avatarData = new AvatarData();
                avatarData.Load(avatarElement);
                avatars[avatarData.id] = avatarData;
            });
        }

        private void LoadLevelExpTable() {
            levelExpTable.Load(resourcePathDictionary["level_exp_table"]);
        }

        private void LoadRoomSettings() {
            string path = resourcePathDictionary["room_settings"];
            UXMLDocument document = new UXMLDocument();
            document.Load(path);
            roomSettings.Clear();
            var dump = document.Element("room_settings").Elements("room_setting").Select(roomSettingElement => {
                RoomSettingData data = new RoomSettingData();
                data.Load(roomSettingElement);
                roomSettings.Add(data);
                return data;
            }).ToList();
        }

        private void LoadStrings() {
            var conflicts = stringResource.Load(kStringAssets, Utility.gameLanguage);
            foreach (var conflict in conflicts) {
                Debug.LogWarning(conflict.ToString());
            }
            Debug.Log($"string loaded {stringCount}");
        }

        private void LoadResourcePath() {
            resourcePathDictionary.Clear();
            UXMLDocument document = new UXMLDocument();
            document.Load(kResourceFile);
            var dump = document.Element("resources").Elements("resource").Select(element => {
                resourcePathDictionary.Add(element.GetString("key"), element.GetString("path"));
                return element;
            }).ToList();
        }

        private void LoadRooms() {
            roomDataDictionary.Clear();
            UXMLDocument document = new UXMLDocument();
            document.Load(resourcePathDictionary["rooms"]);
            var dump = document.Element("rooms").Elements("room").Select((roomElement) => {
                RoomData roomData = new RoomData();
                roomData.Load(roomElement);
                roomDataDictionary[roomData.id] = roomData;
                return roomData;
            }).ToList();
        }

        private void LoadSearchObjects() {
            searchObjects.Clear();
            UXMLDocument document = new UXMLDocument();
            document.Load(resourcePathDictionary["search_objects"]);
            var dump = document.Element("search_objects").Elements("search_object").Select(element => {
                SearchObjectData data = new SearchObjectData(element);
                searchObjects[data.id] = data;
                return data;
            }).ToList();
        }

        public AvatarData GetAvatarData(string avatarId) {
            if (avatars.ContainsKey(avatarId)) {
                return avatars[avatarId];
            }
            return null;
        }

        public RoomData GetRoomData(string roomId) {
            if (roomDataDictionary.ContainsKey(roomId)) {
                return roomDataDictionary[roomId];
            }
            return null;
        }

        public RoomData GetRoomData(RoomType roomType) {
            foreach (var kvp in roomDataDictionary) {
                if (kvp.Value.roomType == roomType) {
                    return kvp.Value;
                }
            }
            return null;
        }

        public QuestOwnerData GetQuestOwner(string id) {
            return questOwners.GetOrDefault(id);
        }



        public RoomData GetRoomDataBySceneName(string sceneName, RoomMode roomMode) {
            foreach (var kvp in roomDataDictionary) {
                if (sceneName == kvp.Value.GetScene(roomMode)) {
                    return kvp.Value;
                }
            }
            return null;
        }

        public override Sprite GetSprite(IconData data) {
            if (data.hasIcon) {
                return spriteObjectCache.GetObject(data.id, data.iconPath);
            } else {
                return transparent;
            }
        }

        public Sprite GetSprite(QuestOwnerData questOwner, RoomMode mode) {
            if(mode == RoomMode.normal) {
                return GetSprite(questOwner);
            } else {
                return GetSprite(questOwner.id + "_scary", questOwner.iconScary);
            }
        }

        public Sprite GetSprite(string key, string path ) {
            var sprite = spriteObjectCache.GetObject(key, path);
            if(sprite == null ) {
                sprite = transparent;
            }
            return sprite;
        }

        public Sprite GetSprite(InventoryItem item) {
            if (item.data != null) {
                return GetSprite(item.data);
            }
            return transparent;
        }

        public Sprite GetSprite(DropItem dropItem) {
            switch (dropItem.type) {
                case DropType.exp: {
                        return expSprite.Sprite;
                    }
                case DropType.gold: {
                        return goldSprite.Sprite;
                    }
                case DropType.health: {
                        return healthSprite.Sprite;
                    }
                case DropType.max_health: {
                        return maxHealthSprite.Sprite;
                    }
                case DropType.silver: {
                        return silverSprite.Sprite;
                    }
                case DropType.item: {
                        return GetSprite(dropItem.itemData);
                    }
                default: {
                        return transparent;
                    }
            }
        }



        public SearchObjectData GetSearchObjectData(string id) {
            return searchObjects.ContainsKey(id) ? searchObjects[id] : null;
        }

        public override GameObject GetCachedPrefab(string key, string path = "") {
            return prefabObjectCache.GetObject(key, path);
        }

        public override string GetString(string key) => stringResource.GetString(key);

        public int stringCount => stringResource.count;

        public RoomSettingData GetRoomSetting(RoomLevel roomLevel) {
            return roomSettings.FirstOrDefault(roomSetting => roomSetting.roomLevel == roomLevel);
        }

        public ToolData GetTool(string id) {
            return tools.GetOrDefault(id);
        }

        public FoodData GetFood(string id) {
            return foods.GetOrDefault(id);
        }

        public BonusData GetBonus(string id) {
            return bonuses.GetOrDefault(id);
        }

        public ChargerData GetCharger(string id) {
            return chargers.GetOrDefault(id);
        }

        public IngredientData GetIngredient(string id) {
            return ingredients.GetOrDefault(id);
        }

        public StoryChargerData GetStoryCharger(string id) {
            return storyChargers.GetOrDefault(id);
        }

        public WeaponData GetWeapon(string id) {
            return weapons.GetOrDefault(id);
        }

        public CollectionData GetCollection(string id) {
            return collections.GetOrDefault(id);
        }

        public CollectableData GetCollectable(string id) {
            return collectables.GetOrDefault(id);
        }

        public List<CollectableData> GetCollectables(string roomId) {
            return collectables.Values.Where(data => data.IsValidRoom(roomId)).ToList();
        }

        public List<IngredientData> GetIngredients(string roomId) {
            return ingredients.Values.Where(data => data.IsValidRoom(roomId)).ToList();
        }

        public BankProductData GetBankProduct(string id) {
            return bankProducts.GetOrDefault(id);
        }

        public StoryCollectionData GetStoryCollection(string id ) {
            return storyCollections.GetOrDefault(id);
        }

        public StoryCollectableData GetStoryCollectable(string id ) {
            return storyCollectables.GetOrDefault(id);
        }

        public JournalEntryData GetJournalEntry(string id) {
            return journal.GetOrDefault(id);
        }

        public QuestData GetQuest(string id) {
            return quests.GetOrDefault(id);
        }

        public StoreItemData GetStoreItem(string id) {
            return storeItems.GetOrDefault(id);
        }

        public VideoData GetVideoData(string id) {
            return videos.GetOrDefault(id);
        }

        public BuffData GetBuff(string id) {
            return buffs.GetOrDefault(id);
        }

        public List<WeaponData> weaponList => new List<WeaponData>(weapons.Values);

        public List<ChargerData> chargerList => new List<ChargerData>(chargers.Values);

        public List<StoryChargerData> storyChargerList => new List<StoryChargerData>(storyChargers.Values);

        public List<BonusData> bonusList => new List<BonusData>(bonuses.Values);

        public List<ToolData> toolList => new List<ToolData>(tools.Values);

        public List<FoodData> foodList => new List<FoodData>(foods.Values);

        public List<IngredientData> ingredientList => new List<IngredientData>(ingredients.Values);

        public List<CollectionData> collectionList =>
            new List<CollectionData>(collections.Values).OrderBy(c => c.id).ToList();

        public List<BankProductData> bankProductList => new List<BankProductData>(bankProducts.Values);

        public List<StoryCollectionData> storyCollectionList => new List<StoryCollectionData>(storyCollections.Values);

        public List<StoryCollectableData> storyCollectableList => new List<StoryCollectableData>(storyCollectables.Values);

        public List<JournalEntryData> journalList => new List<JournalEntryData>(journal.Values);

        public List<QuestData> questList => new List<QuestData>(quests.Values);

        public List<QuestOwnerData> questOwnerList => new List<QuestOwnerData>(questOwners.Values);

        public List<StoreItemData> storeItemList => new List<StoreItemData>(storeItems.Values);

        public List<VideoData> videoList => new List<VideoData>(videos.Values);

        public List<BuffData> buffList => new List<BuffData>(buffs.Values);

        public List<InventoryItemData> marketItems {
            get {
                List<InventoryItemData> items = new List<InventoryItemData>();
                items.AddRange(tools.Values);
                items.AddRange(bonuses.Values);
                items.AddRange(foods.Values);
                items.AddRange(weapons.Values);
                items.AddRange(chargers.Values);
                items.AddRange(storyChargers.Values);
                items.AddRange(ingredients.Values);
                return items;
            }
        }

        public InventoryItemData GetInventoryItemData(InventoryItemType type, string id) {
            switch (type) {
                case InventoryItemType.Bonus: {
                        return GetBonus(id);
                    }
                case InventoryItemType.Charger: {
                        return GetCharger(id);
                    }
                case InventoryItemType.Collectable: {
                        return GetCollectable(id);
                    }
                case InventoryItemType.Collection: {
                        return GetCollection(id);
                    }
                case InventoryItemType.Food: {
                        return GetFood(id);
                    }
                case InventoryItemType.Ingredient: {
                        return GetIngredient(id);
                    }
                case InventoryItemType.StoryCharger: {
                        return GetStoryCharger(id);
                    }
                case InventoryItemType.Tool: {
                        return GetTool(id);
                    }
                case InventoryItemType.Weapon: {
                        return GetWeapon(id);
                    }
                case InventoryItemType.StoryCollection: {
                        return GetStoryCollection(id);
                    }
                case InventoryItemType.StoryCollectable: {
                        return GetStoryCollectable(id);
                    }
                default: {
                        throw new System.NotImplementedException($"{type}");
                    }
            }
        }

        public string GetRoomLevelName(RoomLevel roomLevel) {
            return GetString(roomLevelNameTable[roomLevel]);
        }

        public QuestData GetQuest(JournalEntryData entryData ) {
            return quests.Values.Where(quest => quest.journalId == entryData.id).FirstOrDefault();
        }

        public Sprite GetPriceSprite(PriceData price) {
            if(price.type == MoneyType.silver ) {
                return silverSprite.Sprite;
            } else if(price.type == MoneyType.gold ) {
                return goldSprite.Sprite;
            }
            return transparent;
        }
    }
}
