using System;

namespace Casual.Ravenhill.Data {
    public class ToolData : InventoryItemData {

        public override PriceData price { get; protected set; }

        public override void Load(UXMLElement element) {
            base.Load(element);
            price = new PriceData(element);
        }

        public override InventoryItemType type => InventoryItemType.Tool;
        public override bool isUsableFromInventory => false;
        public override bool IsSellable => true;
    }
}
