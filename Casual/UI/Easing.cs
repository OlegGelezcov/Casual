using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.UI {

    public enum MCEaseType {
        Linear,
        EaseInQuad,
        EaseOutQuad,
        EaseInOutQuad,
        EaseInCubic,
        EaseOutCubic,
        EaseInOutCubic,
        EaseInQuart,
        EaseOutQuart,
        EaseInOutQuart,
        EaseInQuint,
        EaseOutQuint,
        EaseInOutQuint,
        EaseInSine,
        EaseOutSine,
        EaseInOutSine,
        EaseInExpo,
        EaseOutExpo,
        EaseInOutExpo,
        EaseInCirc,
        EaseOutCirc,
        EaseInOutCirc
    }

    public delegate float EasingFunction(float time, float duration, float start, float end);

    public static class MCEasing {


        public static EasingFunction Get(MCEaseType type) {
            switch (type) {
                case MCEaseType.Linear:
                    return Linear;
                case MCEaseType.EaseInQuad:
                    return EaseInQuad;
                case MCEaseType.EaseOutQuad:
                    return EaseOutQuad;
                case MCEaseType.EaseInOutQuad:
                    return EaseInOutQuad;
                case MCEaseType.EaseInCubic:
                    return EaseInCubic;
                case MCEaseType.EaseOutCubic:
                    return EaseOutCubic;
                case MCEaseType.EaseInOutCubic:
                    return EaseInOutCubic;
                case MCEaseType.EaseInQuart:
                    return EaseInQuart;
                case MCEaseType.EaseOutQuart:
                    return EaseOutQuart;
                case MCEaseType.EaseInOutQuart:
                    return EaseInOutQuart;
                case MCEaseType.EaseInQuint:
                    return EaseInQuint;
                case MCEaseType.EaseOutQuint:
                    return EaseOutQuint;
                case MCEaseType.EaseInOutQuint:
                    return EaseInOutQuint;
                case MCEaseType.EaseInSine:
                    return EaseInSine;
                case MCEaseType.EaseOutSine:
                    return EaseOutSine;
                case MCEaseType.EaseInOutSine:
                    return EaseInOutSine;
                case MCEaseType.EaseInExpo:
                    return EaseInExpo;
                case MCEaseType.EaseOutExpo:
                    return EaseOutExpo;
                case MCEaseType.EaseInOutExpo:
                    return EaseInOutExpo;
                case MCEaseType.EaseInCirc:
                    return EaseInCirc;
                case MCEaseType.EaseOutCirc:
                    return EaseOutCirc;
                case MCEaseType.EaseInOutCirc:
                    return EaseInOutCirc;
                default:
                    return Linear;
            }
        }

        public static float Linear(float time, float duration, float start, float end) {
            float c = end - start;
            return c * time / duration + start;
        }

        public static float EaseInQuad(float time, float duration, float start, float end) {
            float c = end - start;
            time /= duration;
            return c * time * time + start;
        }

        public static float EaseOutQuad(float time, float duration, float start, float end) {
            float c = end - start;
            time /= duration;
            return -c * time * (time - 2) + start;
        }

        public static float EaseInOutQuad(float time, float duration, float start, float end) {
            float c = end - start;
            time /= (duration * 0.5f);
            if (time < 1.0f) {
                return c * time * time * 0.5f + start;
            }
            time--;
            return -c * 0.5f * (time * (time - 2) - 1) + start;
        }

        public static float EaseInCubic(float time, float duration, float start, float end) {
            float c = end - start;
            time /= duration;
            return c * time * time * time + start;
        }

        public static float EaseOutCubic(float time, float duration, float start, float end) {
            float c = end - start;
            time /= duration;
            time--;
            return c * (time * time * time + 1) + start;
        }

        public static float EaseInOutCubic(float time, float duration, float start, float end) {
            float c = end - start;
            time /= (duration * 0.5f);
            if (time < 1.0f) {
                return c * 0.5f * time * time * time + start;
            }
            time -= 2.0f;
            return c * 0.5f * (time * time * time + 2.0f) + start;
        }

        public static float EaseInQuart(float time, float duration, float start, float end) {
            float c = end - start;
            time /= duration;
            return c * time * time * time * time + start;
        }

        public static float EaseOutQuart(float time, float duration, float start, float end) {
            float c = end - start;
            time /= duration;
            time--;
            return -c * (time * time * time * time - 1) + start;
        }

        public static float EaseInOutQuart(float time, float duration, float start, float end) {
            float c = end - start;
            time /= (duration * 0.5f);
            if (time < 1.0f) {
                return c * 0.5f * time * time * time * time + start;
            }
            time -= 2.0f;
            return -c * 0.5f * (time * time * time * time - 2.0f) + start;
        }

        public static float EaseInQuint(float time, float duration, float start, float end) {
            float c = end - start;
            time /= duration;
            return c * time * time * time * time * time + start;
        }

        public static float EaseOutQuint(float time, float duration, float start, float end) {
            float c = end - start;
            time /= duration;
            time--;
            return c * (time * time * time * time * time + 1) + start;
        }

        public static float EaseInOutQuint(float time, float duration, float start, float end) {
            float c = end - start;
            time /= (duration * 0.5f);
            if (time < 1.0f) {
                return c * 0.5f * time * time * time * time * time + start;
            }
            time -= 2.0f;
            return c * 0.5f * (time * time * time * time * time + 2.0f) + start;
        }

        public static float EaseInSine(float time, float duration, float start, float end) {
            float c = end - start;
            return -c * Mathf.Cos(time * Mathf.PI * 0.5f / duration) + c + start;
        }

        public static float EaseOutSine(float time, float duration, float start, float end) {
            float c = end - start;
            return c * Mathf.Sin(time * Mathf.PI * 0.5f / duration) + start;
        }

        public static float EaseInOutSine(float time, float duration, float start, float end) {
            float c = end - start;
            return -c * 0.5f * (Mathf.Cos(Mathf.PI * time / duration) - 1.0f) + start;
        }

        public static float EaseInExpo(float time, float duration, float start, float end) {
            float c = end - start;
            return c * Mathf.Pow(2.0f, 10.0f * (time / duration - 1.0f)) + start;
        }

        public static float EaseOutExpo(float time, float duration, float start, float end) {
            float c = end - start;
            return c * (-Mathf.Pow(2.0f, -10.0f * time / duration) + 1.0f) + start;
        }

        public static float EaseInOutExpo(float time, float duration, float start, float end) {
            float c = end - start;
            time /= (duration * 0.5f);
            if (time < 1.0f) {
                return c * 0.5f * Mathf.Pow(2.0f, 10.0f * (time - 1.0f)) + start;
            }
            time--;
            return c * 0.5f * (-Mathf.Pow(2.0f, -10.0f * time) + 2.0f) + start;
        }

        public static float EaseInCirc(float time, float duration, float start, float end) {
            float c = end - start;
            time /= duration;
            return -c * (Mathf.Sqrt(1.0f - time * time) - 1.0f) + start;
        }

        public static float EaseOutCirc(float time, float duration, float start, float end) {
            float c = end - start;
            time /= duration;
            time--;
            return c * Mathf.Sqrt(1.0f - time * time) + start;
        }

        public static float EaseInOutCirc(float time, float duration, float start, float end) {
            float c = end - start;
            time /= (duration * 0.5f);
            if (time < 1.0f) {
                return -c * 0.5f * (Mathf.Sqrt(1.0f - time * time) - 1.0f) + start;
            }
            time -= 2.0f;
            return c * 0.5f * (Mathf.Sqrt(1.0f - time * time) + 1.0f) + start;
        }
    }
}
