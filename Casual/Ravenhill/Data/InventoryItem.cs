using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Data {
    public class InventoryItem : IIdObject {
        public InventoryItemData data { get; }
        public int count { get; private set; }

        public string id {
            get {
                return data?.id ?? string.Empty;
            }
        }

        public InventoryItem(InventoryItemData data, int count) {
            this.data = data;
            this.count = count;
        }
    }
}
