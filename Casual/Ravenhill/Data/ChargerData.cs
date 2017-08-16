namespace Casual.Ravenhill.Data {
    public class ChargerData : InventoryItemData {

        public PriceData price { get; private set; }
        public float prob { get; private set; }

        public override void Load(UXMLElement element) {
            base.Load(element);
            price = new PriceData(element);
            prob = element.GetFloat("prob");
        }

        public override InventoryItemType type => InventoryItemType.Charger;
    }
}
