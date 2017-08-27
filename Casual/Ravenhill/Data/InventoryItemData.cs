using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Data {
    public abstract class InventoryItemData : IconData, ISellable {

        public abstract InventoryItemType type { get; }
        public abstract bool isUsableFromInventory { get; }
        public abstract bool IsSellable { get; }
        public abstract PriceData price { get; protected set; }
    }
}
