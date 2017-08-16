using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Data {
    public class AvatarData : IconData  {

        public string giftIconPath { get; private set; }


        public override void Load(UXMLElement element) {
            base.Load(element);
            giftIconPath = element.GetString("gift_icon");
        }
    }
}
