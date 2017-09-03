using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill{
    public class OfferService : RavenhillGameBehaviour, IOfferService {

        public bool IsBankOfferActive => false;

        public void Setup(object data) {
            
        }
    }
}
