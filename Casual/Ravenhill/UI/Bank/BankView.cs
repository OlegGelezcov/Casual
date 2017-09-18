using UnityEngine;

namespace Casual.Ravenhill.UI {
    public partial class BankView : RavenhillCloseableView, IBankContext {

        public override RavenhillViewType viewType => RavenhillViewType.bank;

        public override bool isModal => true;

        public override int siblingIndex => 4;

        public ButtonState GoldButtonState => goldButtonState;

        public ButtonState SilverButtonState => silverButtonState;

        public ButtonState BestButtonState => bestButtonState;

        public override void Setup(object data = null) {
            base.Setup(data);

            foreach(var productView in productViews) {
                productView.Setup(this);
            }

            foreach(var specialProductView in specialProductViews ) {
                specialProductView.Setup();
            }
        }
    }

    public partial class BankView : RavenhillCloseableView, IBankContext {

        [SerializeField]
        private ButtonState goldButtonState = new ButtonState();

        [SerializeField]
        private ButtonState silverButtonState = new ButtonState();

        [SerializeField]
        private ButtonState bestButtonState = new ButtonState();

        [SerializeField]
        private BankProductView[] productViews;

        [SerializeField]
        private BankSpecialProductView[] specialProductViews;

    }
}
