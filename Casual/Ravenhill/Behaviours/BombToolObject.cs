namespace Casual.Ravenhill {
    using UnityEngine;

    public class BombToolObject : RavenhillGameBehaviour  {

        public void SetTarget(BaseSearchableObject searchableObject ) {
            StartCoroutine(CorProcess(searchableObject));
        }

        private System.Collections.IEnumerator CorProcess(BaseSearchableObject searchableObject) {
            yield return new WaitForSeconds(2.5f);
            if (searchableObject && searchableObject.isActive && !searchableObject.isCollected) {
                var searchManager = FindObjectOfType<SearchManager>();
                searchManager?.CollectObject(searchableObject);
            }
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }
    }
}
