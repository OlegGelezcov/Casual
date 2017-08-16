using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.UI {

    [RequireComponent(typeof(Text))]
    public class NumericTextAnim : BaseAnim {

        private Text m_Text;

        public Text text {
            get {
                if(m_Text == null ) {
                    m_Text = GetComponent<Text>();
                }
                return m_Text;
            }
        }

        public string textValue {
            get {
                return text.text;
            }
        }

        private string GetText(NumericTextAnimData data, float value) {
            string numStr = string.Empty;
            if(string.IsNullOrEmpty(data.zeroFormatString)) {
                numStr = Mathf.RoundToInt(value).ToString();
            } else {
                numStr = Mathf.RoundToInt(value).ToString(data.zeroFormatString);
            }

            return data.prefix + numStr + data.postfix;
        }

        public override void StartAnim(MCFloatAnimData data) {
            text.text = GetText(data as NumericTextAnimData, data.start);
            base.StartAnim(data);
        }

        protected override void OnAnimationUpdate(float value) {
            base.OnAnimationUpdate(value);
            text.text = GetText(data as NumericTextAnimData, value);
        }

        protected override void OnAnimationEnd() {
            base.OnAnimationEnd();
            text.text = GetText(data as NumericTextAnimData, data.end);
        }
    }
}
