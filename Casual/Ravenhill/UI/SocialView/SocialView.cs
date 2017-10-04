namespace Casual.Ravenhill.UI {
    using UnityEngine;
    using UnityEngine.UI;

    public partial class SocialView : RavenhillCloseableView {

        public override RavenhillViewType viewType => RavenhillViewType.social_view;

        public override bool isModal => true;

        public override int siblingIndex => 2;

        public override void Setup(object objdata = null) {
            base.Setup(objdata);

            chatToggle.SetListener((isOn) => {
                if (isOn) {
                    chatView.ActivateSelf();
                    chatView.Setup();
                } else {
                    chatView.DeactivateSelf();
                }
            });

            giftsToggle.SetListener((isOn) => {
                if (isOn) {
                    giftsView.ActivateSelf();
                    giftsView.Setup();
                } else {
                    giftsView.DeactivateSelf();
                }
            });

            friendsToggle.SetListener((isOn) => {
                if(isOn) {
                    friendsView.ActivateSelf();
                    friendsView.Setup();
                } else {
                    friendsView.DeactivateSelf();
                }
            });
            chatToggle.isOn = true;
        }
    }

    public partial class SocialView : RavenhillCloseableView {

        [SerializeField]
        private ChatView chatView;

        [SerializeField]
        private SocialGiftsView giftsView;

        [SerializeField]
        private FriendsView friendsView;

        [SerializeField]
        private Toggle chatToggle;

        [SerializeField]
        private Toggle giftsToggle;

        [SerializeField]
        private Toggle friendsToggle;
    }
}
