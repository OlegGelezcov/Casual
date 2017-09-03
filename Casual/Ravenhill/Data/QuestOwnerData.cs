using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Data {
    public class QuestOwnerData : IconData {

        public string iconScary { get; private set; }

        public override void Load(UXMLElement element) {
            base.Load(element);
            iconScary = element.GetString("icon_scary");
        }


    }
}
