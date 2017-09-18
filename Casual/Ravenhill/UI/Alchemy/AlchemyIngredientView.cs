using System;

namespace Casual.Ravenhill.UI {
    using Casual.Ravenhill.Data;
    using UnityEngine;
    using UnityEngine.UI;

    public partial class AlchemyIngredientView : NameIconView {

        private IngredientData data = null;

        private readonly UpdateTimer wishButtonTimer = new UpdateTimer();

        public override void Setup(IconData objdata) {
            base.Setup(objdata);

            data = objdata as IngredientData;
            if(data == null ) {
                throw new ArgumentException(typeof(IngredientData).Name);
            }

            int playerCount = playerService.GetItemCount(data);
            BonusData bonusData = resourceService.GetBonus(data.bonusId);
            int requiredCount = bonusData.GetIngredientCount(data.id);
            Color color = Color.black;
            if(playerCount < requiredCount ) {
                color = Color.red;
            }
            countText.text = $"{playerCount}/{requiredCount}";
            countText.color = color;

            if(playerCount < requiredCount) {
                iconImage.color = new Color(1, 1, 1, 0.5f);
            } else {
                iconImage.color = Color.white;
            }

            buyButton.SetListener(() => {
                PriceData price = data.price;
                if(playerService.HasCoins(price)) {
                    playerService.RemoveCoins(price);
                    playerService.AddItem(new InventoryItem(data, 1));
                } else {
                    viewService.ShowView(RavenhillViewType.bank);
                }
            }, engine.GetService<IAudioService>());

            wishButtonTimer.Setup(0.5f, (delta) => {
                if (data != null) {
                    wishButton.interactable = (!playerService.IsWishlistFull && !playerService.IsWishlistContains(data));
                } else {
                    wishButton.interactable = false;
                }
            });


            wishButton.SetListener(() => {
                playerService.AddToWishlist(data);
                wishButton.interactable = false;
            }, engine.GetService<IAudioService>());
        }

        public override void Update() {
            base.Update();
            if(data != null ) {
                wishButtonTimer.Update();
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

        private void OnInventoryChanged(InventoryItemType type, string itemId, int count ) {
            if(data != null ) {
                if(type == data.type && itemId == data.id ) {
                    Setup(data);
                }
            }
        }
    }

    public partial class AlchemyIngredientView : NameIconView {

    
        [SerializeField]
        private Text countText;

        [SerializeField]
        private Button buyButton;

        [SerializeField]
        private Button wishButton;
    }
}
