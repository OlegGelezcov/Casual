using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.Ravenhill.UI {
    public partial class ColoredImageCollection : RavenhillUIBehaviour  {

        public void Setup(ControlColor color ) {
            ResetAllImages();
            foreach(var image in images ) {
                if(image.color == color ) {
                    if(image.image != null ) {
                        image.image.ActivateSelf();
                        break;
                    }
                }
            }
        }

        private void ResetAllImages() {
            foreach(var image in images ) {
                if(image.image != null ) {
                    image.image.DeactivateSelf();
                }
            }
        }
    }

    public partial class ColoredImageCollection : RavenhillUIBehaviour {

        [SerializeField]
        private ImageSetup[] images;
    }
}
