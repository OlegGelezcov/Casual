namespace Casual.Ravenhill.Data {

    public class PriceData {
        private int silver { get; set; }
        private int gold { get; set; }

        public static PriceData FromSilver(int silver) {
            return new PriceData(silver, 0);
        }

        public static PriceData FromGold(int gold ) {
            return new PriceData(0, gold);
        }

        public PriceData() {
            silver = 0;
            gold = 0;
        }

        public static bool IsNone(PriceData price) {
            return (price.silver == 0) && (price.gold == 0);
        }

        public static PriceData None {
            get {
                return new PriceData();
            }
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
