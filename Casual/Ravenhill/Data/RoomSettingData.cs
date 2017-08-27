using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Data {
    public class RoomSettingData {

        public RoomLevel roomLevel { get; private set; }
        public int roomProgress { get; private set; }
        public int searchObjectCount { get; private set; }
        public int searchTime { get; private set; }
        public int health { get; private set; }

        public void Load(UXMLElement element) {
            roomLevel = element.GetEnum<RoomLevel>("level", RoomLevel.Beginner);
            roomProgress = element.GetInt("progress", 0);
            searchObjectCount = element.GetInt("search_object_count", 5);
            searchTime = element.GetInt("search_time");
            health = element.GetInt("health");
        }


    }
}
