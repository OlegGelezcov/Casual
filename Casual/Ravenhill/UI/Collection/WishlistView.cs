using Casual.Ravenhill.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class WishlistView : RavenhillUIBehaviour {

        public void Setup() {

            var wishItems = playerService.WishItems;
            for(int i = 0; i < wishItemViews.Length; i++ ) {
                if(i < wishItems.Count) {
                    wishItemViews[i].Setup(wishItems[i]);
                } else {
                    wishItemViews[i].Setup(null);
                }
            }

            if(playerService.WishlistCount > 0 ) {
                shareWishlistButton.interactable = true;
            } else {
                shareWishlistButton.interactable = false;
            }

            shareWishlistButton.SetListener(() => {
                netService.ShareWishlist(wishItems);
            });
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.AddedToWishlist += OnWishItemAdded;
            RavenhillEvents.RemovedFromWishlist += OnWishItemRemoved;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.AddedToWishlist -= OnWishItemAdded;
            RavenhillEvents.RemovedFromWishlist -= OnWishItemRemoved;
        }

        private void OnWishItemAdded(InventoryItemData data) {
            Setup();
        }

        private void OnWishItemRemoved(InventoryItemData data) {
            Setup();
        }
    }

    public partial class WishlistView : RavenhillUIBehaviour {

        [SerializeField]
        private WishItemView[] wishItemViews;

        [SerializeField]
        private Button shareWishlistButton;

    }
}
