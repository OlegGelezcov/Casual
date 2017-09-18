using Casual.Ravenhill;
using Casual.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Casual
{
    [RequireComponent(typeof(Canvas))]
    public class CanvasService : RavenhillGameBehaviour, ICanvasSerive {

#pragma warning disable 0649
        [SerializeField]
        private FirstSiblingTransform m_FirstSiblingTransform;

        [SerializeField]
        private LastSiblingTransform m_LastSiblingTransform;

        private Canvas canvas = null;
#pragma warning restore 0649

        private FirstSiblingTransform firstSiblingTransform => m_FirstSiblingTransform;
        private LastSiblingTransform lastSiblingTransform => m_LastSiblingTransform;

        private static bool isCreated { get; set; } = false;

        private RectTransform m_RectTransform;

        private RectTransform rectTransform {
            get {
                if(!m_RectTransform) {
                    m_RectTransform = GetComponent<RectTransform>();
                }
                return m_RectTransform;
            }
        }

        public Canvas Canvas {
            get {
                if(canvas == null ) {
                    canvas = GetComponent<Canvas>();
                }
                return canvas;
            }
        }

        public Vector2 TouchPositionToCanvasPosition(Vector2 touchPosition) {
            return new Vector2(
                (touchPosition.x - Screen.width * 0.5f) / Canvas.scaleFactor,
                (touchPosition.y - Screen.height * 0.5f) / Canvas.scaleFactor
                );
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

        public Vector3 GetUIWorldPosition(RectTransform transform ) {
            var pos = RectTransformUtility.CalculateRelativeRectTransformBounds(Canvas.GetComponent<RectTransform>(), transform).center;
            CanvasScaler scaler = Canvas.GetComponent<CanvasScaler>();
            pos.x += scaler.referenceResolution.x / 2;
            pos.y += scaler.referenceResolution.y / 2;
            pos.x = pos.x * (Screen.width / scaler.referenceResolution.x);
            pos.y = pos.y * (Screen.height / scaler.referenceResolution.y);
            pos.z = 0;
            return Camera.main.ScreenToWorldPoint(pos);
        }
    }
}
