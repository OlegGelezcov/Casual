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

        private bool m_IsLoaded = false;

        public override bool isLoaded {
            get => m_IsLoaded;
        }

        public override void Load() {
            LoadResourcePath();
            LoadStrings();
            LoadRooms();
            LoadSearchObjects();
            m_IsLoaded = true;
            var eventService = engine.GetService<IEventService>();
            eventService?.SendEvent(new RavenhillResourceLoadedEventArgs());
        }

        public override void Setup(object data) {
            Load();
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


        public SearchObjectData GetSearchObjectData(string id) {
            return searchObjects.ContainsKey(id) ? searchObjects[id] : null;
        }

        public string GetString(string key) => stringResource.GetString(key);

        public int stringCount => stringResource.count;

    }
}
