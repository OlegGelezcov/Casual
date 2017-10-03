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
            }, engine.GetService<IAudioService>());

            giftsToggle.SetListener((isOn) => {
                if (isOn) {
                    giftsView.ActivateSelf();
                    giftsView.Setup();
                } else {
                    giftsView.DeactivateSelf();
                }
            }, engine.GetService<IAudioService>());

            chatToggle.isOn = true;
        }
    }

    public partial class SocialView : RavenhillCloseableView {

        [SerializeField]
        private ChatView chatView;

        [SerializeField]
        private SocialGiftsView giftsView;

        [SerializeField]
        private Toggle chatToggle;

        [SerializeField]
        private Toggle giftsToggle;
    }
}
