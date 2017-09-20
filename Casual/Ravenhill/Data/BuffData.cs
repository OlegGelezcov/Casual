using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Data {
    public class BuffData : IconData {
        public float Value { get; private set; }

        public override void Load(UXMLElement element) {
            base.Load(element);
            Value = element.GetFloat("value");
        }
    }
}
