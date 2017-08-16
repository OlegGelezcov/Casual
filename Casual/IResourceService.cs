using Casual.Ravenhill.Data;
using UnityEngine;

namespace Casual {
    public interface IResourceService : IService {
        void Load();
        bool isLoaded { get;  }
        string GetString(string key);
        GameObject GetCachedPrefab(string key, string path = "");
        Sprite transparent { get; }
        Sprite GetSprite(IconData data);
    }
}
