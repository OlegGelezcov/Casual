using System.Collections.Generic;
using System.Linq;

namespace Casual.Ravenhill.Data {
    public class AchievmentData : IconData {

        public string rankDescriptionId { get; private set; }
        public List<AchievmentTierData> tiers { get; private set; }

        public override void Load(UXMLElement element) {
            base.Load(element);
            rankDescriptionId = element.GetString("rank_description");
            tiers = new List<AchievmentTierData>();
            element.Elements("rank").ForEach(rankElement => {
                AchievmentTierData tierData = new AchievmentTierData();
                tierData.Load(rankElement);
                tiers.Add(tierData);
            });
        }

        public AchievmentTierData GetTier(int tier) {
            return tiers.Where(t => t.tier == tier).FirstOrDefault();
        }

        public int TotalCount {
            get {
                return tiers.Sum(t => t.count);
            }
        }
    }

    public class AchievmentTierData {
        public int tier { get; private set; }
        public int count { get; private set; }
        public DropItem rewardItem { get; private set; }

        public void Load(UXMLElement element) {
            tier = element.GetInt("index");
            count = element.GetInt("count");
            rewardItem = new DropItem(element.Element("rewards").Element("reward"));
        }
    }
}
