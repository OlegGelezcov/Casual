using UnityEngine;

namespace Casual.Ravenhill {
    public class RavenhillInput : CasualInput {
        protected override void SendTouchEvent(Vector2 position) {
            engine.GetService<IEventService>()?.SendEvent(new TouchEventArgs(position));
        }
    }
}
