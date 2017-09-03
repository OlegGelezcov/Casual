namespace Casual.Ravenhill.Data {
    public class StoreItemData {
        public string id { get; private set; }
        public string itemId { get; private set; }
        public InventoryItemType itemType { get; private set; }
        public int count { get; private set; }
        public PriceData price { get; private set; }

        public void Load(UXMLElement element ) {
            id = element.GetString("id");
            itemId = element.GetString("item_id");
            itemType = element.GetEnum<InventoryItemType>("item_type");
            count = element.GetInt("count");
            price = new PriceData();
            price.Load(element);
        }
    }
}
