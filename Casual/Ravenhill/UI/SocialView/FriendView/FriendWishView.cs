namespace Casual.Ravenhill.UI {
    using Casual.Ravenhill.Data;
    using Casual.Ravenhill.Net;
    using UnityEngine.UI;

    public class FriendWishView : RavenhillUIBehaviour {

        public Image icon;
        public Button giftButton;

        private InventoryItemData itemData;
        private NetPlayer friend;

        public void Setup(NetPlayer friend, InventoryItemData itemData) {
            this.friend = friend;
            this.itemData = itemData;

            int playerCount = playerService.GetItemCount(itemData);

            if(playerCount <= 0 ) {
                icon.SetAlpha(0.5f);
                giftButton.DeactivateSelf();
            } else {
                icon.SetAlpha(1.0f);
                giftButton.ActivateSelf();
                giftButton.SetListener(() => {
                    Gift gift = new Gift(engine.GetService<INetService>().LocalPlayer, friend, itemData);
                    engine.GetService<INetService>().SendGift(gift);
                    giftButton.DeactivateSelf();
                });
            }
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.GiftSendedSuccess += OnGiftSended;
            RavenhillEvents.InventoryChanged += OnInventoryChanged;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.GiftSendedSuccess -= OnGiftSended;
            RavenhillEvents.InventoryChanged -= OnInventoryChanged;
        }

        private void OnGiftSended(IGift gift ) {
            if(IsSetupCompleted) {
                if(gift.GetItemData() != null ) {
                    if(itemData.id == gift.GetItemData().id ) {
                        Resetup();
                    }
                }
            }
        }

        private void OnInventoryChanged(InventoryItemType type, string itemId, int count ) {
            if(IsSetupCompleted) {
                if(itemData.type == type && itemData.id == itemId ) {
                    Resetup();
                }
            }
        }

        private bool IsSetupCompleted {
            get {
                return (itemData != null) && (friend != null);
            }
        }

        private void Resetup() {
            Setup(friend, itemData);
        }

    }
}
