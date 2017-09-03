using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Data {

    public class StoryCollectionData : InventoryItemData {

        public override InventoryItemType type => InventoryItemType.StoryCollection;

        public override bool isUsableFromInventory => false;

        public override bool IsSellable => false;

        public override PriceData price { get => PriceData.None; protected set { } }

        public string iconGrayPath { get; private set; }
        public List<string> collectables { get; private set; }


        public string chargeBackImagePath { get; private set; }

        public string chargeBackImageId => id + "chargeback";

        public string chargeColorImagePath { get; private set; }

        public string chargeColorImageId => id + "chargecolor";

        public string chargeGrayImagePath { get; private set; }

        public string chargeGrayImageId => id + "chargegray";

        public string chargerId { get; private set; }

        public string videoId { get; private set; }

        public override void Load(UXMLElement element) {
            base.Load(element);

            iconGrayPath = element.GetString("icon_gray");
            collectables = new List<string>();
            collectables.AddRange(element.GetStringArray("collectables"));
            chargeBackImagePath = element.GetString("charge_back_image");
            chargeColorImagePath = element.GetString("charge_color_image");
            chargeGrayImagePath = element.GetString("charge_gray_image");
            chargerId = element.GetString("charger_id");
            videoId = element.GetString("video");
        }
    }
}
