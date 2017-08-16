using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.UI {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class GraphicAnimAlpha : BaseAnim {

        private Graphic m_Graphic;

        public override void StartAnim(MCFloatAnimData data) {
            m_Graphic = GetComponent<Graphic>();
            m_Graphic.color = new Color(m_Graphic.color.r, m_Graphic.color.g, m_Graphic.color.b, data.start);

            base.StartAnim(data);
        }

        protected override void OnAnimationUpdate(float value) {
            base.OnAnimationUpdate(value);
            m_Graphic.color = new Color(m_Graphic.color.r, m_Graphic.color.g, m_Graphic.color.b, value);
        }

        protected override void OnAnimationEnd() {
            base.OnAnimationEnd();
            m_Graphic.color = new Color(m_Graphic.color.r, m_Graphic.color.g, m_Graphic.color.b, data.end);
        }
    }
}
