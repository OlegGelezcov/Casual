using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Data {
    public class StoryCollectableData : InventoryItemData {

        public override InventoryItemType type => InventoryItemType.StoryCollectable;

        public override bool isUsableFromInventory => false;

        public override bool IsSellable => false;

        public override PriceData price { get => PriceData.None; protected set { } }

        public string collectionId { get; private set; }

        public override void Load(UXMLElement element) {
            base.Load(element);
            collectionId = element.GetString("collection");
        }
    }
}
