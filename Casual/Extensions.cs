using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Casual {
    public static class Extensions {

        public static void ShuffleArray<T>(this T[] array) {
            if(array != null && array.Length > 1) {
                for (int i = array.Length - 1; i > 0; i--) {
                    int j = UnityEngine.Random.Range(0, i + 1);
                    T temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
            }
        }

        public static T Cast<T>(this IService service) where T : class , IService {
            if(service == null) {
                return null;
            }
            return (service as T);
        }

        public static void SetEventTriggerClick(this EventTrigger trigger, UnityAction<BaseEventData> onClick) {
            trigger?.triggers?.Clear();
            if(onClick != null ) {
                EventTrigger.TriggerEvent clickEvent = new EventTrigger.TriggerEvent();
                clickEvent.AddListener(onClick);
                trigger?.triggers?.Add(new EventTrigger.Entry { eventID = EventTriggerType.PointerClick, callback = clickEvent });
            }
        }

        public static void SetListener(this Button button, UnityAction action ) {
            if(button == null ) { return; }

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
        }



        public static T GetOrAdd<T>(this GameObject go) where T : MonoBehaviour {
            if(go == null ) {
                return null;
            }
            T comp = go.GetComponent<T>();
            if(comp == null ) {
                comp = go.AddComponent<T>();
            }
            return comp;
        }

        public static void ActivateSelf(this GameObject go ) {
            if(go == null ) {
                return;
            }
            if(!go.activeSelf ) {
                go.SetActive(true);
            }
        }

        public static void DeactivateSelf(this GameObject go) {
            if(go == null ) {
                return;
            }
            if(go.activeSelf ) {
                go.SetActive(false);
            }
        }

        public static void ActivateSelf(this MonoBehaviour behaviour) {
            if(behaviour && behaviour.gameObject) {
                behaviour.gameObject.ActivateSelf();
            }
        }

        public static void DeactivateSelf(this MonoBehaviour behaviour ) {
            if(behaviour && behaviour.gameObject) {
                behaviour.gameObject.DeactivateSelf();
            }
        }

        public static T GetOrDefault<K, T>(this Dictionary<K, T> dict, K key, T defaultValue = default(T)) {
            T value = default(T);
            if(dict.TryGetValue(key, out value)) {
                return value;
            }
            return defaultValue;
        }
    }
}
