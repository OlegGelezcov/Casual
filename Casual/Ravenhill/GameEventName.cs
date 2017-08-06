using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.Ravenhill {
    public enum GameEventName : int {
        none = 0,
        touch = 1,
        view_added = 2,
        view_removed = 3,
        resource_loaded = 4
    }

    public class TouchEventArgs : RavenhillEventArgs {

        public Vector2 position { get; }

        public TouchEventArgs(Vector2 position) 
            : base(GameEventName.touch) {
            this.position = position;
        }
    }

    public class RavenhillViewAddedArgs : RavenhillEventArgs {

        public RavenhillViewType viewType { get; }

        public RavenhillViewAddedArgs(RavenhillViewType viewType)
            : base(GameEventName.view_added) {
            this.viewType = viewType;
        }
    }

    public class RavenhillViewRemovedEventArgs : RavenhillEventArgs {
        public RavenhillViewType viewType { get; }

        public RavenhillViewRemovedEventArgs(RavenhillViewType viewType)
            : base(GameEventName.view_removed) {
            this.viewType = viewType;
        }
    }

    public class RavenhillResourceLoadedEventArgs : RavenhillEventArgs {
        public RavenhillResourceLoadedEventArgs() : base(GameEventName.resource_loaded) { }
    }
}
