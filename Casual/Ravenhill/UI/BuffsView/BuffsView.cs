namespace Casual.Ravenhill.UI {
    using Casual.UI;
    using UnityEngine;

    public partial class BuffsView : RavenhillBaseView {

        public override RavenhillViewType viewType => RavenhillViewType.buffs_view;

        public override bool isModal => false;

        public override int siblingIndex => -10;

        public override void Setup(object objdata = null) {
            base.Setup(objdata);

            listView.Clear();
            BuffListView.ListViewData data = new ListView<BuffInfo>.ListViewData {
                dataList = playerService.BuffList
            };
            listView.Setup(data);
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.BuffAdded += OnBuffAdded;
            RavenhillEvents.BuffRemoved += OnBuffRemoved;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.BuffAdded -= OnBuffAdded;
            RavenhillEvents.BuffRemoved -= OnBuffRemoved;
        }

        private void OnBuffAdded(BuffInfo info ) {
            Setup();
        }

        private void OnBuffRemoved(BuffInfo info ) {
            Setup();
        }
    }

    public partial class BuffsView : RavenhillBaseView {

        [SerializeField]
        private BuffListView listView;

    }
}
