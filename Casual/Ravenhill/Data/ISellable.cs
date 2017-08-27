using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Data {
    public interface ISellable {
        bool IsSellable { get; }
        PriceData price { get; }
    }
}
