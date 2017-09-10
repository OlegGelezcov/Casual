namespace Casual.Ravenhill.UI {
    using Casual.Ravenhill.Data;
    using UnityEngine;
    using UnityEngine.UI;

    public partial class NoteCollectableView : RavenhillUIBehaviour {

        private CollectableData data;

        public void Setup(CollectableData data) {
            this.data = data;

            iconImage.overrideSprite = resourceService.GetSprite(data);
            int playerCount = playerService.GetItemCount(data);
            iconImage.color = (playerCount > 0) ? Color.white : new Color(1, 1, 1, 0.5f);
        }

        public CollectableData Data => data;
    }

    public partial class NoteCollectableView : RavenhillUIBehaviour {

        [SerializeField]
        private Image iconImage;
    }
}
