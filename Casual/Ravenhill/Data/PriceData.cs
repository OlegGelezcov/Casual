using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Data {

    public class PriceData {
        private int silver { get; set; }
        private int gold { get; set; }

        public PriceData() {
            silver = 0;
            gold = 0;
        }

        public PriceData(int silver, int gold) {
            this.silver = silver;
            this.gold = gold;
        }

        public PriceData(UXMLElement element) {
            Load(element);
        }

        public void Load(UXMLElement element) {
            silver = element.GetInt("silver", 0);
            gold = element.GetInt("gold", 0);
        }

        public int price {
            get {
                if(silver != 0 ) {
                    return silver;
                }
                return gold;
            }
        }

        public MoneyType type {
            get {
                if(silver != 0) {
                    return MoneyType.silver;
                }
                return MoneyType.gold;
            }
        }
    }

    public enum MoneyType {
        silver,
        gold
    }
}
