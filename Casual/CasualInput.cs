using Casual.Ravenhill;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Casual {

    public abstract class CasualInput : GameBehaviour {

        [SerializeField]
        private float m_MaxTouchDistance = 100.0f;

        [SerializeField]
        private float m_MaxTouchInterval = 0.8f;

        private bool m_IsTouchStarted = false;

        private Vector2 m_TouchStartPosition;
        private Vector2 m_TouchEndPosition;
        private float m_TouchTimer = 0.0f;

        public Vector2 lastPointerPosition { get; private set; } = Vector2.zero;

        private List<RaycastResult> raycastResults { get; } = new List<RaycastResult>();

        private float maxTouchDistance {
            get {
                return m_MaxTouchDistance;
            }
        }

        private float maxTouchInterval {
            get {
                return m_MaxTouchInterval;
            }
        }

        protected abstract void SendTouchEvent(Vector2 position);
//            {
//#if RAVENHILL
//            CasualEngine.instance.GetService<RavenhillEventService>()?.SendEvent(new TouchEventArgs(position));

//#else
//            throw new NotImplementedException("CasualInput not implemented");
//#endif
//        }

        public override void Update() {
            base.Update();

            if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor ||
                Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer ) {
                if(Input.GetMouseButtonUp(0)) {
                    raycastResults.Clear();
                    PointerEventData pointerEventData = new PointerEventData(null);
                    pointerEventData.position = Input.mousePosition;
                    lastPointerPosition = Input.mousePosition;

                    if (EventSystem.current != null) {
                        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
                        if (raycastResults.Count == 0) {
                            SendTouchEvent(Input.mousePosition);
                        }
                    }
                }
            } else if(Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android ) {
                if(m_IsTouchStarted) {
                    m_TouchTimer += Time.deltaTime;
                }

                if(Input.touchCount > 0 ) {
                    Touch touch = Input.GetTouch(0);
                    lastPointerPosition = touch.position;

                    if(!m_IsTouchStarted) {
                        raycastResults.Clear();
                        PointerEventData pointerEventData = new PointerEventData(null);
                        pointerEventData.position = touch.position;

                        if (EventSystem.current) {
                            EventSystem.current.RaycastAll(pointerEventData, raycastResults);
                            if (raycastResults.Count == 0) {
                                if (touch.phase == TouchPhase.Began) {
                                    m_IsTouchStarted = true;
                                    m_TouchStartPosition = touch.position;
                                    m_TouchTimer = 0.0f;
                                    return;
                                }
                            }
                        }
                    }

                    if(m_IsTouchStarted) {
                        if(touch.phase == TouchPhase.Ended ) {
                            m_IsTouchStarted = false;
                            m_TouchEndPosition = touch.position;
                            float distance = Vector2.Distance(m_TouchStartPosition, m_TouchEndPosition);
                            if(distance < maxTouchDistance && m_TouchTimer < maxTouchInterval) {
                                SendTouchEvent(touch.position);
                                m_TouchTimer = 0.0f;
                            }
                        }

                        if(touch.phase == TouchPhase.Canceled ) {
                            m_IsTouchStarted = false;
                            m_TouchTimer = 0.0f;
                        }
                    }
                }
            }
        }
    }
}
