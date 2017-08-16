using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.UI {
    public class ColoredObjectAnim : BaseColorAnim {

        private ColoredObject m_ColoredObject;


        public override void StartAnim(ColorAnimData data) {
            m_ColoredObject = GetComponent<ColoredObject>();
            m_ColoredObject.color = data.start;
            base.StartAnim(data);
        }

        protected override void OnAnimationUpdate(Color value) {
            base.OnAnimationUpdate(value);
            m_ColoredObject.color = value;
        }

        protected override void OnAnimationEnd() {
            base.OnAnimationEnd();
            m_ColoredObject.color = data.end;
        }
    }
}
