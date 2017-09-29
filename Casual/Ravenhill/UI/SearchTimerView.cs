using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public class SearchTimerView : RavenhillUIBehaviour, IPauseCounterSource {

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

        private float pauseTimer = 0.0f;
        private float pauseInterval = 0.0f;

        private int countOfPauseOnSession = 0;

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

        public int SessionPoints {
            get {
                float remain = timer;
                if(remain < 0 ) {
                    remain = 0;
                }
                if(countOfPauseOnSession > 0) {
                    int low = Mathf.RoundToInt(5.0f / (countOfPauseOnSession + 1));
                    int high = Mathf.RoundToInt(10.0f / (countOfPauseOnSession + 1));
                    if(low >= high) {
                        high = low + 1;
                    }

                    return Mathf.RoundToInt(remain * UnityEngine.Random.Range(low, high));
                } else {
                    return Mathf.RoundToInt(remain * UnityEngine.Random.Range(5, 10));
                }
            }
        }



        public void SetPause(float interval) {
            countOfPauseOnSession++;
            bool oldPaused = isPaused;
            isPaused = true;
            pauseInterval = interval;
            pauseTimer = pauseInterval;
            timerText.color = Color.red;

            if (oldPaused != isPaused) {
                RavenhillEvents.OnSearchTimerPauseChanged(oldPaused, isPaused, (int)interval);
            }
        }

        public void RemoveFromTimer(int count) {
            timer -= count;
        }

        public bool IsAllowMissPenalty {
            get {
                return (isStarted) && (!isPaused) && (!isStopped) && (!isBreaked);
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
            if(isStarted && isPaused ) {
                pauseTimer -= Time.deltaTime;
                if(pauseTimer <= 0.0f ) {
                    bool oldPaused = isPaused;
                    isPaused = false;
                    if (oldPaused != isPaused) {
                        RavenhillEvents.OnSearchTimerPauseChanged(oldPaused, isPaused, (int)pauseInterval);
                        timerText.color = Color.black;
                    }
                }
            }
        }

        public int GetPauseTimer() {
            int count = Mathf.RoundToInt(pauseTimer);
            if(count < 0 ) {
                count = 0;
            }
            return count;
        }
    }
}
