using UnityEngine;

namespace Casual.Ravenhill.Data {
    public class DropItem : RavenhillGameElement {

        public InventoryItemData itemData { get; private set; }
        public int count { get; private set; }
        public DropType type { get; private set; }

        public Color color { get; set; } = Color.white;

        public DropItem(DropType type, int count, InventoryItemData itemData = null) {
            this.type = type;
            this.count = count;
            this.itemData = itemData;
        }

        public DropItem(UXMLElement element ) {
            this.type = element.GetEnum<DropType>("type");
            this.count = element.GetInt("count");

            if(this.type == DropType.item ) {
                string itemId = element.GetString("item_id");
                InventoryItemType itemType = element.GetEnum<InventoryItemType>("item_type");
                this.itemData = resourceService.GetInventoryItemData(itemType, itemId);
            }
        }
    }
}
