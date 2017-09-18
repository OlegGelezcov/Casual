using Casual.Ravenhill.Data;
using Casual.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class StoreItemView : ListItemView<InventoryItemData> {

        public override void Setup(InventoryItemData data) {
            base.Setup(data);

            iconImage.overrideSprite = resourceService.GetSprite(data);
            nameText.text = resourceService.GetString(data.nameId);

            if (data.hasDescription) {
                descriptionText.text = resourceService.GetString(data.descriptionId);
            } else {
                descriptionText.text = string.Empty;
            }

            priceIconImage.overrideSprite = resourceService.GetPriceSprite(data.price);
            priceText.text = data.price.price.ToString();

            buyButton.SetListener(() => {
                if (playerService.HasCoins(data.price)) {
                    playerService.RemoveCoins(data.price);
                    playerService.AddItem(new InventoryItem(data, 1));
                } else {
                    Debug.Log($"Low coins show bank....");
                }
            }, engine.GetService<IAudioService>());
        }
    }

    public partial class StoreItemView : ListItemView<InventoryItemData> {

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private Text nameText;

        [SerializeField]
        private Text descriptionText;

        [SerializeField]
        private Button buyButton;

        [SerializeField]
        private Image priceIconImage;

        [SerializeField]
        private Text priceText;
    }


}
