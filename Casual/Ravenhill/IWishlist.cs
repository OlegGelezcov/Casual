using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill {
    public interface IWishlist {
        bool Add(InventoryItemData data);
        bool Remove(InventoryItemData data);
        bool IsContains(InventoryItemData data);
        bool IsFull { get; }

    }
}
