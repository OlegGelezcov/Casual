using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casual.Ravenhill.Data;

namespace Casual.Ravenhill {
    public class EditorStoreProvider : IStoreProvider {

        public void StartPurchase(IPurchaseService service, BankProductData data) {
            service.OnPurchaseEnded(data);
        }
    }
}
