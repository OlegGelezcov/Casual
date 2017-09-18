namespace Casual.Ravenhill.UI {
    using Casual.Ravenhill.Data;
    using UnityEngine;
    using UnityEngine.UI;

    public partial class BuyItemView : RavenhillCloseableView {

        private InventoryItemData data = null;

        public override RavenhillViewType viewType => RavenhillViewType.buy_item_view;

        public override bool isModal => true;

        public override int siblingIndex => 2;

        public override void Setup(object objdata = null) {
            base.Setup(objdata);

            data = objdata as InventoryItemData;
            nameText.text = resourceService.GetString(data.nameId);
            descriptionText.text = resourceService.GetString(data.descriptionId);
            iconImage.overrideSprite = resourceService.GetSprite(data);
            priceText.text = data.price.price.ToString();
            priceIconImage.overrideSprite = resourceService.GetPriceSprite(data.price);
            closeBigButton.SetListener(Close, engine.GetService<IAudioService>());
            buyButton.SetListener(() => {
                if( playerService.Buy(data)) {
                    Close();
                }
            }, engine.GetService<IAudioService>());
        }
    }

    public partial class BuyItemView : RavenhillCloseableView {

        [SerializeField]
        private Text nameText;

        [SerializeField]
        private Text descriptionText;

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private Button buyButton;

        [SerializeField]
        private Text priceText;

        [SerializeField]
        private Image priceIconImage;

        [SerializeField]
        private Button closeBigButton;
    }
}
