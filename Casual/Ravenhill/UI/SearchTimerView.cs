using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public class SearchTimerView : RavenhillUIBehaviour {

#pragma warning disable 0649
        [SerializeField]
        private Text m_TimerText;
#pragma warning restore 0649
        public bool isStarted { get; private set; } = false;
        public  bool isPaused { get; private set; } = false;
        private int duration { get; set; }
        public float timer { get; private set; }

        private Text timerText => m_TimerText;

        private bool isStopped { get; set; } = false;
        public bool isBreaked { get; set; } = false;

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.ViewAdded += OnViewAdded;
            RavenhillEvents.ViewRemoved += OnViewRemoved;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.ViewAdded -= OnViewAdded;
            RavenhillEvents.ViewRemoved -= OnViewRemoved;
        }

        private void OnViewRemoved(RavenhillViewType viewType) {
            isStopped = viewService.hasModals;
        }

        private void OnViewAdded(RavenhillViewType viewType ) {
            isStopped = viewService.hasModals;
        }

        public void StartTimer(int duration) {
            this.duration = duration;
            this.timer = duration;
            isStarted = true;
        }

        public float searchTime {
            get {
                return duration - timer;
            }
        }



        public void SetPause(float interval) {
            StartCoroutine(CorSetPause(interval));
        }

        private System.Collections.IEnumerator CorSetPause(float interval) {
            bool oldPaused = isPaused;
            isPaused = true;

            if (oldPaused != isPaused) {
                RavenhillEvents.OnSearchTimerPauseChanged(oldPaused, isPaused, (int)interval);
            }

            yield return new WaitForSeconds(interval);
            oldPaused = isPaused;
            isPaused = false;
            if(oldPaused != isPaused ) {
                RavenhillEvents.OnSearchTimerPauseChanged(oldPaused, isPaused, (int)interval);
            }
        }

        public override void Update() {
            base.Update();

            if(isStarted && !isPaused && (!isStopped) && (!isBreaked)) {
                timer -= Time.deltaTime;
                if(timer <= 0.0f ) {
                    timer = 0.0f;
                    isStarted = false;
                    isPaused = false;
                    RavenhillEvents.OnSearchTimerCompleted();
                }
                timerText.text = Utility.FormatMS(timer);
            }
        }
    }
}
