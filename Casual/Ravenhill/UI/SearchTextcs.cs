using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public class SearchTextcs : GameBehaviour {

        [SerializeField]
        private Text m_Text;

        [SerializeField]
        private Image m_StrokeImage;

        [SerializeField]
        private float m_StokeSpeed = 1.0f;


        private Text text => m_Text;
        private Image strokeImage => m_StrokeImage;
        private float strokeSpeed => m_StokeSpeed;

        public void Clear() {
            if(text != null) {
                text.text = string.Empty;
            }
            if(strokeImage != null ) {
                strokeImage.fillAmount = 0.0f;
            }
        }

        public void Setup(SearchObjectData data) {
            Clear();

        }
    }
}
