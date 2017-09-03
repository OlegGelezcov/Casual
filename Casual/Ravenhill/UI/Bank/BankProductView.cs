using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class BankProductView : RavenhillUIBehaviour {

        public void Setup(IBankContext context ) {
            var product = resourceService.GetBankProduct(productId);

            if(product == null ) {
                throw new UnityException($"dont exists product with id {productId}");
            }

            ButtonState buttonState = null;
            if(product.isBest || product.isPopular ) {
                buttonState = context.BestButtonState;
            } else if(product.price.type == Data.MoneyType.gold ) {
                buttonState = context.GoldButtonState;
            } else {
                buttonState = context.SilverButtonState;
            }

            if(buttonState != null ) {
                
               
                SpriteState spriteState = new SpriteState {
                    disabledSprite = buttonState.disabledSprite,
                    highlightedSprite = buttonState.highlightSprite,
                    pressedSprite = buttonState.activeSprite
                };
                buyButton.spriteState = spriteState;
                buyButton.image.overrideSprite = buttonState.normalSprite;
                buyButton.image.sprite = buttonState.normalSprite;
            }



            priceImage.overrideSprite = resourceService.GetPriceSprite(product.price);

            if(product.isBest) {
                bestImage.ActivateSelf();
                popularImage.DeactivateSelf();
            } else if(product.isPopular ) {
                popularImage.ActivateSelf();
                bestImage.DeactivateSelf();
            } else {
                popularImage.DeactivateSelf();
                bestImage.DeactivateSelf();
            }

            string countString = product.price.price.ToString();
            if(product.bonus > 0 ) {
                countString += $" + {product.bonus}";
            }
            countText.text = countString;

            if(engine.GetService<IOfferService>().IsBankOfferActive ) {
                bonusPan.ActivateSelf();
            } else {
                bonusPan.DeactivateSelf();
            }

            priceText.text = $"{product.realPrice}$";

            buyButton.SetListener(() => {
                engine.GetService<IPurchaseService>().PurchaseProduct(product);
            });
        }
    }

    public partial class BankProductView : RavenhillUIBehaviour {

        [SerializeField]
        private string productId;

        [SerializeField]
        private Button buyButton;

        [SerializeField]
        private Image priceImage;

        [SerializeField]
        private Image bestImage;

        [SerializeField]
        private Image popularImage;

        [SerializeField]
        private Text countText;

        [SerializeField]
        private Image bonusPan;

        [SerializeField]
        private Text priceText;

    }
}
