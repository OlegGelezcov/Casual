using UnityEngine;
namespace Casual.UI {

    public class ImageProgress : MonoBehaviour {

        [SerializeField]
        private float m_FullDuration = 2.0f;



        private ImageAnimFill m_Anim;

        private ImageAnimFill anim {
            get {
                if (m_Anim == null) {
                    m_Anim = gameObject.GetOrAdd<ImageAnimFill>();
                }
                return m_Anim;
            }
        }

        public void SetValue(float to) {
            anim.StopAnim();
            anim.StartAnim(new MCFloatAnimData {
                duration = DurationOnLength(anim.image.fillAmount, to),
                end = to,
                start = anim.image.fillAmount,
                endAction = () => { }
            });
        }

        public void SetValue(float from, float to) {
            anim.StopAnim();
            anim.StartAnim(new MCFloatAnimData {
                duration = DurationOnLength(from, to),
                end = to,
                start = from,
                endAction = () => { }
            });
        }

        private float DurationOnLength(float from, float to) {
            float speed = 1.0f / m_FullDuration;
            return Mathf.Abs(to - from) / speed;
        }
    }
}
