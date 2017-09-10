namespace Casual.Ravenhill.UI {
    using Casual.UI;
    using UnityEngine;

    public partial class AchievmentsView : RavenhillCloseableView {

        public override RavenhillViewType viewType => RavenhillViewType.achievments_view;

        public override bool isModal => true;

        public override int siblingIndex => 2;

        public override void Setup(object data = null) {
            base.Setup(data);

            listView.Clear();
            listView.Setup(new ListView<Data.AchievmentData>.ListViewData {
                dataList = resourceService.achievmentList
            });
        }
    }

    public partial class AchievmentsView : RavenhillCloseableView {

        [SerializeField]
        private AchievmentListView listView;


    }
}
