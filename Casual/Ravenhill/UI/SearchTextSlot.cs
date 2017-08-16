using Casual.Ravenhill.Data;
using UnityEngine;

namespace Casual.Ravenhill.UI {
    public class SearchTextSlot : RavenhillGameBehaviour {

        private SearchText searchText { get; set; }
        public bool isEmpty => (searchText == null);


        public void DestroyText() {
            if(!isEmpty) {
                Destroy(searchText.gameObject);
                searchText = null;
            }
        }

        public void CreateText(SearchObjectData searchObjectData) {
            DestroyText();

            GameObject searchTextGameObject = Instantiate<GameObject>(engine.GetService<IResourceService>().GetCachedPrefab("search_text"));
            searchTextGameObject.transform.SetParent(transform, false);
            searchTextGameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            searchText = searchTextGameObject.GetComponent<SearchText>();
            searchText.Setup(searchObjectData);
        }

        public string searchObjectDataId {
            get {
                if(!isEmpty) {
                    return searchText.searchObjectData?.id;
                }
                return string.Empty;
            }
        }

        public void Stroke() {
            if(!isEmpty) {
                searchText.Stroke();
            }
        }

    }
}
