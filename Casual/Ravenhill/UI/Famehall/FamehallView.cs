using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Casual.Ravenhill.UI {
    public partial class FamehallView : RavenhillCloseableView {

        public override RavenhillViewType viewType => RavenhillViewType.famehall;

        public override bool isModal => true;

        public override int siblingIndex => 2;

        public override void Setup(object data = null) {
            base.Setup(data);
            storyCollectionViews.ToList().ForEach(view => view.Setup());
        }
    }

    public partial class FamehallView : RavenhillCloseableView {

        [SerializeField]
        private StoryCollectionView[] storyCollectionViews;
    }
}
