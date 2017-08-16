using UnityEngine;

namespace Casual.UI {

    public class NumericTextProgress : MonoBehaviour {

        [SerializeField]
        private float m_Speed = 10.0f;

        [SerializeField]
        private float m_MaxDuration = 3.0f;

        public string prefix { get; set; } = string.Empty;
        public string postfix { get; set; } = string.Empty;
        public string zeroFormat { get; set; } = string.Empty;

        private float speed {
            get {
                if(Mathf.Approximately(m_Speed, 0.0f )) {
                    m_Speed = 1.0f;
                }
                return m_Speed;
            }
        }

        private float maxDuration => m_MaxDuration;

        private NumericTextAnim m_Anim;

        private NumericTextAnim anim {
            get {
                if(m_Anim == null ) {
                    m_Anim = gameObject.GetOrAdd<NumericTextAnim>();
                }
                return m_Anim;
            }
        }

        public void SetValue(int to ) {
            anim.StopAnim();
            anim.StartAnim(new NumericTextAnimData {
                duration = GetDuration(anim.textValue, to),
                start = Utility.TryParseInt(anim.textValue),
                end = to,
                endAction = () => { },
                postfix = postfix,
                prefix = prefix,
                zeroFormatString = zeroFormat
            });
        }

        public void SetValue(int from, int to) {
            anim.StopAnim();
            anim.StartAnim(new NumericTextAnimData {
                duration = GetDuration(from, to),
                start = from,
                end = to,
                endAction = () => { },
                postfix = postfix,
                prefix = prefix,
                zeroFormatString = zeroFormat
            });
        }

        private float GetDuration(string from, int to ) {
            return GetDuration(Utility.TryParseInt(from), to);
        }

        private float GetDuration(int from, int to ) {
            float timer = (float)Mathf.Abs(to - from) / speed;
            timer = Mathf.Clamp(timer, 0.0f, maxDuration);
            return timer;
        }
    }
}
