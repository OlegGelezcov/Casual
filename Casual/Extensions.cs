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

        private static readonly Dictionary<ColorType, string> s_ColorTypeMap = new Dictionary<ColorType, string> {
            { ColorType.aqua, "#00ffffff" },
            { ColorType.black, "#000000ff" },
            { ColorType.blue, "#0000ffff" },
            { ColorType.brown, "#a52a2aff" },
            { ColorType.darkblue, "#0000a0ff" },
            { ColorType.fuchsia, "#ff00ffff" },
            { ColorType.green, "#008000ff"},
            { ColorType.grey, "#808080ff" },
            { ColorType.lightblue, "#add8e6ff" },
            { ColorType.lime, "#00ff00ff" },
            { ColorType.magenta, "#ff00ffff" },
            { ColorType.maroon, "#800000ff" },
            { ColorType.navy, "#000080ff" },
            { ColorType.olive, "#808000ff" },
            { ColorType.orange, "#ffa500ff" },
            { ColorType.purple, "#800080ff" },
            { ColorType.red, "#ff0000ff" },
            { ColorType.silver, "#c0c0c0ff" },
            { ColorType.teal, "#008080ff" },
            { ColorType.white, "#ffffffff" },
            { ColorType.yellow, "#ffff00ff" }
        };

        public static string Colored(this string str, string colorName) {
            var values = Enum.GetValues(typeof(ColorType));

            string colorNameTrimmed = colorName.ToLower().Trim();
            foreach (var type in values) {
                if (type.ToString() == colorNameTrimmed) {
                    return str.Colored((ColorType)type);
                }
            }

            return str.Colored(ColorType.white);
        }

        public static string Colored(this string str, ColorType type) {
            return string.Format("<color={0}>{1}</color>", ColorType2HexString(type), str);
        }

        public static string Sized(this string str, int size) {
            return string.Format("<size={0}>{1}</size>", size, str);
        }

        public static string Bolded(this string str) {
            return string.Format("<b>{0}</b>", str);
        }

        public static string Italized(this string str) {
            return string.Format("<i>{0}</i>", str);
        }


        public static string ColorType2HexString(ColorType type) {
            if (s_ColorTypeMap.ContainsKey(type)) {
                return s_ColorTypeMap[type];
            }
            return "#ffffffff";
        }




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

        public static void SetListener(this Toggle toggle, UnityAction<bool> action) {
            if(toggle == null) { return; }
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener(action);
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

        public static T RandomElement<T>(this List<T> list) {
            return list[UnityEngine.Random.Range(0, list.Count - 1)];
        }

        public static bool IsValid(this string str) {
            return !string.IsNullOrEmpty(str);
        }

        public static string GetPointerObjectName(this PointerEventData eventData ) {
            if(eventData.pointerCurrentRaycast.gameObject != null ) {
                return eventData.pointerCurrentRaycast.gameObject.name;
            }
            return string.Empty;
        }
    }

    public enum ColorType {
        aqua,
        black,
        blue,
        brown,
        darkblue,
        fuchsia,
        green,
        grey,
        lightblue,
        lime,
        magenta,
        maroon,
        navy,
        olive,
        orange,
        purple,
        red,
        silver,
        teal,
        white,
        yellow
    }
}
