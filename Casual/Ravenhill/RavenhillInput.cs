using UnityEngine;

namespace Casual.Ravenhill {
    public class RavenhillInput : CasualInput {
        protected override void SendTouchEvent(Vector2 position) {
            RavenhillEvents.OnTouch(position);
        }
    }
}
