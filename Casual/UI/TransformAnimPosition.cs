using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.UI {
    public class TransformAnimPosition : VectorBaseAnim {

        private Transform m_Transform;

        public override void StartAnim(MCVectorAnimData data) {
            m_Transform = transform;
            m_Transform.localPosition = data.start;
            base.StartAnim(data);
        }

        protected override void OnAnimationUpdate(Vector3 value) {
            base.OnAnimationUpdate(value);
            m_Transform.localPosition = value;
        }

        protected override void OnAnimationEnd() {
            base.OnAnimationEnd();
            m_Transform.localPosition = data.end;
        }
    }
}
