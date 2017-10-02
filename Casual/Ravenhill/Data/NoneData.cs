using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Data {
    public class NoneData : InventoryItemData {
        public override InventoryItemType type => InventoryItemType.None;

        public override bool isUsableFromInventory => false;

        public override bool IsSellable => false;

        public override PriceData price {
            get => PriceData.None;
            protected set { }
        }

    }
}
