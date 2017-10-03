using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill {
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public static class RavenhillExtensions {

        public static void SetListener(this Button button, UnityAction action) {
            button.SetListener(action, CasualEngine.Get<RavenhillEngine>().GetService<IAudioService>());
        }
    }
}
