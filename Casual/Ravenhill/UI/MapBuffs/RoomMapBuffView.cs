using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class RoomMapBuffView : RavenhillUIBehaviour {

        public void Setup(BuffData data ) {
            iconImage.overrideSprite = resourceService.GetSprite(data);
        }
    }

    public partial class RoomMapBuffView : RavenhillUIBehaviour {

        [SerializeField]
        private Image iconImage;
    }

}
