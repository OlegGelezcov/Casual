using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Net {
    public class GiftsRequest : BaseRequest {

        public GiftsRequest(INetService netService, 
            string url, 
            INetErrorFactory errorFactory):
            base(netService, url, errorFactory) { }


        private void GetGifts(ISender receiver, Action<Dictionary<string, NetGift>> onSuccess, Action<INetError> onError) {

        }
    }
}
