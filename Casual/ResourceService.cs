using Casual.Ravenhill.Data;
using UnityEngine;

namespace Casual {
    public abstract class ResourceService : GameElement, IResourceService {

        public abstract bool isLoaded { get; }

        public abstract void Load();

        public abstract void Setup(object data);

        public abstract string GetString(string key);

        public abstract GameObject GetCachedPrefab(string key, string path = "");

        public abstract Sprite transparent { get; }

        public abstract Sprite GetSprite(IconData data);

    }
}
