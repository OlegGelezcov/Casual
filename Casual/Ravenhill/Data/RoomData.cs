using System.Collections.Generic;

namespace Casual.Ravenhill.Data {
    public class RoomData : IconData {

        public int level { get; private set; }
        public PriceData price { get; private set; } = new PriceData();
        //public int energy { get; private set; }
        public RoomType roomType { get; private set; }
        public string scaryIconPath { get; private set; }
        public int silverReward { get; private set; }
        public int expReward { get; private set; }
        public string scene { get; private set; }
        public string scaryScene { get; private set; }
        public bool isSpecial { get; private set; }
        public int floor { get; private set; }
        public List<string> linkedRooms { get; } = new List<string>();

        public override void Load(UXMLElement element) {
            base.Load(element);
            level = element.GetInt("level", 1);
            price.Load(element);
            //energy = element.GetInt("energy", 0);
            roomType = element.GetEnum<RoomType>("type", RoomType.hallway);
            scaryIconPath = element.GetString("sicon");
            silverReward = element.GetInt("reward_silver", 0);
            expReward = element.GetInt("reward_exp", 0);
            scaryScene = element.GetString("sscene");
            scene = element.GetString("scene");
            isSpecial = element.GetBool("special", false);
            floor = element.GetInt("floor", 0);
            linkedRooms.Clear();
            linkedRooms.AddRange(element.GetStringArray("linked"));
        }

        public string GetScene(RoomMode mode) {
            return (mode == RoomMode.normal) ? scene : scaryScene;
        }

        public string GetIcon(RoomMode mode) {
            return (mode == RoomMode.normal) ? iconPath : scaryIconPath;
        }

        public string GetIconKey(RoomMode mode) {
            return (mode == RoomMode.normal) ? "n_" + id : "s_" + id;
        }

        public bool IsLinked(string otherRoomId ) {
            return linkedRooms.Contains(otherRoomId);
        }

    }
}
