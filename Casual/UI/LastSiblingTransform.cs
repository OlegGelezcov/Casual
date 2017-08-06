using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.UI {
    public class LastSiblingTransform : GameBehaviour {

        [SerializeField]
        private float m_UpdateInterval = 0.2f;
        private float m_Timer;
        private Transform m_CachedTransform;


        private float updateInterval => m_UpdateInterval;
        private Transform cachedTransform => m_CachedTransform;
        private int siblingIndex { get; set; } = 0;

        public override void Start() {
            base.Start();
            m_CachedTransform = transform;
            m_Timer = updateInterval;
            siblingIndex = cachedTransform.GetSiblingIndex();
        }

        public override void Update() {
            base.Update();
            m_Timer -= Time.deltaTime;

            if(m_Timer <= 0.0f ) {
                m_Timer += updateInterval;
                if(cachedTransform.parent != null ) {
                    int parentCount = cachedTransform.parent.childCount;
                    if(siblingIndex != (parentCount - 1)) {
                        cachedTransform.SetSiblingIndex(parentCount - 1);
                        siblingIndex = cachedTransform.GetSiblingIndex();
                    }
                }
            }
        }
    }
}
