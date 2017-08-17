using System;
using System.Collections.Generic;

namespace Casual.Ravenhill.Data {
    public class CollectionData : InventoryItemData {
        public List<string> collectableIds { get; } = new List<string>();
        public List<CollectionChargeData> chargers { get; } = new List<CollectionChargeData>();
        public List<DropItem> rewards { get; } = new List<DropItem>();

        public override void Load(UXMLElement element) {
            base.Load(element);
            collectableIds.Clear();
            chargers.Clear();
            rewards.Clear();

            foreach (string collectableId in element.GetString("collectables").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
                collectableIds.Add(collectableId);
            }

            element.Element("chargers").Elements("charger").ForEach(chargerElement => {
                CollectionChargeData collectionChargeData = new CollectionChargeData();
                collectionChargeData.Load(chargerElement);
                chargers.Add(collectionChargeData);
            });

            element.Element("rewards").Elements("reward").ForEach(rewardElement => {
                DropItem dropItem = new DropItem(rewardElement);
                rewards.Add(dropItem);
            });
        }

        public override InventoryItemType type => InventoryItemType.Collection;
    }

    public class CollectionChargeData {
        public string id { get; private set; }
        public int count { get; private set; }
      
        public void Load(UXMLElement element) {
            id = element.GetString("id");
            count = element.GetInt("count");
        }
    }
}
