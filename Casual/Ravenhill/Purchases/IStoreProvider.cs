using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill{
    public interface IStoreProvider {
        void StartPurchase(IPurchaseService service, BankProductData data);
    }
}
