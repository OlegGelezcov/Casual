using UnityEngine;

namespace Casual.Ravenhill {
    public class UpdateTimer {
        
        public float duration { get; private set; }
        
        private System.Action<float> action { get; set; }

        private float timer { get; set; }

        private float realDelta = 0.0f;


        public void Setup(float duration, System.Action<float> action ) {
            this.duration = duration;
            this.action = action;
            timer = this.duration;
        }

        public void Update() {
            realDelta += Time.deltaTime;
            timer -= Time.deltaTime;
            if(timer <= 0.0f ) {
                timer += duration;
                action?.Invoke(realDelta);
                realDelta = 0.0f;
            }
        }
    }
}
