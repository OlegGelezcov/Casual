using Casual.Ravenhill.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class StoryBuyChargerView : RavenhillUIBehaviour {

        public void Setup(StoryCollectionData data) {
            StoryChargerData chargerData = resourceService.GetStoryCharger(data.chargerId);
            StoreItemData storeItemData = resourceService.GetStoreItem(chargerData.storeItemId);

            if(storeItemData == null ) {
                Debug.Log($"storeItemData null for charger {chargerData.id}:{chargerData.storeItemId}");
                return;
            }

            priceImage.overrideSprite = resourceService.GetPriceSprite(storeItemData.price);
            priceText.text = storeItemData.price.price.ToString();

            buyButton.SetListener(() => {
                playerService.Buy(storeItemData);
            });
            askButton.SetListener(() => {
                netService.Ask(chargerData);
            });
        }
    }

    public partial class StoryBuyChargerView : RavenhillUIBehaviour {

        [SerializeField]
        private Button buyButton;

        [SerializeField]
        private Button askButton;

        [SerializeField]
        private Image priceImage;

        [SerializeField]
        private Text priceText;
    }
}
