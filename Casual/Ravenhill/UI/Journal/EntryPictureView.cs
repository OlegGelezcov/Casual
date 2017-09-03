using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class EntryPictureView : RavenhillUIBehaviour {

        public void Setup(JournalEntryInfo info) {

            if(info.type == Data.JournalEntryType.story ) {
                storyParent.ActivateSelf();
                storyImage.overrideSprite = resourceService.GetSprite(info.Data);
                sideParent.DeactivateSelf();
            } else if(info.type == Data.JournalEntryType.side ){
                storyParent.DeactivateSelf();
                sideParent.ActivateSelf();
                sideImage.overrideSprite = resourceService.GetSprite(info.Data);
            } else {
                storyParent.DeactivateSelf();
                sideParent.DeactivateSelf();
            }
        }

        public void Clear() {
            storyParent.DeactivateSelf();
            sideParent.DeactivateSelf();
        }
    }

    public partial class EntryPictureView : RavenhillUIBehaviour {

        [SerializeField]
        private GameObject storyParent;

        [SerializeField]
        private Image storyImage;

        [SerializeField]
        private GameObject sideParent;

        [SerializeField]
        private Image sideImage;
    }
}
