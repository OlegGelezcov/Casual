using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Net {
    public enum NetErrorCode : int {
        unknown = 0,
        unityrequest = 1,
        json = 2,
        sender_not_founded = 3,
        receiver_not_founded = 4,
        gift_not_found = 5
    }
}
