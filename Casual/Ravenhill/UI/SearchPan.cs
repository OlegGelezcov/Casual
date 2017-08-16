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

        private SearchTimerView searchTimerView => m_SearchTimerView;


        public override string listenerName => "search_pan";

        public override RavenhillViewType viewType => RavenhillViewType.search_pan;

        public override bool isModal => false;

        public override int siblingIndex => -10;

        private SearchTextSlot[] slots => m_Slots;

        public override void OnEnable() {
            base.OnEnable();
            AddHandler(GameEventName.search_object_collected, OnSearchObjectCollected);
            AddHandler(GameEventName.search_progress_changed, OnSearchProgressChanged);
            AddHandler(GameEventName.search_object_activated, OnSearchTextActivated);
            AddHandler(GameEventName.search_text_stroked, OnSearchTextStroked);

        }

        public override void OnDisable() {
            base.OnDisable();
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
            searchTimerView?.StartTimer(session.roomInfo.roomSetting.searchTime);
        }

        private SearchTextSlot FindEmptySlot() {
            return slots.FirstOrDefault(slot => slot.isEmpty);
        }

        private SearchTextSlot FindSlot(SearchObjectData searchObjectData ) {
            return slots.FirstOrDefault(slot => slot.searchObjectDataId == searchObjectData.id);
        }

        private void OnSearchProgressChanged(EventArgs<GameEventName> inargs) {

        }

        private void OnSearchTextStroked(EventArgs<GameEventName> inargs ) {
            SearchTextStrokedEventArgs args = inargs as SearchTextStrokedEventArgs;
            if (args != null ) {
                var slot = FindSlot(args.searchObjectData);
                slot?.DestroyText();
            }
        }

        private void OnSearchObjectCollected(EventArgs<GameEventName> inargs ) {
            SearchObjectCollectedEventArgs args = inargs as SearchObjectCollectedEventArgs;
            if (args != null ) {
                var slot = FindSlot(args.searchObjectData);
                slot?.Stroke();
            }
        }

        private void OnSearchTextActivated(EventArgs<GameEventName> inargs ) {
            SearchObjectActivatedEventArgs args = inargs as SearchObjectActivatedEventArgs;
            if (args != null ) {
                StartCoroutine(CorCreateSearchTextSlotText(args.searchObjectData));
            }
        }

        private System.Collections.IEnumerator CorCreateSearchTextSlotText(SearchObjectData searchObjectData ) {
            yield return new WaitUntil(() => FindEmptySlot() != null);
            var slot = FindEmptySlot();
            slot?.CreateText(searchObjectData);
        }
    }
}
