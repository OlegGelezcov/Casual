using Casual.Ravenhill.Data;
using Casual.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class InventoryItemView : ListItemView<InventoryItemData> {

        public override void Setup(InventoryItemData data) {
            base.Setup(data);

            iconImage.overrideSprite = resourceService.GetSprite(data);
            nameText.text = resourceService.GetString(data.nameId);

            if (data.hasDescription) {
                descriptionText.text = resourceService.GetString(data.descriptionId);
            } else {
                descriptionText.text = string.Empty;
            }

            int playerCount = playerService.GetItemCount(data);
            if(playerCount > 0 ) {
                countPanObj.ActivateSelf();
                countText.text = playerCount.ToString();
            } else {
                countText.text = string.Empty;
                countPanObj.DeactivateSelf();
            }

            if(playerCount > 0 ) {
                


                if(data.isUsableFromInventory ) {
                    buyButton.DeactivateSelf();
                    priceIconImage.DeactivateSelf();
                    priceText.DeactivateSelf();

                    useButton.ActivateSelf();
                    useButton.SetListener(() => {
                        playerService.UseItem(data);
                    });
                } else {
                    useButton.DeactivateSelf();
                    SetupBuySection();
                }
            } else {
                useButton.DeactivateSelf();

                if (data.IsSellable) {
                    SetupBuySection();
                } else {
                    buyButton.DeactivateSelf();
                    priceIconImage.DeactivateSelf();
                    priceText.DeactivateSelf();
                }
            }
        }

        private void SetupBuySection() {
            buyButton.ActivateSelf();
            priceIconImage.ActivateSelf();
            priceText.ActivateSelf();

            priceIconImage.overrideSprite = resourceService.GetPriceSprite(data.price);
            priceText.text = data.price.price.ToString();
            buyButton.SetListener(() => {
                Debug.Log($"buy item {data.type}-{data.id}");
                playerService.Buy(data);
            });
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.InventoryChanged += OnInventoryChanged;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.InventoryChanged -= OnInventoryChanged;
        }

        private void OnInventoryChanged(InventoryItemType type, string id, int count ) {
            if(data != null ) {
                if(data.type == type && data.id == id ) {
                    Setup(data);
                }
            }
        }
    }

    public partial class InventoryItemView : ListItemView<InventoryItemData> {

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private Text nameText;

        [SerializeField]
        private Text descriptionText;

        [SerializeField]
        private Text countText;

        [SerializeField]
        private GameObject countPanObj;

        [SerializeField]
        private Button buyButton;

        [SerializeField]
        private Button useButton;

        [SerializeField]
        private Image priceIconImage;

        [SerializeField]
        private Text priceText;
    }
}
