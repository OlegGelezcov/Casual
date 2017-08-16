namespace Casual.UI {
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Image))]
    public class ImageAnimFill : BaseAnim {

        private Image m_Image;

        public Image image {
            get {
                if (m_Image == null) {
                    m_Image = GetComponent<Image>();
                }
                return m_Image;
            }
        }

        public override void StartAnim(MCFloatAnimData data) {
            image.fillAmount = data.start;
            base.StartAnim(data);
        }

        protected override void OnAnimationUpdate(float value) {
            base.OnAnimationUpdate(value);
            image.fillAmount = value;
        }

        protected override void OnAnimationEnd() {
            base.OnAnimationEnd();
            image.fillAmount = data.end;
        }

    }
}
