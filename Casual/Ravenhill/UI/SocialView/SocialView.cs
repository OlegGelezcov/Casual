namespace Casual.Ravenhill.UI {
    using UnityEngine;

    public partial class SocialView : RavenhillCloseableView {

        public override RavenhillViewType viewType => RavenhillViewType.social_view;

        public override bool isModal => true;

        public override int siblingIndex => 2;

        public override void Setup(object objdata = null) {
            base.Setup(objdata);
            chatView.Setup();

        }
    }

    public partial class SocialView : RavenhillCloseableView {

        [SerializeField]
        private ChatView chatView;

    }
}
