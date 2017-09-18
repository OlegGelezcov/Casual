namespace Casual.Ravenhill.UI {
    using UnityEngine;
    using UnityEngine.UI;

    public partial class ActionButton : RavenhillUIBehaviour  {

        public void Setup(string buttonName, System.Action buttonAction ) {
            button.SetListener(() => {
                buttonAction?.Invoke();
            }, engine.GetService<IAudioService>());
            if(nameText != null ) {
                nameText.text = buttonName;
            }
        }
    }

    public partial class ActionButton : RavenhillUIBehaviour {

        [SerializeField]
        private Button button;

        [SerializeField]
        private Text nameText;
    }
}
