using Casual.Ravenhill.Data;
using UnityEngine;

namespace Casual.Ravenhill.UI {
    public class SearchTextSlot : RavenhillUIBehaviour {

        private SearchText searchText { get; set; }
        public bool isEmpty => (searchText == null);


        private void DestroyText() {
            if(!isEmpty) {
                Destroy(searchText.gameObject);
                searchText = null;
            }
        }

        private void CreateText(SearchObjectData searchObjectData) {
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

        public void Setup() {
            if(isEmpty ) {
                var searchManager = FindObjectOfType<SearchManager>();
                SearchObjectData data = null;
                if(searchManager.TryActivateNext(out data)) {
                    CreateText(data);
                }
            }
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.SearchTextStroked += OnSearchTextStroked;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.SearchTextStroked -= OnSearchTextStroked;
        }

        private void OnSearchTextStroked(SearchText searchText, SearchObjectData data) {
            if (!isEmpty) {
                if (this.searchText == searchText) {
                    DestroyText();
                    var searchManager = FindObjectOfType<SearchManager>();
                    SearchObjectData sdata = null;
                    if (searchManager.TryActivateNext(out sdata)) {
                        CreateText(sdata);
                    }
                }
            }
        }
    }
}
