using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill {
    public class RavenhillResourceService : ResourceService {

        private const string kResourceFile = "Data/Temp/resources";

        private Dictionary<string, string> resourcePathDictionary { get; } = new Dictionary<string, string>();
        private Dictionary<string, RoomData> roomDataDictionary { get; } = new Dictionary<string, RoomData>();


        private bool m_IsLoaded = false;

        public override bool isLoaded {
            get => m_IsLoaded;
        }

        public override void Load() {
            LoadResourcePath();
            LoadRooms();
            m_IsLoaded = true;
            var eventService = engine.GetService<IEventService>();
            eventService?.SendEvent(new RavenhillResourceLoadedEventArgs());
        }

        public override void Setup(object data) {
            Load();
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


    }
}
