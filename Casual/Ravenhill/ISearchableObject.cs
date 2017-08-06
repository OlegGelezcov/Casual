using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill {
    public interface ISearchableObject {
        string id { get; }
        bool isActive { get; }
        bool isCollected { get; }

        void Activate(SearchObjectData data);
        void Collect();
    }
}
