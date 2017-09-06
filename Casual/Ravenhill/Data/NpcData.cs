using System.Collections.Generic;

namespace Casual.Ravenhill.Data {
    public class NpcData : IconData {

        public NpcType type { get; private set; }
        public float prob { get; private set; }
        public List<DropItem> rewards { get; } = new List<DropItem>();
        public string buffId { get; private set; } = string.Empty;
        public string weaponId { get; private set; } = string.Empty;

        public override void Load(UXMLElement element) {
            base.Load(element);
            type = element.GetEnum<NpcType>("type");
            prob = element.GetFloat("prob");
            rewards.Clear();
            element.Element("rewards").Elements("reward").ForEach(rewardEleent => {
                rewards.Add(new DropItem(rewardEleent));
            });
            if(element.HasAttribute("buff")) {
                buffId = element.GetString("buff");
            }
            if(element.HasAttribute("weapon")) {
                weaponId = element.GetString("weapon");
            }
        }
    }
}
