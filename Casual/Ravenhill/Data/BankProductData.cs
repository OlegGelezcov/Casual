using System.Collections.Generic;

namespace Casual.Ravenhill.Data {
    public class BankProductData {
        public string id { get; private set; }
        public string nameId { get; private set; }
        public string iosStoreId { get; private set; }
        public PriceData price { get; private set; }
        public int bonus { get; private set; }
        public string realPrice { get; private set; }
        public bool isBest { get; private set; }
        public bool isPopular { get; private set; }
        public bool isBank { get; private set; }
        public DiscountData discountData { get; private set; }
        public List<DropItem> rewards { get; private set; }

        public void Load(UXMLElement element) {
            id = element.GetString("id");
            nameId = element.GetString("name");
            iosStoreId = element.GetString("ios_store_id");
            price = new PriceData(element);
            bonus = element.GetInt("bonus");
            realPrice = element.GetString("real_price");
            isBest = element.GetBool("is_best");
            isPopular = element.GetBool("is_popular");
            isBank = element.GetBool("is_bank");

            discountData = new DiscountData();
            var discountElement = element.Element("discount_info");
            if(discountElement != null ) {
                discountData.Load(discountElement);
            }

            rewards = new List<DropItem>();
            var rewardsElement = element.Element("rewards");
            if (rewardsElement != null) {
                foreach (UXMLElement rewardElement in rewardsElement.Elements("reward")) {
                    DropItem dropItem = new DropItem(rewardElement);
                    rewards.Add(dropItem);
                }
            }
        }
    }

    public class DiscountData {
        public string id { get; private set; }
        public string oldPrice { get; private set; }
        public string oldIosStoreId { get; private set; }
        public string iconPath { get; private set; }

        public void Load(UXMLElement element ) {
            if(element != null ) {
                id = element.GetString("id");
                oldPrice = element.GetString("old_price");
                oldIosStoreId = element.GetString("old_ios_store_id");
                iconPath = element.GetString("icon");
            }
        }
    }
}
