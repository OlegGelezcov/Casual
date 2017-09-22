using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Net {
    public interface IGift {
        ISender GetReceiver();
        ISender GetSender();
        InventoryItemData GetItemData();
    }
}
