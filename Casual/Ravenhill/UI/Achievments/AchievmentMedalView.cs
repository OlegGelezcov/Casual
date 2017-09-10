using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI  {
    public partial class AchievmentMedalView : RavenhillUIBehaviour {

        public void Setup(AchievmentData data ) {
            IAchievmentService service = engine.GetService<IAchievmentService>();
            iconImage.overrideSprite = service.IsTierUnlocked(data, tier) ? openedSprite : closedSprite;
        }
    }

    public partial class AchievmentMedalView : RavenhillUIBehaviour {

        [SerializeField]
        private int tier = 1;

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private Sprite openedSprite;

        [SerializeField]
        private Sprite closedSprite;
    }
}
