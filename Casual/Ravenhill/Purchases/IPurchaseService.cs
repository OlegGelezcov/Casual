using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill {
    public interface IPurchaseService : IService {
        void PurchaseProduct(BankProductData product);
        void OnPurchaseEnded(BankProductData product);
    }
}
