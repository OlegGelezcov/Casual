using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casual.Ravenhill.Data;

namespace Casual.Ravenhill {
    public abstract class BaseSearchableObject : RavenhillBaseListenerBehaviour, ISearchableObject {

        public abstract string id { get; }

        public abstract bool isActive { get;  set; }

        public abstract bool isCollected { get; set; }

        public abstract void Activate(SearchObjectData data);

        public abstract void Collect();
    }
}
