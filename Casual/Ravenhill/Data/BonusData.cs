using System.Collections.Generic;
using System.Linq;

namespace Casual.Ravenhill.Data {

    public class BonusData : InventoryItemData {
        public override PriceData price { get; protected set; }
        public int interval { get; private set; }
        public float value { get; private set; }
        public Dictionary<string, int> ingredients { get; private set; }

        public override void Load(UXMLElement element) {
            base.Load(element);
            price = new PriceData(element);
            ingredients = new Dictionary<string, int>();
            value = element.GetFloat("value");

            element.Element("formula").Elements("ingredient").ForEach(ingredientElement => {
                string ingredientId = ingredientElement.GetString("id");
                int count = ingredientElement.GetInt("count");
                ingredients[ingredientId] = count;
            });
        }

        public override InventoryItemType type => InventoryItemType.Bonus;
        public override bool isUsableFromInventory => true;
        public override bool IsSellable => true;

        public int GetIngredientCount(string ingredientId ) {
            return ingredients.GetOrDefault(ingredientId);
        }

        public List<string> ingredientList {
            get {
                return new List<string>(ingredients.Keys).OrderBy(i => i).ToList();
            }
        }
    }
}
