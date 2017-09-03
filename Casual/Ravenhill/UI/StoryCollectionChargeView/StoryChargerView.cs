using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class StoryChargerView : RavenhillUIBehaviour  {

        public void Setup(StoryChargerData data ) {
            int playerCount = playerService.GetItemCount(data);
            iconImage.color = (playerCount > 0) ? Color.white : emptyColor;
            iconImage.overrideSprite = resourceService.GetSprite(data);
            nameText.text = resourceService.GetString(data.nameId);
        }
    }

    public partial class StoryChargerView : RavenhillUIBehaviour {

        [SerializeField]
        private Color emptyColor = new Color(1, 1, 1, 0.5f);

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private Text nameText;
    }
}
