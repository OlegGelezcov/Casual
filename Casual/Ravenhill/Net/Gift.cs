using Casual.Ravenhill.Data;

namespace Casual.Ravenhill.Net {

    public class Gift : IGift {

        private InventoryItemData itemData = null;
        private ISender receiver = null;
        private ISender sender = null;

        public Gift(ISender sender, ISender receiver, InventoryItemData itemData) {
            this.sender = sender;
            this.receiver = receiver;
            this.itemData = itemData;
        }

        public InventoryItemData GetItemData() {
            return itemData;
        }

        public ISender GetReceiver() {
            return receiver;
        }

        public ISender GetSender() {
            return sender;
        }
    }
}
