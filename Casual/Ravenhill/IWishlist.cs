using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill {
    public interface IWishlist {
        bool Add(CollectableData data);
        bool Remove(CollectableData data);
        bool IsContains(CollectableData data);
        bool IsFull { get; }
    }
}
