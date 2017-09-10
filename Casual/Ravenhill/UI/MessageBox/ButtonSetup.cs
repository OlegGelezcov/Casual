using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {

    [Serializable]
    public class ButtonSetup {
        public ActionButton actionButton;
        public ControlColor color;
    }

    [Serializable]
    public class ImageSetup {
        public Image image;
        public ControlColor color;
    }
}
