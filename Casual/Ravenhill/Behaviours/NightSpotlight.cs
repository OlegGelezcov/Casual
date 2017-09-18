namespace Casual.Ravenhill {
    using UnityEngine;

    public partial class NightSpotlight : RavenhillGameBehaviour {

        private Renderer renderer = null;
        private bool isMouseDragStarted = false;
        private Vector3 prevMousePosition = Vector3.zero;
        private Vector2 spotPosition = Vector2.zero;

        public override void Start() {
            base.Start();
            renderer = GetComponent<Renderer>();
            SetSpot(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f));
        }

        public override void Update() {
            base.Update();
            if(Utility.isEditorOrStandalone) {
                if(Input.GetMouseButtonDown(0)) {
                    isMouseDragStarted = true;
                    prevMousePosition = Input.mousePosition;
                }
                if(isMouseDragStarted) {
                    Vector3 deltaPosition = Input.mousePosition - prevMousePosition;
                    prevMousePosition = Input.mousePosition;
                    MoveSpot(deltaPosition);
                }
                if(Input.GetMouseButtonUp(0)) {
                    isMouseDragStarted = false;
                }
            } else {
                if(Input.touchCount == 1) {
                    Touch touch = Input.GetTouch(0);
                    if(touch.phase == TouchPhase.Moved) {
                        MoveSpot(touch.deltaPosition);
                    }
                }
            }
        }

        private void MoveSpot(Vector2 deltaPosition ) {
            Vector2 newPosition = spotPosition + new Vector2(deltaPosition.x, deltaPosition.y) * moveSpeed;
            newPosition.x = Mathf.Clamp(newPosition.x, 0, Screen.width);
            newPosition.y = Mathf.Clamp(newPosition.y, 0, Screen.height);
            SetSpot(newPosition);
        }

        private void SetSpot(Vector2 pos) {
            Vector2 tPos = TransformCoord(pos);
            renderer.material.SetFloat("_Ratio", (float)Screen.height / (float)Screen.width);
            renderer.material.SetFloat("_U", tPos.x);
            renderer.material.SetFloat("_V", tPos.y);
        }

        private Vector2 TransformCoord(Vector2 pos) {
            spotPosition = pos;
            float x = (pos.x / Screen.width);
            float y = (pos.y / Screen.height);
            //y += 1;
            return new Vector2(x, y);
        }

        public void SetSpotToWorldPosition(Vector3 position) {
            var screenPosition = Camera.main.WorldToScreenPoint(position);
            SetSpot(screenPosition);
        }
    }

    public partial class NightSpotlight : RavenhillGameBehaviour {

        [SerializeField]
        private float moveSpeed = 10.0f;

        [SerializeField]
        private float defaultSpotLightSize = 0.05f;


    }
}
