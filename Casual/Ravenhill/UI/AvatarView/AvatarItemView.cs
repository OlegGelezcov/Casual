namespace Casual.Ravenhill.UI {
    using UnityEngine;
    using UnityEngine.UI;

    public partial class AvatarItemView : RavenhillUIBehaviour {

        public string AvatarId => avatarId;

        public void Setup() {
            toggle.SetListener((isOn) => {
                if(isOn) {
                    playerService.SetAvatar(resourceService.GetAvatarData(AvatarId));
                }
            }, engine.GetService<IAudioService>());
        }

        public void Select() {
            toggle.isOn = true;
        }
    }

    public partial class AvatarItemView : RavenhillUIBehaviour {

        [SerializeField]
        private string avatarId;

        [SerializeField]
        private Toggle toggle;
    }
}
