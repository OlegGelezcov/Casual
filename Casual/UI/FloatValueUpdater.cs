using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.UI {
    public class FloatValueUpdater {

        public float startValue { get; private set; } = 0;
        public float endValue { get; private set; } = 1;
        public float speed { get; private set; } = 1;
        public bool isStarted { get; private set; } = false;
        public MCEaseType easeType { get; private set; } = MCEaseType.Linear;
        private float timer { get; set; } = 0;


        public void Start(float start, float end, float speed, MCEaseType easeType = MCEaseType.Linear) {
            this.startValue = start;
            this.endValue = end;
            this.speed = speed;
            this.easeType = easeType;
            timer = 0;
            isStarted = true;
        }

        public void Start() {
            timer = 0;
            isStarted = true;
        }

        public float normalTimer {
            get {
                return timer / duration;
            }
        }

        public float duration {
            get {
                return Mathf.Abs(endValue - startValue) / speed;
            }
        }



        public Tuple<float, bool> Update() {
            if(isStarted) {
                timer += Time.deltaTime;
                if(timer >= duration ) {
                    isStarted = false;
                }
                timer = Mathf.Clamp(timer, 0.0f, duration);
                float value = MCEasing.Get(easeType)(timer, duration, startValue, endValue);
                return new Tuple<float, bool>(value, isStarted);
            } else {
                return new Tuple<float, bool>(startValue, false);
            }
        }

    }
}
