using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill {
    public abstract class RavenhillBaseListenerBehaviour : GameBehaviour, IEventListener<GameEventName> {
        public abstract string listenerName { get; }

        public abstract void OnEvent(EventArgs<GameEventName> args);
    }
}
