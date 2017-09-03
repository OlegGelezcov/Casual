using Casual.Ravenhill.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class StoryCollectableView : RavenhillUIBehaviour {

        public void Setup(StoryCollectableData data) {
            int playerCount = playerService.GetItemCount(data);

            iconImage.color = (playerCount > 0) ? Color.white : emptyColor;
            iconImage.overrideSprite = resourceService.GetSprite(data);
            nameText.text = resourceService.GetString(data.nameId);
        }
    }

    public partial class StoryCollectableView : RavenhillUIBehaviour {

        [SerializeField]
        private Color emptyColor = new Color(1, 1, 1, 0.5f);

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private Text nameText;

    }
}
