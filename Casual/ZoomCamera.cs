using UnityEngine;

namespace Casual
{
    [RequireComponent(typeof(Camera))]
    public class ZoomCamera : GameBehaviour {

        public enum AutoZoomDirection {
            ZoomIn,
            ZoomOut
        }

        [SerializeField]
        private float m_PanSpeed = 8f;

        [SerializeField]
        private float m_PanSmooth = 0.2f;

        [SerializeField]
        private float m_ZoomSmooth = 0.3f;

        [SerializeField]
        private float m_ZoomSpeed = 100f;

        [SerializeField]
        private float m_ZoomLimitUp = 850.0f;

        [SerializeField]
        private float m_ZoomLimitDown = 350.0f;

        [SerializeField]
        private float m_XLimitForZoomDown = -888.0f;

        [SerializeField]
        private float m_YLimitForZoomDown = -909.0f;

        [SerializeField]
        private float m_XLimitForZoomDownInSearch = -400;

        [SerializeField]
        private float m_YLimitForZoomDownInSearch = -312;

        [SerializeField]
        private float m_ZoomAnimateInterval = 1.0f;

        [SerializeField]
        private bool m_IsZoomActive = true;

        private AutoZoomDirection m_ZoomDirection = AutoZoomDirection.ZoomIn;
        private float m_TargetOrthoSize;
        private Vector3 m_TargetPosition;
        private Vector2 m_PanPrevPosition;
        private Vector3 m_PanVelocity;
        private float m_ZoomVelocity;
        private Camera m_CachedCamera;
        private bool m_IsZoomEnabled = true;
        private bool m_IsZoomAnimated = false;
        private float m_ZoomAnimateSpeed;


        private float zoomLimitDown {
            get {
                return m_ZoomLimitDown;
            }
        }

        private float zoomLimitUp {
            get {
                return m_ZoomLimitUp;
            }
        }
        private Camera cachedCamera {
            get {
                if(!m_CachedCamera) {
                    m_CachedCamera = GetComponent<Camera>();
                }
                return m_CachedCamera;
            }
        }

        private float zoomDifference {
            get {
                return (zoomLimitUp - cachedCamera.orthographicSize);
            }
        }

        private float zoomSpeed {
            get {
                if(Application.platform == RuntimePlatform.Android ) {
                    return m_ZoomSpeed * 10.0f;
                }
                return m_ZoomSpeed;
            }
        }

        private float targetOrthoSize {
            get {
                return m_TargetOrthoSize;
            }
            set {
                m_TargetOrthoSize = value;
            }
        }

        private Vector3 targetPosition {
            get {
                return m_TargetPosition;
            }
            set {
                m_TargetPosition = value;
            }
        }

        private float panSpeed {
            get {
                if(Application.platform == RuntimePlatform.Android ) {
                    return m_PanSpeed * 7.0f;
                }
                return m_PanSpeed;
            }
        }

        private AutoZoomDirection zoomDirection {
            get {
                return m_ZoomDirection;
            }
            set {
                m_ZoomDirection = value;
            }
        }

        private float targetZoomAnimatedSize {
            get {
                if(zoomDirection == AutoZoomDirection.ZoomIn) {
                    return zoomLimitDown;
                }
                return zoomLimitUp;
            }
        }

        private float zoomAnimateInterval {
            get {
                return m_ZoomAnimateInterval;
            }
        }

        private float zoomAnimateSpeed {
            get => m_ZoomAnimateSpeed;
            set => m_ZoomAnimateSpeed = value;
        }

        private Vector2 panPrevPosition {
            get => m_PanPrevPosition;
            set => m_PanPrevPosition = value;
        }

        private float panSmooth { get => m_PanSmooth; }

        private bool isZoomEnabled {
            get => m_IsZoomEnabled;
            set => m_IsZoomEnabled = value;
        }



        private float zoomSmooth {
            get {
                return m_ZoomSmooth;
            }
        }

        private bool isZoomAnimated {
            get => m_IsZoomAnimated;
            set => m_IsZoomAnimated = value;
        }

        private bool isZoomActive {
            get => m_IsZoomActive;
        }

        private float xLimitForZoomDownInSearch {
            get {
                return m_XLimitForZoomDownInSearch;
            }
        }

        private float xLimitForZoomDown {
            get => m_XLimitForZoomDown;
        }

        private float yLimitForZoomDownInSearch {
            get {
                return m_YLimitForZoomDownInSearch;
            }
        }

        private float yLimitForZoomDown {
            get => m_YLimitForZoomDown;
        }

        private float xLimitByGameMode {
            get {
                if(gameModeService.gameModeName == GameModeName.search ) {
                    return xLimitForZoomDownInSearch;
                }
                return xLimitForZoomDown;
            }
        }

        private float yLimitByGameMode {
            get {
                if(gameModeService.gameModeName == GameModeName.search) {
                    return yLimitForZoomDownInSearch;
                }
                return yLimitForZoomDown;
            }
        }

        private float xLimit {
            get {
                float x2 = xLimitByGameMode;
                float x1 = 0.0f;
                float t2 = zoomLimitDown;
                float t1 = zoomLimitUp;
                return ((x2 - x1) / (t2 - t1)) * (cachedCamera.orthographicSize - t1) + x1;
            }
        }

        private float yLimit {
            get {
                float y2 = yLimitByGameMode;
                float y1 = 0.0f;
                float t2 = zoomLimitDown;
                float t1 = zoomLimitUp;
                return ((y2 - y1) / (t2 - t1)) * (cachedCamera.orthographicSize - t1) + y1;
            }
        }

        public override void Start() {
            base.Start();
            targetOrthoSize = cachedCamera.orthographicSize;
            targetPosition = cachedCamera.transform.position;
        }

        public override void Update() {
            base.Update();
            Vector3 newPosition = transform.position;

            if(Utility.isEditorOrStandalone) {
                if(Input.GetMouseButtonDown(0)) {
                    panPrevPosition = Input.mousePosition;
                }
                if(Input.GetMouseButtonUp(1)) {
                    StartZoomAnimation();
                }
                if(Input.GetMouseButton(0)) {
                    Vector2 deltaPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - panPrevPosition;
                    panPrevPosition = Input.mousePosition;
                    Vector3 currentPosition = transform.position;
                    Vector3 nextPosition = transform.position + new Vector3(deltaPosition.x, deltaPosition.y, 0.0f) * panSpeed;
                    newPosition = Vector3.SmoothDamp(currentPosition, nextPosition, ref m_PanVelocity, panSmooth);
                }

                if(isZoomEnabled && (!viewService.hasModals)) {
                    float scrollValue = Input.GetAxis("Mouse ScrollWheel");
                    float newOrthoSize = cachedCamera.orthographicSize + scrollValue * zoomSpeed * Time.deltaTime;
                    float orthoSize = Mathf.SmoothDamp(cachedCamera.orthographicSize, newOrthoSize, ref m_ZoomVelocity, zoomSmooth);
                    orthoSize = Mathf.Clamp(orthoSize, zoomLimitDown, zoomLimitUp);

                    if(!isZoomAnimated) {
                        cachedCamera.orthographicSize = orthoSize;
                    }
                }
            } else {
                if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved && (!viewService.hasModals)) {
                    Touch touch = Input.GetTouch(0);
                    Vector3 difference = touch.deltaPosition * panSpeed;
                    targetPosition = transform.position + new Vector3(-difference.x, -difference.y, 0.0f);
                }

                if(isZoomActive) {
                    if(Input.touchCount == 2 && (!viewService.hasModals)) {
                        Touch touchZero = Input.GetTouch(0);
                        Touch touchOne = Input.GetTouch(1);
                        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                        if(isZoomAnimated) {
                            deltaMagnitudeDiff = 0.0f;
                        }

                        targetOrthoSize = cachedCamera.orthographicSize + deltaMagnitudeDiff * zoomSpeed * Time.deltaTime;
                    }
                }
            }

            if(isZoomAnimated) {
                float currentValue = cachedCamera.orthographicSize + zoomAnimateSpeed * Time.deltaTime;
                switch(zoomDirection) {
                    case AutoZoomDirection.ZoomIn: {
                            if(currentValue <= zoomLimitDown) {
                                currentValue = zoomLimitDown;
                                isZoomAnimated = false;
                                zoomDirection = AutoZoomDirection.ZoomOut;
                            }
                        }
                        break;
                    case AutoZoomDirection.ZoomOut: {
                            if(currentValue >= zoomLimitUp ) {
                                currentValue = zoomLimitUp;
                                isZoomAnimated = false;
                                zoomDirection = AutoZoomDirection.ZoomIn;
                            }
                        }
                        break;
                }
                cachedCamera.orthographicSize = currentValue;
                targetOrthoSize = cachedCamera.orthographicSize;

            } else {
                if(isZoomActive) {
                    float northoSize = Mathf.SmoothDamp(cachedCamera.orthographicSize, targetOrthoSize, ref m_ZoomVelocity, zoomSmooth);
                    northoSize = Mathf.Clamp(northoSize, zoomLimitDown, zoomLimitUp);
                    cachedCamera.orthographicSize = northoSize;
                }
            }

            newPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref m_PanVelocity, panSmooth);
            float xLim = Mathf.Abs(xLimit);
            float yLim = Mathf.Abs(yLimit);
            newPosition.x = Mathf.Clamp(newPosition.x, -xLim, xLim);
            newPosition.y = Mathf.Clamp(newPosition.y, -yLim, yLim);
            transform.position = newPosition;
        }

        private void StartZoomAnimation() {
            float timeRatio = Mathf.Abs(targetZoomAnimatedSize - cachedCamera.orthographicSize) / (zoomLimitUp - zoomLimitDown);
            float interval = zoomAnimateInterval * timeRatio;
            if(Mathf.Approximately(interval, 0.0f)) {
                interval = 0.1f;
            }
            zoomAnimateSpeed = (targetZoomAnimatedSize - cachedCamera.orthographicSize) / interval;
        }


    }
}
