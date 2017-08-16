using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public class SearchTimerView : RavenhillUIBehaviour {

        [SerializeField]
        private Text m_TimerText;

        public bool isStarted { get; private set; } = false;
        public  bool isPaused { get; private set; } = false;
        private int duration { get; set; }
        public float timer { get; private set; }

        private Text timerText => m_TimerText;

        public override string listenerName => "search_timer_view";

        public void StartTimer(int duration) {
            this.duration = duration;
            this.timer = duration;
            isStarted = true;
        }

        public void SetPause(float interval) {
            StartCoroutine(CorSetPause(interval));
        }

        private System.Collections.IEnumerator CorSetPause(float interval) {
            isPaused = true;
            yield return new WaitForSeconds(interval);
            isPaused = false;
        }

        public override void Update() {
            base.Update();

            if(isStarted && !isPaused ) {
                timer -= Time.deltaTime;
                if(timer <= 0.0f ) {
                    timer = 0.0f;
                    isStarted = false;
                    isPaused = false;
                }
                timerText.text = Utility.FormatMS(timer);
            }
        }
    }
}
