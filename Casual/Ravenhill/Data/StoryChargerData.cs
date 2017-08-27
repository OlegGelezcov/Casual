using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Data {
    public class StoryChargerData : InventoryItemData {
        public override PriceData price { get; protected set; }

        public override void Load(UXMLElement element) {
            base.Load(element);
            price = new PriceData(element);
        }

        public override InventoryItemType type => InventoryItemType.StoryCharger;
        public override bool isUsableFromInventory => false;
        public override bool IsSellable => true;
    }
}
