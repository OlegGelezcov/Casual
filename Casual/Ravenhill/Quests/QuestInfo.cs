using Casual.Ravenhill.Data;
using UnityEngine;

namespace Casual.Ravenhill {
    public class QuestInfo : RavenhillGameElement, ISaveElement, IQuest, IIdObject {

        public string id { get; private set; }
        public QuestState state { get; private set; } = QuestState.not_started;
        public int startTime { get; private set; } = 0;
        public int endTime { get; private set; } = 0;


        private QuestData data = null;

        public QuestData Data {
            get {
                CacheData();
                return data;
            }
        }

        private void CacheData() {
            if (data == null) {
                data = resourceService.GetQuest(id);
            }
        }

        public QuestInfo(UXMLElement element) {
            Load(element);
        }

        public QuestInfo(QuestData data) {
            this.data = data;
            this.id = data.id;
            this.state = QuestState.not_started;
        }

        public bool IsValidData => Data != null;

        public QuestType type => Data.type;

        public T GetCompleteCondition<T>() where T : Condition {
            return Data.GetCompleteCondition<T>();
        }

        #region ISaveElement
        public UXMLWriteElement GetSave() {
            UXMLWriteElement writeElement = new UXMLWriteElement("quest");
            writeElement.AddAttribute("id", id);
            writeElement.AddAttribute("state", state.ToString());
            writeElement.AddAttribute("start_time", startTime);
            writeElement.AddAttribute("end_time", endTime);
            return writeElement;
        }

        public void Load(UXMLElement element) {
            id = element.GetString("id");
            CacheData();
            state = element.GetEnum<QuestState>("state");
            startTime = element.GetInt("start_time", 0);
            endTime = element.GetInt("end_time", 0);
        }

        public void InitSave() {
            state = QuestState.not_started;
            startTime = 0;
            endTime = 0;
        } 
        #endregion

        public bool CheckCompleteConditions(IQuestService service) {
            if(!IsValidData) {
                Debug.Log($"Quest {id} data not valid...");
                return false;
            }

            bool isConditionsValid = true;
            foreach(Condition condition in Data.completeConditions ) {
                if(!condition.Check(service, this)) {
                    isConditionsValid = false;
                    break;
                }
            }
            return isConditionsValid;
        }

        public bool CheckStartConditions(IQuestService service ) {
            if (!IsValidData) {
                Debug.Log($"Quest {id} data not valid...");
                return false;
            }

            bool isConditionsValid = true;
            foreach (Condition condition in Data.startConditions) {
                if (!condition.Check(service, this)) {
                    isConditionsValid = false;
                    break;
                }
            }
            return isConditionsValid;
        }

        public void Complete() {
            QuestState oldState = state;
            state = QuestState.ready;
            if(oldState != state ) {
                endTime = Utility.unixTime;

                if (Data.journalId.IsValid()) {
                    engine.GetService<IJournalService>().OpenEndText(Data.journalId);
                }

                RavenhillEvents.OnQuestCompleted(this);
            }
        }

        public void Start() {
            QuestState oldState = state;
            state = QuestState.started;
            if(oldState != state ) {
                startTime = Utility.unixTime;

                if(Data.journalId.IsValid()) {
                    engine.GetService<IJournalService>().AddEntry(new JournalEntryInfo(Data.journalId));
                }
                RavenhillEvents.OnQuestStarted(this);
            }
        }

        public bool Reward() {
            QuestState oldState = state;
            state = QuestState.completed;
            if(oldState != state ) {
                
                RavenhillEvents.OnQuestRewarded(this);
                return true;
            }
            return false;
        }

        public bool isHasJournalEntry {
            get {
                return Data.journalId.IsValid();
            }
        }
    }
}
