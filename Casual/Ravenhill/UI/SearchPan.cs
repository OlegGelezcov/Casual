using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public class SearchPan : RavenhillBaseView {


#pragma warning disable 0169,0649
        [SerializeField]
        private SearchTextSlot[] m_Slots;

        [SerializeField]
        private Text m_SearchItemCountText;

        [SerializeField]
        private Button m_PauseButton;

        [SerializeField]
        private Button m_SettingsButton;

        [SerializeField]
        private Button m_TakeScreenButton;

        [SerializeField]
        private Button m_ExitSearchButton;

        [SerializeField]
        private Button m_ClewerToolButton;

        [SerializeField]
        private Button m_DinamiteToolButton;

        [SerializeField]
        private Button m_ClockToolButton;

        [SerializeField]
        private Button m_EyeToolButton;

        [SerializeField]
        private SearchTimerView m_SearchTimerView;
#pragma warning restore 0169,0649


        private SearchTimerView searchTimerView => m_SearchTimerView;

        public override RavenhillViewType viewType => RavenhillViewType.search_pan;

        public override bool isModal => false;

        public override int siblingIndex => -10;

        private SearchTextSlot[] slots => m_Slots;

        public override void OnEnable() {
            base.OnEnable();


            RavenhillEvents.SearchObjectCollected += OnSearchObjectCollected;
            RavenhillEvents.SearchProgressChanged += OnSearchProgressChanged;
            RavenhillEvents.SearchObjectActivated += OnSearchTextActivated;
            RavenhillEvents.SearchTextStroked += OnSearchTextStroked;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.SearchObjectCollected -= OnSearchObjectCollected;
            RavenhillEvents.SearchProgressChanged -= OnSearchProgressChanged;
            RavenhillEvents.SearchObjectActivated -= OnSearchTextActivated;
            RavenhillEvents.SearchTextStroked -= OnSearchTextStroked;
        }

        public override void Setup(object data = null) {
            base.Setup(data);

            var searchManager = FindObjectOfType<SearchManager>();
            var activeSearchObjects = searchManager?.activeObjects ?? new List<Data.SearchObjectData>();

            foreach(var activeObject in activeSearchObjects) {
                var slot = FindEmptySlot();
                slot?.CreateText(activeObject);
            }

            SearchSession session = data as SearchSession;
            searchTimerView?.StartTimer(session.roomInfo.currentRoomSetting.searchTime);
        }

        private SearchTextSlot FindEmptySlot() {
            return slots.FirstOrDefault(slot => slot.isEmpty);
        }

        private SearchTextSlot FindSlot(SearchObjectData searchObjectData ) {
            return slots.FirstOrDefault(slot => slot.searchObjectDataId == searchObjectData.id);
        }

        private void OnSearchProgressChanged(int oldValue, int newValue) {

        }

        private void OnSearchTextStroked(SearchText searchText, SearchObjectData data) {
            var slot = FindSlot(data);
            slot?.DestroyText();
        }

        private void OnSearchObjectCollected(SearchObjectData data, ISearchableObject searchable ) {
            var slot = FindSlot(data);
            slot?.Stroke();
        }

        private void OnSearchTextActivated(SearchObjectData data, BaseSearchableObject searchable) {
            StartCoroutine(CorCreateSearchTextSlotText(data));
        }

        private System.Collections.IEnumerator CorCreateSearchTextSlotText(SearchObjectData searchObjectData ) {
            yield return new WaitUntil(() => FindEmptySlot() != null);
            var slot = FindEmptySlot();
            slot?.CreateText(searchObjectData);
        }

        public bool HasEmptySlot => FindEmptySlot() != null;
    }
}
