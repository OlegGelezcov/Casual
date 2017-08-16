using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Data {

    public class BonusData : InventoryItemData {
        public PriceData price { get; private set; }
        public int interval { get; private set; }
        public Dictionary<string, int> ingredients { get; private set; }

        public override void Load(UXMLElement element) {
            base.Load(element);
            price = new PriceData(element);
            ingredients = new Dictionary<string, int>();

            element.Element("formula").Elements("ingredient").ForEach(ingredientElement => {
                string ingredientId = ingredientElement.GetString("id");
                int count = ingredientElement.GetInt("count");
                ingredients[ingredientId] = count;
            });
        }

        public override InventoryItemType type => InventoryItemType.Bonus;
    }
}
