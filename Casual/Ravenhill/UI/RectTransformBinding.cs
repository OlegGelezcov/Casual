using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.Ravenhill.UI {
    public class RectTransformBinding : RavenhillGameBehaviour {

        [SerializeField]
        private float m_UpdateInterval = 0.2f;
        private RectTransform m_RectTransform;

        private Transform parent { get; set; }
        private Vector2 offset { get; set; }

        private RectTransform rectTransform {
            get {
                if(!m_RectTransform) {
                    m_RectTransform = GetComponent<RectTransform>();
                }
                return m_RectTransform;
            }
        }

        private float timer { get; set; }

        private float updateInterval => m_UpdateInterval;

        public void Bind(Transform parent, Vector2 offset, float updateInterval ) {
            this.parent = parent;
            this.offset = offset;
            this.m_UpdateInterval = updateInterval;
        }

        public void SetOffset(Vector2 offset) {
            this.offset = offset;
        }

        public void Unbind() {
            parent = null;
        }

        public override void Update() {
            base.Update();
            timer += Time.deltaTime;

            if(timer >= updateInterval ) {
                timer -= updateInterval;
                if(parent != null ) {
                    rectTransform.anchoredPosition = canvasService.WorldToCanvasPoint(parent.position) + offset;
                }
            }
        }

    }
}
