namespace Casual.Ravenhill.UI {
    using Casual.Ravenhill.Data;
    using UnityEngine;
    using UnityEngine.UI;

    public class NameIconView : RavenhillUIBehaviour {

        [SerializeField]
        protected Image iconImage;

        [SerializeField]
        protected Text nameText;

        public virtual void Setup(IconData objData) {

            if (iconImage != null) {
                iconImage.overrideSprite = resourceService.GetSprite(objData);
            }
            if (nameText != null) {
                nameText.text = resourceService.GetString(objData.nameId);
            }
        }
    }
}
