namespace Casual.Ravenhill.Data {
    public class IngredientData : InventoryItemData {
        public PriceData price { get; private set; }
        public float prob { get; private set; }
        public string room { get; private set; }

        public override void Load(UXMLElement element) {
            base.Load(element);
            price = new PriceData(element);
            prob = element.GetFloat("prob");
            room = element.GetString("drop_rooms");
        }

        public override InventoryItemType type => InventoryItemType.Ingredient;
    }
}
