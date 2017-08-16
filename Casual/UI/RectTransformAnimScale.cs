namespace Casual.UI {
    using UnityEngine;

    public class RectTransformAnimScale : BaseAnim {


        private RectTransform m_RectTransform;

        public override void StartAnim(MCFloatAnimData data) {
            m_RectTransform = GetComponent<RectTransform>();
            m_RectTransform.localScale = new Vector3(data.start, data.start, 1);
            base.StartAnim(data);
        }

        protected override void OnAnimationUpdate(float value) {
            base.OnAnimationUpdate(value);
            m_RectTransform.localScale = new Vector3(value, value, 1);
        }

        protected override void OnAnimationEnd() {
            base.OnAnimationEnd();
            m_RectTransform.localScale = new Vector3(data.end, data.end, 1);
        }
    }
}
