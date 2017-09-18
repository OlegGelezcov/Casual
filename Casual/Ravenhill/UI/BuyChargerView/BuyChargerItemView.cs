using Casual.Ravenhill.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class BuyChargerItemView  : RavenhillUIBehaviour {

        private ChargerData data;
        private CollectionData collectionData;

        private readonly UpdateTimer wishlistUpdateTimer = new UpdateTimer();

        public void Setup(ChargerData data, CollectionData collectionData) {

            this.data = data;
            this.collectionData = collectionData;

            nameText.text = resourceService.GetName(data);
            iconImage.overrideSprite = resourceService.GetSprite(data);

            int requiredCount = collectionData.GetChargerCount(data.id);
            int playerCount = playerService.GetItemCount(data);

            if(playerCount >= requiredCount) {
                buyButton.DeactivateSelf();
                availableLabel.ActivateSelf();
            } else {
                buyButton.ActivateSelf();
                availableLabel.DeactivateSelf();
                priceImage.overrideSprite = resourceService.GetPriceSprite(data.price);
                priceText.text = data.price.price.ToString();

                buyButton.SetListener(() => {
                    playerService.Buy(data);
                }, engine.GetService<IAudioService>());
            }

            countText.text = $"You have {playerCount} from {requiredCount}";

            wishlistUpdateTimer.Setup(.5f, UpdateWishlistButton);
            wishlistButton.SetListener(() => {
                if(!playerService.IsWishlistContains(data) && !playerService.IsWishlistFull) {
                    playerService.AddToWishlist(data);
                    Resetup();
                }
            }, engine.GetService<IAudioService>());
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.InventoryChanged += OnInventoryChanged;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.InventoryChanged -= OnInventoryChanged;
        }

        private void OnInventoryChanged(InventoryItemType type, string itemId, int count ) {
            if(type == InventoryItemType.Charger ) {
                Resetup();
            }
        }
        private void Resetup() {
            if(data != null && collectionData != null ) {
                Setup(data, collectionData);
            }
        }

        public override void Update() {
            base.Update();
            wishlistUpdateTimer.Update();
        }

        private void UpdateWishlistButton(float realDelta) {
            wishlistButton.interactable = IsWishlistButtonInteractable;
        }

        private bool IsWishlistButtonInteractable {
            get {
                return (data != null) && (!playerService.IsWishlistContains(data)) && (!playerService.IsWishlistFull);
            }
        }
    }

    public partial class BuyChargerItemView : RavenhillUIBehaviour {

        [SerializeField]
        private Text nameText;

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private Button buyButton;

        [SerializeField]
        private Text priceText;

        [SerializeField]
        private Image priceImage;

        [SerializeField]
        private Text availableLabel;

        [SerializeField]
        private Text countText;

        [SerializeField]
        private Button wishlistButton;
    }
}
