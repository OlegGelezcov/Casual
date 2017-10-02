using Casual.Ravenhill.Data;
using System.Collections.Generic;

namespace Casual.Ravenhill.Net {
    public class NetGift : IGift {

        private Gift gift = null;
        private string id = null;

        public NetGift(Dictionary<string, object> dict, IResourceService resource) {
            id = dict.GetStringOrDefault("gift_id");

            string senderId = dict.GetStringOrDefault("sender_id");
            string senderName = dict.GetStringOrDefault("sender_name");
            string senderAvatar = dict.GetStringOrDefault("sender_avatar_id");
            int senderLevel = dict.GetIntOrDefault("sender_level");

            string receiverId = dict.GetStringOrDefault("receiver_id");
            string receiverName = dict.GetStringOrDefault("receiver_name");
            string receiverAvatar = dict.GetStringOrDefault("receiver_avatar_id");
            int receiverLevel = dict.GetIntOrDefault("receiver_level");

            InventoryItemType itemType = dict.GetItemType("item_type");
            string itemId = dict.GetStringOrDefault("item_id");

            ISender sender = new NetPlayer(senderId, senderName, senderAvatar, senderLevel, true);
            ISender receiver = new NetPlayer(receiverId, receiverName, receiverAvatar, receiverLevel, true);
            InventoryItemData itemData = resource.GetInventoryItemData(itemType, itemId);
            gift = new Gift(sender, receiver, itemData);

        }

        public string Id => id;

        public InventoryItemData GetItemData() {
            return gift.GetItemData();
        }

        public ISender GetReceiver() {
            return gift.GetReceiver();
        }

        public ISender GetSender() {
            return gift.GetSender();
        }
    }
}
