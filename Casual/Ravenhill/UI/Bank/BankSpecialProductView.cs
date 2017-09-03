using Casual.Ravenhill.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class BankSpecialProductView : RavenhillUIBehaviour {

        public void Setup() {
            BankProductData product = resourceService.GetBankProduct(productId);

            if (product == null) {
                throw new UnityException($"Not exists product with id {productId}");
            }

            if (!string.IsNullOrEmpty(product.nameId)) {
                nameText.text = resourceService.GetString(product.nameId);
            } else {
                nameText.text = string.Empty;
            }

            if (!string.IsNullOrEmpty(product.discountData.id)) {
                iconImage.overrideSprite = resourceService.GetSprite(product.discountData.id, product.discountData.iconPath);
                oldPriceText.text = $"{product.discountData.oldPrice}$";
            } else {
                iconImage.overrideSprite = resourceService.transparent;
                oldPriceText.text = string.Empty;
            }

            for (int i = 0; i < rewardDescriptionTexts.Length; i++) {
                if (i < product.rewards.Count) {

                    var dropItem = product.rewards[i];
                    if (dropItem.type == DropType.item && dropItem.itemData != null) {
                        rewardDescriptionTexts[i].text = $"{dropItem.count} {resourceService.GetString(dropItem.itemData.nameId)}";
                    } else {
                        rewardDescriptionTexts[i].text = string.Empty;
                    }
                } else {
                    rewardDescriptionTexts[i].text = string.Empty;
                }
            }

            priceText.text = $"{product.realPrice}$";
            buyButton.SetListener(() => {
                engine.GetService<IPurchaseService>().PurchaseProduct(product);
            });
        }
    }

    public partial class BankSpecialProductView : RavenhillUIBehaviour {

        [SerializeField]
        private string productId;

        [SerializeField]
        private Text nameText;

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private Text[] rewardDescriptionTexts;

        [SerializeField]
        private Text oldPriceText;

        [SerializeField]
        private Button buyButton;

        [SerializeField]
        private Text priceText;
    }
}
