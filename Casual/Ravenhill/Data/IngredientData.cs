using System;
using System.Collections.Generic;

namespace Casual.Ravenhill.Data {

    public class IngredientData : InventoryItemData {
        public override PriceData price { get; protected set; }
        public float prob { get; private set; }
        public List<string> rooms { get; } = new List<string>();

        public override void Load(UXMLElement element) {
            base.Load(element);
            price = new PriceData(element);
            prob = element.GetFloat("prob");
            rooms.Clear();
            foreach(string roomId in element.GetString("drop_rooms").Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries)) {
                rooms.Add(roomId);
            }
        }

        public override InventoryItemType type => InventoryItemType.Ingredient;
        public override bool isUsableFromInventory => false;
        public override bool IsSellable => true;

        public bool IsValidRoom(string roomId) {
            return rooms.Contains(roomId);
        }
    }
}
