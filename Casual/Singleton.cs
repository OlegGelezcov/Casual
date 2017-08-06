using UnityEngine;

namespace Casual {
    public class Singleton<T> : MonoBehaviour where T:MonoBehaviour {

        private static T s_Instance = default(T);

        public static T instance {
            get {
                if (!s_Instance) {
                    s_Instance = GameObject.FindObjectOfType<T>();
                }
                if(!s_Instance) {
                    GameObject newGameObject = new GameObject(typeof(T).Name, typeof(T));
                    s_Instance = newGameObject.GetComponent<T>();
                }
                return s_Instance;
            }
        }
    }
}
