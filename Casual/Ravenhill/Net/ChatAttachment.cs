using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Net {

    [Serializable]
    public class ChatAttachment : IAttachment {

        public string id;
        public int item_type;

        public ChatAttachment() {

        }

        public ChatAttachment(InventoryItemData data ) {
            id = data.id;
            item_type = (int)data.type;
        }

        public string GetId() {
            return id;
        }

        public int GetItemType() {
            return item_type;
        }
    }
}
