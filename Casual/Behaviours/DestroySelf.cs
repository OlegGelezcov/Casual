using UnityEngine;

namespace Casual {
    public class DestroySelf : MonoBehaviour {

        [SerializeField]
        private float interval = 1.0f;

        public void Start() {
            if(interval <= 0.0f ) {
                Destroy(gameObject);
            } else {
                Destroy(gameObject, interval);
            }
        }
    }
}
