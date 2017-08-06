using Casual.Ravenhill.Data;
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
        resource_loaded = 4,
        search_object_collected = 5,
        search_started = 6,
        search_progress_changed = 7,
        search_object_activated = 8
    }

    public class SearchObjectActivatedEventArgs : RavenhillEventArgs {

        public SearchObjectData searchObjectData { get; private set; }
        public BaseSearchableObject searchableObject { get; private set; }

        public SearchObjectActivatedEventArgs(SearchObjectData data, BaseSearchableObject searchableObject )
            : base(GameEventName.search_object_activated) {
            this.searchObjectData = data;
            this.searchableObject = searchableObject;
        }
    }

    public class SearchProgressChangedEventArgs : RavenhillEventArgs {
        public int foundedCount { get; private set; }
        public int totalCount { get; private set; }

        public SearchProgressChangedEventArgs(int foundedCount, int totalCount)
            : base(GameEventName.search_progress_changed) {
            this.foundedCount = foundedCount;
            this.totalCount = totalCount;
        }
    }

    public class SearchObjectCollectedEventArgs : RavenhillEventArgs {

        public SearchObjectData searchObjectData { get; private set; }
        public ISearchableObject targetSearchableObject { get; private set; }

        public SearchObjectCollectedEventArgs(SearchObjectData searchObjectData, ISearchableObject targetSearchableObject)
            : base(GameEventName.search_object_collected) {
            this.searchObjectData = searchObjectData;
            this.targetSearchableObject = targetSearchableObject;
        }
    }

    public class SearchStartedEventArgs : RavenhillEventArgs {
        public SearchStartedEventArgs() : base(GameEventName.search_started) { }
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
