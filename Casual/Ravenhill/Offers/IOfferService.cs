using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill {
    public interface IOfferService : IService {

        bool IsBankOfferActive { get; }
    }
}
