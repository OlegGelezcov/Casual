using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill {
    public abstract class RavenhillEventArgs : EventArgs<GameEventName> {

        public RavenhillEventArgs(GameEventName name)
            : base(name) { }
    }

    
}
