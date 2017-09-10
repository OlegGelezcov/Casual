namespace Casual.Ravenhill.UI {
    using Casual.Ravenhill.Data;
    using UnityEngine;
    using UnityEngine.UI;

    public partial class NoteCollectionView : RavenhillUIBehaviour {
        private CollectionData data;

        public void Setup(CollectionData data) {
            this.data = data;

            iconImage.overrideSprite = resourceService.GetSprite(data);
            int playerCount = playerService.GetItemCount(data);
            iconImage.color = (playerCount > 0) ? Color.white : new Color(1, 1, 1, 0.5f);
        }

        public CollectionData Data => data;
    }

    public partial class NoteCollectionView : RavenhillUIBehaviour {
        [SerializeField]
        private Image iconImage;
    }
}
