using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.Ravenhill {
    public class SearchManager : RavenhillBaseListenerBehaviour {

        [SerializeField]
        private int m_MaxActiveObjects = 6;

        [SerializeField]
        private SearchGroup[] m_SearchGroups;

        public int maxActiveObjects => m_MaxActiveObjects;

        public override string listenerName => "search_manager";

        private SearchGroup[] searchGroups => m_SearchGroups;

        private List<SearchObjectData> notFoundedObjects { get; } = new List<SearchObjectData>();
        private List<SearchObjectData> activeObjects { get; } = new List<SearchObjectData>();
        private List<SearchObjectData> foundedObjects { get; } = new List<SearchObjectData>();

        private SearchGroup activeGroup { get; set; }
        private BaseSearchableObject[] currentSearchObjects { get; set; }
        private int numberToFind { get; set; }

        public override void OnEvent(EventArgs<GameEventName> args) {

        }

        public int searchableObjectCount => currentSearchObjects?.Length ?? 0;

        public int foundedSearchObjectCount => foundedObjects.Count;

        public override void Start() {
            base.Start();

        }

        public void StartSearch(int maxCount) {
            engine.GetService<IEventService>().SendEvent(new SearchStartedEventArgs());
            ActivateGroup(maxCount);
        }

        private void ActivateGroup(int maxObjectCount) {
            int groupIndex = UnityEngine.Random.Range(0, searchGroups.Length);
            searchGroups[groupIndex].gameObject.SetActive(true);
            activeGroup = searchGroups[groupIndex];
            currentSearchObjects = activeGroup.Activate(maxObjectCount);
            engine.GetService<IEventService>().SendEvent(new SearchProgressChangedEventArgs(foundedSearchObjectCount, searchableObjectCount));

            var resourceService = engine.GetService<IResourceService>() as RavenhillResourceService;
            notFoundedObjects.Clear();
            for(int i = 0; i < currentSearchObjects.Length; i++ ) {
                var searchObjectData = resourceService.GetSearchObjectData(currentSearchObjects[i].id);
                if(searchObjectData != null ) {
                    notFoundedObjects.Add(searchObjectData);
                }
            }
            numberToFind = notFoundedObjects.Count;
        }

        private void ActivateIndices() {
            int numberToActivate = maxActiveObjects - activeObjects.Count;
            int notFoundedCount = notFoundedObjects.Count;

            for(int i = 0; i < Mathf.Min(numberToActivate, notFoundedCount); i++ ) {
                SearchObjectData firstData = notFoundedObjects[0];
                notFoundedObjects.RemoveAt(0);
                activeObjects.Add(firstData);
                ActivateObject(firstData);
            }
        }

        private void ActivateObject(SearchObjectData data) {
            var targetObject = currentSearchObjects.FirstOrDefault(sObj => sObj.id == data.id);
            targetObject?.Activate(data);
        }

        public void CollectObject(string id) {
            var activeData = activeObjects.FirstOrDefault(obj => obj.id == id);
            activeObjects.Remove(activeData);
            foundedObjects.Add(activeData);
            engine.GetService<IEventService>()?.SendEvent(new SearchProgressChangedEventArgs(foundedSearchObjectCount, searchableObjectCount));
            if(isWin) {
                ExitWithSuccess();
            } else {
                ActivateIndices();
            }
        }

        private bool isWin => foundedSearchObjectCount == numberToFind;

        protected virtual void ExitWithSuccess() {

        }
    }
}
