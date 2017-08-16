namespace Casual.UI {
    using UnityEngine;

    public class RectTransformAnimPosition : VectorBaseAnim {

        private RectTransform m_RectTransform;

        public override void StartAnim(MCVectorAnimData data) {
            m_RectTransform = GetComponent<RectTransform>();
            m_RectTransform.anchoredPosition = data.start;

            base.StartAnim(data);
        }

        protected override void OnAnimationUpdate(Vector3 value) {
            base.OnAnimationUpdate(value);
            m_RectTransform.anchoredPosition = value;
        }

        protected override void OnAnimationEnd() {
            base.OnAnimationEnd();
            m_RectTransform.anchoredPosition = data.end;
        }
    }

}
