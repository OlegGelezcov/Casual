using UnityEngine;

namespace Casual.Ravenhill {
    public class UpdateTimer {
        
        public float duration { get; private set; }
        
        private System.Action action { get; set; }

        private float timer { get; set; }

        public void Setup(float duration, System.Action action ) {
            this.duration = duration;
            this.action = action;
            timer = this.duration;
        }

        public void Update() {
            timer -= Time.deltaTime;
            if(timer <= 0.0f ) {
                timer += duration;
                action?.Invoke();
            }
        }
    }
}
