using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Data {
    public class QuestData : IconData {
        public QuestType type { get; private set; }
        public string ownerId { get; private set; }
        public string startTextId { get; private set; }
        public string hintTextId { get; private set; }
        public string endTextId { get; private set; }
        public string journalId { get; private set; }

        public List<Condition> startConditions { get; private set; }
        public List<Condition> completeConditions { get; private set; }
        public List<DropItem> rewards { get; private set; }

        public DropItem expReward {
            get {
                return rewards.FirstOrDefault(reward => reward.type == DropType.exp);
            }
        }

        public DropItem silverReward {
            get {
                return rewards.FirstOrDefault(reward => reward.type == DropType.silver);
            }
        }

        public int GetSearchCounterConditionValue() {
            foreach(var completeCondition in completeConditions ) {
                if(completeCondition.type == ConditionType.search_counter_ge ) {
                    SearchCounterGeCondition scCond = completeCondition as SearchCounterGeCondition;
                    return scCond.count;
                }
            }
            return 1000;
        }

        public T GetCompleteCondition<T>() where T : Condition {
            foreach(var condition in completeConditions ) {
                if(condition is T ) {
                    return condition as T;
                }
            }
            return default(T);
        }
 
        public override void Load(UXMLElement element) {
            base.Load(element);
            type = element.GetEnum<QuestType>("type");
            ownerId = element.GetString("owner");
            startTextId = element.GetString("start_text");
            hintTextId = element.GetString("hint_text");
            endTextId = element.GetString("end_text");
            journalId = element.GetString("journal");

            startConditions = new List<Condition>();
            UXMLElement scParentElement = element.Element("start_conditions");
            if(scParentElement != null ) {
                scParentElement.Elements("condition").ForEach(e => {
                    startConditions.Add(Condition.FromXml(e));
                });
            }

            completeConditions = new List<Condition>();
            UXMLElement ccParentElement = element.Element("complete_conditions");
            if(ccParentElement != null ) {
                ccParentElement.Elements("condition").ForEach(e => {
                    completeConditions.Add(Condition.FromXml(e));
                });
            }

            rewards = new List<DropItem>();
            UXMLElement rewardsParentElement = element.Element("rewards");
            rewardsParentElement.Elements("reward").ForEach(e => {
                rewards.Add(new DropItem(e));
            });
        }
    }
}
