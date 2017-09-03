using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Data {
    public class VideoData {
        public string id { get; private set; }
        public string streamingAsset { get; private set; }
        public string resourceAsset { get; private set; }

        public void Load(UXMLElement element) {
            id = element.GetString("id");
            streamingAsset = element.GetString("streaming_asset");
            resourceAsset = element.GetString("resource_asset");
        }
    }
}
