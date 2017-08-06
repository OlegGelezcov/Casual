using System.Collections.Generic;
using UnityEngine;

namespace Casual {
    public class ResourceObjectCache<K, V> where V : UnityEngine.Object {

        private Dictionary<K, V> cachedObjects { get; } = new Dictionary<K, V>();

        public V GetObject(K key, string path = "") {
            if(cachedObjects.ContainsKey(key)) {
                return cachedObjects[key];
            } else {
                if(!string.IsNullOrEmpty(path)) {
                    V obj = Resources.Load<V>(path);
                    if(obj != null ) {
                        cachedObjects.Add(key, obj);
                    }
                    return obj;
                }
                return default(V);
            }
        }

        public V Load(K key, string path, bool replace = false) {
            if(!cachedObjects.ContainsKey(key)) {
                V obj = Resources.Load<V>(path);
                if (obj != null) {
                    cachedObjects.Add(key, obj);
                }
                return obj;
            } else {
                if(replace ) {
                    V obj = Resources.Load<V>(path);
                    cachedObjects[key] = obj;
                    return obj;
                } else {
                    return cachedObjects[key];
                }
            }
        }

        public Dictionary<K, V> Load(Dictionary<K, string> paths, bool replace = false) {
            Dictionary<K, V> resultDictionary = new Dictionary<K, V>();
            foreach(var pathPair in paths) {
                V obj = Load(pathPair.Key, pathPair.Value, replace);
                if(obj != null ) {
                    resultDictionary.Add(pathPair.Key, obj);
                }
            }
            return resultDictionary;
        }
    }
}
