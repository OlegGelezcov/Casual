namespace Casual.Ravenhill {
    using UnityEngine.Events;
    using UnityEngine.UI;

    public static class RavenhillExtensions {

        public static void SetListener(this Button button, UnityAction action) {
            button.SetListener(action, CasualEngine.Get<RavenhillEngine>().GetService<IAudioService>());
        }

        public static void SetListener(this Toggle toggle, UnityAction<bool> action) {
            toggle.SetListener(action, CasualEngine.Get<RavenhillEngine>().GetService<IAudioService>());
        }
    }
}
