using Casual.Ravenhill.Data;
using System.Linq;
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

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.StoryCollectionCharged += OnStoryCollectionCharged;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.StoryCollectionCharged -= OnStoryCollectionCharged;
        }

        private void OnStoryCollectionCharged(StoryCollectionData data) {
            Setup();
        }
    }

    public partial class FamehallView : RavenhillCloseableView {

        [SerializeField]
        private StoryCollectionView[] storyCollectionViews;
    }
}
