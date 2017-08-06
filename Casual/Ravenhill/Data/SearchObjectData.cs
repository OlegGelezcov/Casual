using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Data {
    public class SearchObjectData {
        public string id { get; private set; }
        public string textId { get; private set; }

        public void Load(UXMLElement element) {
            id = element.GetString("id");
            textId = element.GetString("text_id");
        }

        public SearchObjectData() { }

        public SearchObjectData(UXMLElement element) {
            Load(element);
        }
    }
}
