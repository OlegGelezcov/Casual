using Casual.Ravenhill;
using Casual.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual {
    [RequireComponent(typeof(Canvas))]
    public class CanvasService : RavenhillBaseListenerBehaviour, ICanvasSerive {

        [SerializeField]
        private FirstSiblingTransform m_FirstSiblingTransform;

        [SerializeField]
        private LastSiblingTransform m_LastSiblingTransform;

        private FirstSiblingTransform firstSiblingTransform => m_FirstSiblingTransform;
        private LastSiblingTransform lastSiblingTransform => m_LastSiblingTransform;

        private static bool isCreated { get; set; } = false;

        public override string listenerName => "canvas";

        private RectTransform m_RectTransform;

        private RectTransform rectTransform {
            get {
                if(!m_RectTransform) {
                    m_RectTransform = GetComponent<RectTransform>();
                }
                return m_RectTransform;
            }
        }

        public override void Awake() {

            if (isCreated) {
                Destroy(gameObject);
                return;
            } else {
                DontDestroyOnLoad(gameObject);
                base.Awake();
                isCreated = true;
            }
        }

        public void Add(Transform view) {
            view.SetParent(transform, false);
        }

        public void AddToFirstGroup(Transform view) {
            if(firstSiblingTransform) {
                view.SetParent(firstSiblingTransform.transform, false);
            }
        }

        public void AddToLastGroup(Transform view) {
            if(lastSiblingTransform) {
                view.SetParent(lastSiblingTransform.transform, false);
            }
        }

        public void RestoreSiblings() {
            firstSiblingTransform?.transform?.SetAsFirstSibling();
            lastSiblingTransform?.transform?.SetAsLastSibling();
        }

        public void Setup(object data) { }


        public Vector2 WorldToCanvasPoint(Vector3 position, Camera camera = null) {
            if(camera == null ) {
                camera = Camera.main;
            }
            if(camera != null ) {
                Vector2 viewportPosition = camera.WorldToViewportPoint(position);
                Vector2 canvasUnscaledPosition = new Vector2(
                    viewportPosition.x * rectTransform.sizeDelta.x - rectTransform.sizeDelta.x * 0.5f,
                    viewportPosition.y * rectTransform.sizeDelta.y - rectTransform.sizeDelta.y * 0.5f
                    );
                return canvasUnscaledPosition;
            }
            return Vector2.zero;
        }
    }
}
