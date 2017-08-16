using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.UI {
    public class TransformAnimScale : BaseAnim {

        private Transform m_Transform;


        public override void StartAnim(MCFloatAnimData data) {
            m_Transform = transform;
            m_Transform.localScale = new Vector3(data.start, data.start, data.start);

            base.StartAnim(data);
        }

        protected override void OnAnimationUpdate(float value) {
            base.OnAnimationUpdate(value);
            m_Transform.localScale = new Vector3(value, value, value);
        }

        protected override void OnAnimationEnd() {
            base.OnAnimationEnd();
            m_Transform.localScale = new Vector3(data.end, data.end, data.end);
        }
    }
}
