using Casual.Ravenhill.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class WishItemView : RavenhillUIBehaviour {

        public void Setup(InventoryItemData data ) {
            if(data == null ) {
                iconImage.overrideSprite = resourceService.transparent;
                removeWishButton.DeactivateSelf();
            } else {
                iconImage.overrideSprite = resourceService.GetSprite(data);
                removeWishButton.ActivateSelf();
                removeWishButton.SetListener(() => {
                    playerService.RemoveFromWishlist(data);
                }, engine.GetService<IAudioService>());
            }
        }
    }

    public partial class WishItemView : RavenhillUIBehaviour {

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private Button removeWishButton;


    }
}
