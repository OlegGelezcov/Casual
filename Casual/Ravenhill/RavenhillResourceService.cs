using Casual.Ravenhill.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Casual.Ravenhill {
    public class RavenhillResourceService : ResourceService {

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

        private List<RoomSettingData> roomSettings { get; } = new List<RoomSettingData>();
        public LevelExpTable levelExpTable { get; } = new LevelExpTable();
        private Dictionary<string, AvatarData> avatars { get; } = new Dictionary<string, AvatarData>();

        private Sprite m_TransparentSprite = null;

        public override Sprite transparent {
            get {
                if(m_TransparentSprite == null ) {
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
            m_IsLoaded = true;
            var eventService = engine.GetService<IEventService>();
            eventService?.SendEvent(new RavenhillResourceLoadedEventArgs());
        }

        public override void Setup(object data) {
            Load();
        }

        private void PreloadPrefabs() {
            prefabObjectCache.Load(new Dictionary<string, string> {
                ["search_text"] = "Prefabs/UI/Misc/search_text",
                ["search_object_particles"] = "Prefabs/Effects/search_object_particles",
                ["found_search_object"] = "Prefabs/Effects/found_search_object"
            });
        }

        private void PreloadSprites() {
            spriteObjectCache.Load(new Dictionary<string, string> {
                ["transparent"] = "Sprites/transparent"
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
            if(avatars.ContainsKey(avatarId)) {
                return avatars[avatarId];
            }
            return null;
        }

        public RoomData GetRoomData(string roomId) {
            if(roomDataDictionary.ContainsKey(roomId)) {
                return roomDataDictionary[roomId];
            }
            return null;
        }

        public RoomData GetRoomData(RoomType roomType ) {
            foreach(var kvp in roomDataDictionary) {
                if(kvp.Value.roomType == roomType ) {
                    return kvp.Value;
                }
            }
            return null;
        }

        public RoomData GetRoomDataBySceneName(string sceneName, RoomMode roomMode) {
            foreach(var kvp in roomDataDictionary) {
                if(sceneName == kvp.Value.GetScene(roomMode)) {
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

        public SearchObjectData GetSearchObjectData(string id) {
            return searchObjects.ContainsKey(id) ? searchObjects[id] : null;
        }

        public override GameObject GetCachedPrefab(string key, string path="") {
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
    }
}
