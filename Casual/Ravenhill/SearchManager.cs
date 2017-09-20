using Casual.Ravenhill.Data;
using Casual.Ravenhill.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.Ravenhill {
    public class SearchManager : RavenhillGameBehaviour {

#pragma warning disable 0649
        [SerializeField]
        private int m_MaxActiveObjects = 6;

        [SerializeField]
        private SearchGroup[] m_SearchGroups;
#pragma warning restore 0649

        public int maxActiveObjects => m_MaxActiveObjects;

        private SearchGroup[] searchGroups => m_SearchGroups;

        private List<SearchObjectData> notFoundedObjects { get; } = new List<SearchObjectData>();
        public List<SearchObjectData> activeObjects { get; } = new List<SearchObjectData>();
        private List<SearchObjectData> foundedObjects { get; } = new List<SearchObjectData>();

        private readonly SearchMissChecker missChecker = new SearchMissChecker();

        private SearchGroup activeGroup { get; set; }
        private BaseSearchableObject[] currentSearchObjects { get; set; }
        private int numberToFind { get; set; }


        public int searchableObjectCount => currentSearchObjects?.Length ?? 0;

        public int foundedSearchObjectCount => foundedObjects.Count;

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.SearchTimerCompleted += OnSearchTimerCompleted;
            RavenhillEvents.Touch += OnTouch;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.SearchTimerCompleted -= OnSearchTimerCompleted;
            RavenhillEvents.Touch -= OnTouch;
        }

        private void OnTouch(Vector2 position ) {
            missChecker.Check(position);
        }

        private void OnSearchTimerCompleted() {
            EndSearch(status: SearchStatus.fail, showExitRoomView: true);
        }

        public override void Start() {
            base.Start();
           
            SearchSession session = ravenhillGameModeService.searchSession;
            StartSearch(session.roomInfo.currentRoomSetting.searchObjectCount);
            engine.GetService<IViewService>().ShowView(RavenhillViewType.search_pan, session);

            if(ravenhillGameModeService.searchSession.searchMode == SearchMode.Night ) {
                GameObject spotInstance = Instantiate<GameObject>(resourceService.GetCachedPrefab("spot"), new Vector3(0, 0, -30), Quaternion.identity);
            }
        }

        public void StartSearch(int maxCount) {
            RavenhillEvents.OnSearchStarted();

            int groupIndex = UnityEngine.Random.Range(0, searchGroups.Length);
            searchGroups[groupIndex].gameObject.SetActive(true);
            activeGroup = searchGroups[groupIndex];

            currentSearchObjects = activeGroup.Activate(maxCount);

            RavenhillEvents.OnSearchProgressChanged(foundedSearchObjectCount, searchableObjectCount);
            var resourceService = engine.GetService<IResourceService>() as RavenhillResourceService;
            notFoundedObjects.Clear();
            for (int i = 0; i < currentSearchObjects.Length; i++) {
                var searchObjectData = resourceService.GetSearchObjectData(currentSearchObjects[i].id);
                if (searchObjectData != null) {
                    notFoundedObjects.Add(searchObjectData);
                }
            }
            numberToFind = notFoundedObjects.Count;
            Debug.Log($"Not Founded: {notFoundedObjects.Count}, Active: {activeObjects.Count}, Founded: {foundedObjects.Count}");
        }

        //private void ActivateGroup(int maxObjectCount) {

        //    //StartCoroutine(CorActivate());
        //}

        //private bool isActivationNotStarted = true;

        //private System.Collections.IEnumerator CorActivate() {
        //    yield return new WaitUntil(() => isActivationNotStarted);
        //    StartCoroutine(ActivateIndices());
        //}

        public bool TryActivateNext(out SearchObjectData data) {
            data = null;
            if(notFoundedObjects.Count > 0 ) {
                data = notFoundedObjects[0];
                notFoundedObjects.RemoveAt(0);
                activeObjects.Add(data);
                ActivateObject(data);
                Debug.Log($"Activated {data.id}");
                return true;
            }
            return false;
        }

        //private System.Collections.IEnumerator ActivateIndices() {
        //    isActivationNotStarted = false;
        //    int numberToActivate = maxActiveObjects - activeObjects.Count;
        //    int notFoundedCount = notFoundedObjects.Count;

        //    for(int i = 0; i < Mathf.Min(numberToActivate, notFoundedCount); i++ ) {

        //        yield return new WaitUntil(() => {
        //            var sobj = viewService.GetView(RavenhillViewType.search_pan);
        //            if(sobj) {
        //                var searchPan = sobj.GetComponentInChildren<SearchPan>();
        //                if(searchPan ) {
        //                    return searchPan.HasEmptySlot;
        //                }
        //            }
        //            return false;
        //        });

        //        SearchObjectData firstData = notFoundedObjects[0];
        //        notFoundedObjects.RemoveAt(0);
        //        activeObjects.Add(firstData);
        //        ActivateObject(firstData);
        //    }
        //    isActivationNotStarted = true;
        //}

        private void ActivateObject(SearchObjectData data) {
            var targetObject = currentSearchObjects.FirstOrDefault(sObj => sObj.id == data.id);
            targetObject?.Activate(data);
            
        }

        public void CollectObject(BaseSearchableObject searchableObject) {
            var activeData = activeObjects.FirstOrDefault(obj => obj.id == searchableObject.id);

            if (activeData != null) {
                activeObjects.Remove(activeData);
                foundedObjects.Add(activeData);
                RavenhillEvents.OnSearchProgressChanged(foundedSearchObjectCount, searchableObjectCount);
                searchableObject.Collect();

                if (isWin) {
                    Debug.Log("EXIT");
                    EndSearch(status: SearchStatus.success, showExitRoomView: true);
                }
            }
        }

        private bool isWin => foundedSearchObjectCount == numberToFind;

        public void WinRoomInstantly() {
            EndSearch(SearchStatus.success, true);
        }

        public void EndSearch(SearchStatus status, bool showExitRoomView) {
            var timerView = FindObjectOfType<SearchTimerView>();
            timerView.isBreaked = true;

            int searchTime = 0;
            if(timerView ) {
                searchTime = Mathf.RoundToInt(timerView.searchTime);
            }  else {
                searchTime = ravenhillGameModeService.searchSession.roomInfo.currentRoomSetting.searchTime;
            }

            engine.Cast<RavenhillEngine>().EndSearchSession(status, searchTime);

            if(status == SearchStatus.success || showExitRoomView ) {
                StartCoroutine(CorShowExitRoomView());
            } else {
                Exit();
            }
        }

        private System.Collections.IEnumerator CorShowExitRoomView() {
            yield return new WaitForSeconds(2.0f);
            viewService.RemoveView(RavenhillViewType.search_pan);
               
            viewService.ShowView(RavenhillViewType.exit_room_view, ravenhillGameModeService.searchSession);
        }



        public virtual void Exit() {
            StartCoroutine(CorExit());
        }


        private System.Collections.IEnumerator CorExit() {
            yield return new WaitForSeconds(2.0f);
            if(viewService.ExistView(RavenhillViewType.search_pan)) {
                viewService.RemoveView(RavenhillViewType.search_pan);
            }
            engine.Cast<RavenhillEngine>().LoadPreviousScene();
        }
    }
}
