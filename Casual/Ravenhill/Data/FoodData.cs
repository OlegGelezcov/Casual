using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Data {
    public class FoodData : InventoryItemData {
        public override PriceData price { get; protected set; }

        public override void Load(UXMLElement element) {
            base.Load(element);
            price = new PriceData(element);
        }

        public override InventoryItemType type => InventoryItemType.Food;
        public override bool isUsableFromInventory => true;
        public override bool IsSellable => true;
    }
}
