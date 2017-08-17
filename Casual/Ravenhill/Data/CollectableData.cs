using System;
using System.Collections.Generic;

namespace Casual.Ravenhill.Data {
    public class CollectableData : InventoryItemData  {

        public string collectionId { get; private set; }
        public List<string> rooms { get; } = new List<string>();
        public RoomLevel roomLevel { get; private set; }
        public float prob { get; private set; }


        public override void Load(UXMLElement element) {
            base.Load(element);
            collectionId = element.GetString("collection");
            rooms.Clear();
            foreach(string roomId in element.GetString("room").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
                rooms.Add(roomId);
            }
            roomLevel = element.GetEnum<RoomLevel>("rank");
            prob = element.GetFloat("prob");
        }

        public override InventoryItemType type => InventoryItemType.Collectable;

        public bool IsValidRoom(string roomId ) {
            return rooms.Contains(roomId);
        }
    }
}
