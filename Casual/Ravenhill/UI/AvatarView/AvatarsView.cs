using System.Linq;

namespace Casual.Ravenhill.UI {

    using UnityEngine;

    public partial class AvatarsView : RavenhillCloseableView {

        public override RavenhillViewType viewType => RavenhillViewType.avatars_view;

        public override bool isModal => true;

        public override int siblingIndex => 2;

        public override void Setup(object objdata = null) {
            base.Setup(objdata);

            views.ToList().ForEach(view => view.Setup());
            var currentView = GetView(playerService.avatarId);
            if(currentView != null ) {
                currentView.Select();
            } else {
                views[0].Select();
            }
        }

        private AvatarItemView GetView(string avatarId ) {
            return views.Where(view => view.AvatarId == avatarId).FirstOrDefault();
        }
    }

    public partial class AvatarsView : RavenhillCloseableView {

        [SerializeField]
        private AvatarItemView[] views;


    }
}
