namespace Casual.Ravenhill.UI {
    public partial class SearchPauseView : RavenhillCloseableView {

        public override RavenhillViewType viewType => RavenhillViewType.search_pause_view;

        public override bool isModal => true;

        public override int siblingIndex => 200;

        public override void Setup(object objdata = null) {
            base.Setup(objdata);
        }
    }

    public partial class SearchPauseView : RavenhillCloseableView {
    }
}
