using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.UI {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class VectorBaseAnim : MonoBehaviour {
        [SerializeField]
        private MCEaseType m_EaseType;

        protected bool m_Started = false;
        private EasingFunction m_CachedFunction;
        protected float m_Timer;
        private MCVectorAnimData m_Data;

        private EasingFunction easeFunction {
            get {
                return MCEasing.Get(m_EaseType);
            }
        }

        protected MCEaseType easeType {
            get {
                return m_EaseType;
            }
        }

        public virtual void StartAnim(MCVectorAnimData data) {
            m_Data = data;
            m_CachedFunction = easeFunction;
            m_Timer = 0.0f;
            m_Data.ResetTimedActions();
            m_Started = true;
        }

        protected bool started {
            get {
                return m_Started;
            }
        }

        protected MCVectorAnimData data {
            get {
                return m_Data;
            }
        }

        public virtual void Update() {
            if (started) {
                m_Timer += Time.deltaTime;
                float clampedTimer = Mathf.Clamp(m_Timer, 0.0f, m_Data.duration);
                float xval = m_CachedFunction(clampedTimer, m_Data.duration, m_Data.start.x, m_Data.end.x);
                float yval = m_CachedFunction(clampedTimer, m_Data.duration, m_Data.start.y, m_Data.end.y);
                float zval = m_CachedFunction(clampedTimer, m_Data.duration, m_Data.start.z, m_Data.end.z);
                OnAnimationUpdate(new Vector3(xval, yval, zval));

                float normTimer = Mathf.Clamp01(m_Timer / m_Data.duration);
                data.CompleteTimedActions(normTimer);

                if (m_Timer >= m_Data.duration) {
                    if (m_Data.endAction != null) {
                        m_Data.endAction();
                    }
                    StopAnim();
                    OnAnimationEnd();
                }
            }
        }

        protected virtual void StopAnim() {
            m_Started = false;
        }

        protected virtual void OnAnimationUpdate(Vector3 value) {

        }

        protected virtual void OnAnimationEnd() {

        }
    }


    public abstract class BaseColorAnim : MonoBehaviour {
        [SerializeField]
        private MCEaseType m_EaseType;

        protected bool m_Started = false;
        private EasingFunction m_CachedFunction;
        protected float m_Timer;
        private ColorAnimData m_Data;

        private EasingFunction easeFunction {
            get {
                return MCEasing.Get(m_EaseType);
            }
        }

        protected MCEaseType easeType {
            get {
                return m_EaseType;
            }
        }

        public virtual void StartAnim(ColorAnimData data ) {
            m_Data = data;
            if (data.overwriteEasing != null) {
                m_EaseType = data.overwriteEasing.type;
            }
            m_CachedFunction = easeFunction;
            m_Timer = 0.0f;
            m_Data.ResetTimedActions();
            m_Started = true;
        }

        protected bool started {
            get {
                return m_Started;
            }
        }

        protected ColorAnimData data {
            get {
                return m_Data;
            }
        }

        public virtual void StopAnim() {
            m_Started = false;
        }

        public virtual void Update() {
            if (started) {
                m_Timer += Time.deltaTime;
                float clampedTimer = Mathf.Clamp(m_Timer, 0.0f, m_Data.duration);
                float rval = m_CachedFunction(clampedTimer, m_Data.duration, m_Data.start.r, m_Data.end.r);
                float gval = m_CachedFunction(clampedTimer, m_Data.duration, m_Data.start.g, m_Data.end.g);
                float bval = m_CachedFunction(clampedTimer, m_Data.duration, m_Data.start.b, m_Data.end.b);
                float aval = m_CachedFunction(clampedTimer, m_Data.duration, m_Data.start.a, m_Data.end.a);
                OnAnimationUpdate(new Color(rval, gval, bval, aval));

                float normTimer = Mathf.Clamp01(m_Timer / m_Data.duration);
                data.CompleteTimedActions(normTimer);

                if (m_Timer >= m_Data.duration) {
                    if (m_Data.endAction != null) {
                        m_Data.endAction();
                    }
                    StopAnim();
                    OnAnimationEnd();
                }
            }
        }

        protected virtual void OnAnimationUpdate(Color value) {

        }

        protected virtual void OnAnimationEnd() {

        }
    }

    public abstract class BaseAnim : MonoBehaviour {

        [SerializeField]
        private MCEaseType m_EaseType;

        protected bool m_Started = false;
        private EasingFunction m_CachedFunction;
        protected float m_Timer;
        private MCFloatAnimData m_Data;




        private EasingFunction easeFunction {
            get {
                return MCEasing.Get(m_EaseType);
            }
        }

        protected MCEaseType easeType {
            get {
                return m_EaseType;
            }
        }

        public virtual void StartAnim(MCFloatAnimData data) {

            m_Data = data;
            if (data.overwriteEasing != null) {
                m_EaseType = data.overwriteEasing.type;
            }
            m_CachedFunction = easeFunction;
            m_Timer = 0.0f;
            m_Data.ResetTimedActions();
            m_Started = true;
        }

        protected bool started {
            get {
                return m_Started;
            }
        }

        protected MCFloatAnimData data {
            get {
                return m_Data;
            }
        }

        public virtual void StopAnim() {
            m_Started = false;
        }

        public virtual void Update() {
            if (started) {
                m_Timer += Time.deltaTime;
                float clampedTimer = Mathf.Clamp(m_Timer, 0.0f, m_Data.duration);
                float val = m_CachedFunction(clampedTimer, m_Data.duration, m_Data.start, m_Data.end);
                OnAnimationUpdate(val);
                float normTimer = Mathf.Clamp01(m_Timer / m_Data.duration);
                data.CompleteTimedActions(normTimer);
                if (m_Timer >= m_Data.duration) {
                    if (m_Data.endAction != null) {
                        m_Data.endAction();
                    }
                    StopAnim();
                    OnAnimationEnd();
                }
            }
        }

        protected virtual void OnAnimationUpdate(float value) {

        }

        protected virtual void OnAnimationEnd() {

        }

    }

    public class TimedAction {
        public float timerPercent;
        public System.Action action;

        public bool isCompleted { get; private set; }

        public void Reset() {
            isCompleted = false;
        }

        public void Complete() {
            if(!isCompleted ) {
                action?.Invoke();
                isCompleted = true;
            }
        }
    }

    public class MCBaseAnimData {
        public float duration;
        public System.Action endAction;
        public List<TimedAction> timedActions = new List<TimedAction>();

        public void ResetTimedActions() {
            foreach(TimedAction timedAction in timedActions ) {
                timedAction.Reset();
            }
        }

        public void CompleteTimedActions(float percent) {
            foreach(TimedAction timedAction in timedActions ) {
                if (percent >= timedAction.timerPercent) {
                    if (!timedAction.isCompleted) {
                        timedAction.Complete();
                    }
                }
            }
        }
    }

    public class OverwriteEasing {
        public MCEaseType type;
    }

    public class ColorAnimData : MCBaseAnimData {
        public Color start;
        public Color end;
        public OverwriteEasing overwriteEasing;
    }

    public class MCFloatAnimData : MCBaseAnimData {
        public float start;
        public float end;
        public OverwriteEasing overwriteEasing;
    }

    public class MCVectorAnimData : MCBaseAnimData {
        public Vector3 start;
        public Vector3 end;
    }

    public class NumericTextAnimData : MCFloatAnimData {
        public string prefix = string.Empty;
        public string postfix = string.Empty;
        public string zeroFormatString = string.Empty;
    }
}
