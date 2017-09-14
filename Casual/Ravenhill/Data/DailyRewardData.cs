using System.Collections.Generic;

namespace Casual.Ravenhill.Data {
    public class DailyRewardData : RavenhillGameElement {
        public int day { get; private set; }
        public List<DropItem> rewards { get; private set; }

        public void Load(UXMLElement element) {
            day = element.GetInt("day");
            rewards = new List<DropItem>();
            element.Element("rewards").Elements("reward").ForEach(rewardElement => {
                DropItem dropItem = new DropItem(rewardElement);
                rewards.Add(dropItem);
            });

        }
    }
}
