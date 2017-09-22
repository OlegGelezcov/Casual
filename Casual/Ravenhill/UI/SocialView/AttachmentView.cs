namespace Casual.Ravenhill.UI {
    using Casual.Ravenhill.Data;
    using Casual.Ravenhill.Net;
    using UnityEngine;
    using UnityEngine.UI;

    public partial class AttachmentView : RavenhillUIBehaviour {

        private InventoryItemData itemData = null;
        private ChatMessage message = null;
        private ISender sender = null;

        public void Setup(ChatMessage message, ChatAttachment attachment) {
            itemData = resourceService.GetInventoryItemData((InventoryItemType)attachment.item_type, attachment.id);
            this.message = message;
            this.sender = message.GetSender();

            if(itemData != null ) {
                iconImage.overrideSprite = resourceService.GetSprite(itemData);
                nameText.text = resourceService.GetString(itemData.nameId);
                giftButton.SetListener(() => {
                    netService.SendGift(new Gift(netService.LocalPlayer, sender, itemData));
                }, engine.GetService<IAudioService>());

                UpdateIconImage(sender, itemData);
                UpdateGiftButtonState(sender, itemData);
            }
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.InventoryChanged += OnInventoryChanged;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.InventoryChanged -= OnInventoryChanged;
        }

        private void OnInventoryChanged(InventoryItemType itemType, string itemId, int count ) {
            if(itemData != null && message != null && sender != null ) {
                if(itemData.type == itemType && itemData.id == itemId ) {
                    UpdateIconImage(sender, itemData);
                    UpdateGiftButtonState(sender, itemData);
                }
            }
        }

        private void UpdateIconImage(ISender sender, InventoryItemData itemData) {
            if(playerService.GetItemCount(itemData) > 0 ) {
                iconImage.color = Color.white;
            } else {
                iconImage.color = new Color(1, 1, 1, 0.5f);
            }
        }

        private void UpdateGiftButtonState(ISender sender, InventoryItemData itemData) {
            switch(GetAttachmentState(sender, itemData)) {
                case AttachmentState.IsLocalPlayerWishItem: {
                        giftButton.DeactivateSelf();
                    }
                    break;
                case AttachmentState.IsRemoteGiftWhatLocalDontHave: {
                        giftButton.ActivateSelf();
                        giftButton.interactable = false;
                    }
                    break;
                case AttachmentState.IsRemoteGiftWhatLocalHave: {
                        giftButton.ActivateSelf();
                        giftButton.interactable = true;
                    }
                    break;
            }
        }

        private AttachmentState GetAttachmentState(ISender sender, InventoryItemData itemData) {
            INetService netService = engine.GetService<INetService>();
            IInventory inventory = playerService;

            if(netService.IsLocalPlayer(sender)) {
                return AttachmentState.IsLocalPlayerWishItem;
            } else {
                if(inventory.GetItemCount(itemData) <= 0 ) {
                    return AttachmentState.IsRemoteGiftWhatLocalDontHave;
                } else {
                    return AttachmentState.IsRemoteGiftWhatLocalHave;
                }
            }
        }

        public enum AttachmentState {
            IsLocalPlayerWishItem,
            IsRemoteGiftWhatLocalDontHave,
            IsRemoteGiftWhatLocalHave
        }


    }

    public partial class AttachmentView : RavenhillUIBehaviour {

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private Text nameText;

        [SerializeField]
        private Button giftButton;

    }
}
