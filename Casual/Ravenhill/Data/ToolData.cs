namespace Casual.Ravenhill.Data {
    public class ToolData : InventoryItemData {

        public PriceData price { get; private set; }

        public override void Load(UXMLElement element) {
            base.Load(element);
            price = new PriceData(element);
        }

        public override InventoryItemType type => InventoryItemType.Tool;
    }
}
