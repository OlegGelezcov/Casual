using Casual.Ravenhill.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class CollectableItemView : RavenhillUIBehaviour {

        private CollectableData data;

        public void Setup(CollectableData data) {
            this.data = data;
            nameText.text = resourceService.GetString(data.nameId);
            iconImage.overrideSprite = resourceService.GetSprite(data);

            int playerCount = playerService.GetItemCount(data);
            if(playerCount > 0 ) {
                iconImage.color = nonEmptyCellColor;
                countPanObj.ActivateSelf();
                countText.text = playerCount.ToString();
            } else {
                iconImage.color = emptyCellColor;
                countText.text = string.Empty;
                countPanObj.DeactivateSelf();
            }

            if(playerService.IsWishlistContains(data) || playerService.IsWishlistFull ) {
                addWishButton.interactable = false;
            }  else {
                addWishButton.interactable = true;
            }

            addWishButton.SetListener(() => {
                playerService.AddToWishlist(data);
            }, engine.GetService<IAudioService>());

            if(playerCount > 0 ) {
                sendGiftButton.interactable = true;
            } else {
                sendGiftButton.interactable = false;
            }

            sendGiftButton.SetListener(() => {
                Debug.Log("Show Friend Selection View");
            }, engine.GetService<IAudioService>());
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.AddedToWishlist += OnWishlistAdded;
            RavenhillEvents.RemovedFromWishlist += OnWishlistRemoved;
            RavenhillEvents.InventoryChanged += OnInventoryChanged;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.AddedToWishlist -= OnWishlistAdded;
            RavenhillEvents.RemovedFromWishlist -= OnWishlistRemoved;
            RavenhillEvents.InventoryChanged -= OnInventoryChanged;
        }

        private void OnWishlistAdded(InventoryItemData wishdata) {
            if(data != null ) {
                if (wishdata.type == InventoryItemType.Collectable) {
                    if (data.id == wishdata.id) {
                        Setup(data);
                    }
                }
            }
        }

        private void OnWishlistRemoved(InventoryItemData wishdata) {
            if(data != null ) {
                if (wishdata.type == InventoryItemType.Collectable) {
                    if (wishdata.id == data.id) {
                        Setup(data);
                    }
                }
            }
        }

        private void OnInventoryChanged(InventoryItemType type, string id, int count ) {
            if(data != null && data.type == type && data.id == id ) {
                Setup(data);
            }
        }
    }



    public partial class CollectableItemView : RavenhillUIBehaviour {

        [SerializeField]
        private Text nameText;

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private GameObject countPanObj;

        [SerializeField]
        private Text countText;

        [SerializeField]
        private Button addWishButton;

        [SerializeField]
        private Button sendGiftButton;

        [SerializeField]
        private Color emptyCellColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);

        [SerializeField]
        private Color nonEmptyCellColor = Color.white;
    }
}
