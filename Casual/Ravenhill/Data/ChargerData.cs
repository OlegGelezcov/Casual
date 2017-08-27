using System;

namespace Casual.Ravenhill.Data {
    public class ChargerData : InventoryItemData {

        public override PriceData price { get; protected set; }
        public float prob { get; private set; }

        public override void Load(UXMLElement element) {
            base.Load(element);
            price = new PriceData(element);
            prob = element.GetFloat("prob");
        }

        public override InventoryItemType type => InventoryItemType.Charger;

        public override bool isUsableFromInventory => false;
        public override bool IsSellable => true;
    }
}
