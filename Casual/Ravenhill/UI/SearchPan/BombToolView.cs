using System.Collections.Generic;
using System.Linq;

namespace Casual.Ravenhill.UI {
    using Casual.Ravenhill.Data;
    using UnityEngine;

    public partial class BombToolView : BaseToolView {
        protected override void OnUse(InventoryItemData data) {
            base.OnUse(data);

            var searchableObjects = FindObjectsOfType<BaseSearchableObject>().Where(bso => bso.isActive && !bso.isCollected).Take(3).ToList();
            if(searchableObjects.Count > 0 ) {
                StartCoroutine(CorSpawnBombs(searchableObjects));
            }
        }

        private System.Collections.IEnumerator CorSpawnBombs(List<BaseSearchableObject> searchableObjects) {
            foreach(BaseSearchableObject so in searchableObjects) {
                so.SetCollectType(CollectType.Bomb);
                Vector3 position = new Vector3(so.transform.position.x, so.transform.position.y, -30);
                GameObject instance = Instantiate<GameObject>(effectPrefab, position, Quaternion.identity);
                instance.GetComponent<BombToolObject>().SetTarget(so);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    public partial class BombToolView : BaseToolView {

        [SerializeField]
        private GameObject effectPrefab;
    }
}
