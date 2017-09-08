using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casual.Ravenhill.Data;
using UnityEngine;
using Casual.Ravenhill.UI;

namespace Casual.Ravenhill {
    public class QuestService : RavenhillGameBehaviour, IQuestService, ISaveable {

        private readonly Dictionary<string, QuestInfo> notStartedQuests = new Dictionary<string, QuestInfo>();
        private readonly Dictionary<string, QuestInfo> startedQuests = new Dictionary<string, QuestInfo>();
        private readonly Dictionary<string, QuestInfo> completedQuests = new Dictionary<string, QuestInfo>();

        

        //private readonly List<QuestInfo> tempQuestList = new List<QuestInfo>();

        public override void Start() {
            base.Start();
            engine.GetService<ISaveService>().Register(this);
        }


        private int TryCompleteQuests() {
            List<QuestInfo> tempQuestList = new List<QuestInfo>();

            foreach (var questPair in startedQuests ) {
                var quest = questPair.Value;
                if(quest.CheckCompleteConditions(this)) {
                    tempQuestList.Add(quest);
                }
            }

            int count = tempQuestList.Count();
            tempQuestList.ForEach(quest => startedQuests.Remove(quest.id));
            tempQuestList.ForEach(quest => completedQuests[quest.id] = quest);
            tempQuestList.ForEach(quest => quest.Complete());
            RewardQuests();

            return count;
        }

        private void RewardQuests() {
            StartCoroutine(CorRewardQuests());
        }

        private System.Collections.IEnumerator CorRewardQuests() {
            var targets = completedQuests.Values.Where(quest => quest.state == QuestState.ready).ToList();
            if(targets.Count > 0 ) {
                foreach(QuestInfo quest in targets ) {
                    yield return StartCoroutine(CorShowRewardQuest(quest));
                }
            }
        }

        private System.Collections.IEnumerator CorShowRewardQuest(QuestInfo quest) {
            yield return new WaitUntil(() => 
            (ravenhillGameModeService.gameModeName == GameModeName.map || ravenhillGameModeService.gameModeName == GameModeName.hallway) &&
            (viewService.hasModals == false));
            if(quest.state == QuestState.ready ) {
                if(quest.Data.endTextId.IsValid() && quest.Data.ownerId.IsValid()) {
                    QuestDialogView.Data dialogData = new QuestDialogView.Data {
                        isStart = false,
                        quest = quest.Data
                    };
                    viewService.ShowView(RavenhillViewType.quest_dialog_view, dialogData);
                } else {
                    viewService.ShowView(RavenhillViewType.quest_end_view, quest.Data);
                }
            }
        }

        private int TryStartQuests() {
            Debug.Log($"not started quests count = {notStartedQuests.Count}");
            List<QuestInfo> tempQuestList = new List<QuestInfo>();

            foreach (var questPair in notStartedQuests ) {
                var quest = questPair.Value;
                if(quest.CheckStartConditions(this)) {
                    tempQuestList.Add(quest);
                }
            }

            int count = tempQuestList.Count();
            tempQuestList.ForEach(quest => notStartedQuests.Remove(quest.id));
            tempQuestList.ForEach(quest => startedQuests[quest.id] = quest);
            tempQuestList.ForEach(quest => quest.Start());
            return count;
        }


        public bool RewardQuest(string id ) {
            if(completedQuests.ContainsKey(id)) {
                return completedQuests[id].Reward();
            }
            return false;
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.InventoryItemAdded += OnItemAdded;
            RavenhillEvents.SearchCounterChanged += OnSearchCounterChanged;
            RavenhillEvents.QuestCompleted += OnQuestCompleted;
            RavenhillEvents.GameModeChanged += OnGameModeChanged;
            RavenhillEvents.QuestStarted += OnQuestStarted;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.InventoryItemAdded -= OnItemAdded;
            RavenhillEvents.SearchCounterChanged -= OnSearchCounterChanged;
            RavenhillEvents.QuestCompleted -= OnQuestCompleted;
            RavenhillEvents.GameModeChanged -= OnGameModeChanged;
            RavenhillEvents.QuestStarted -= OnQuestStarted;
        }

        private void OnQuestStarted(QuestInfo quest) {
            if(quest.Data.ownerId.IsValid()) {
                viewService.ShowViewWithCondition(RavenhillViewType.quest_dialog_view, () => {
                    return ravenhillGameModeService.gameModeName == GameModeName.map || ravenhillGameModeService.gameModeName == GameModeName.hallway;
                }, new QuestDialogView.Data {  quest = quest.Data, isStart = true });
            } else {
                viewService.ShowViewWithCondition(RavenhillViewType.quest_start_view, () => {
                    return ravenhillGameModeService.gameModeName == GameModeName.map || ravenhillGameModeService.gameModeName == GameModeName.hallway;
                }, quest.Data);
            }
        }

        private void OnItemAdded(InventoryItemType type, string id, int count) {
            if(type == InventoryItemType.StoryCollectable ||
                type == InventoryItemType.StoryCollection ||
                type == InventoryItemType.Collectable ||
                type == InventoryItemType.Collection ) {
                int completedQuestCount = TryCompleteQuests();
                Debug.Log($"{completedQuestCount} completed when inventory item added...");
            }
        }

        private void OnSearchCounterChanged(int counter) {
            TryCompleteQuests();
        }

        private void OnQuestCompleted(QuestInfo quest) {
            if(quest.Data.type == QuestType.story || 
                quest.Data.type == QuestType.charge_story_collection ) {
                ravenhillGameModeService.ResetSearchCounter();
            }
        }



        private System.Collections.IEnumerator CorStartQuests() {
            yield return new WaitForSeconds(UnityEngine.Random.Range(1.0f, 3.0f));
            Debug.Log("try start quests...");
            int count = TryStartQuests();
            if(count > 0 ) {
                Debug.Log($"{count} of quests started....");
            }
        }

        private void OnGameModeChanged(GameModeName oldGameMode, GameModeName newGameMode ) {
            if(newGameMode == GameModeName.hallway ||
                newGameMode == GameModeName.map ) {
                RewardQuests();

                StartCoroutine(CorStartQuests());
            }
        }

        #region IQuestService
        public int playerLevel => playerService.level;

        public int searchCounter => ravenhillGameModeService.searchCounter;

        public bool IsCompleted(string id) {
            return completedQuests.ContainsKey(id);
        }

        public bool IsHasCollectable(string id) {
            var data = resourceService.GetCollectable(id);
            return playerService.GetItemCount(data) > 0;
        }

        public bool IsHasCollection(string id) {
            var data = resourceService.GetCollection(id);
            return playerService.GetItemCount(data) > 0;
        }

        public bool IsHasStoryCollection(string id) {
            var data = resourceService.GetStoryCollection(id);
            return playerService.GetItemCount(data) > 0;
        }

        public bool IsLastSearchRoom(string id) {
            return (ravenhillGameModeService.lastSearchRoomId == id);
        }

        public bool IsRoomMode(RoomMode mode) {
            return (ravenhillGameModeService.roomMode == mode);
        }

        public void Setup(object data) {
        }

        public QuestInfo GetInfo(QuestData data) {
            if(notStartedQuests.ContainsKey(data.id)) {
                return notStartedQuests[data.id];
            }
            if(startedQuests.ContainsKey(data.id)) {
                return startedQuests[data.id];
            }
            if(completedQuests.ContainsKey(data.id)) {
                return completedQuests[data.id];
            }
            return null;
        }
        #endregion


        #region ISaveable

        public string saveId => "quests";

        public bool isLoaded { get; private set; }

        public List<QuestInfo> startedQuestList => new List<QuestInfo>(startedQuests.Values);

        public List<QuestInfo> completedQuestList => new List<QuestInfo>(completedQuests.Values);


        public string GetSave() {
            UXMLWriteElement root = new UXMLWriteElement(saveId);

            UXMLWriteElement startedRoot = new UXMLWriteElement("started");
            foreach(var startedPair in startedQuests ) {
                startedRoot.Add(startedPair.Value.GetSave());
            }

            UXMLWriteElement completedRoot = new UXMLWriteElement("completed");
            foreach(var completedPair in completedQuests ) {
                completedRoot.Add(completedPair.Value.GetSave());
            }

            root.Add(startedRoot);
            root.Add(completedRoot);
            return root.ToString();
        }

        public void InitSave() {
            startedQuests.Clear();
            completedQuests.Clear();

            foreach(QuestData questData in resourceService.questList ) {
                notStartedQuests[questData.id] = new QuestInfo(questData);
            }
            isLoaded = true;
        }

        public bool Load(string saveStr) {
            if(string.IsNullOrEmpty(saveStr)) {
                InitSave();
            } else {
                UXMLDocument document = UXMLDocument.FromXml(saveStr);
                var root = document.Element(saveId);

                startedQuests.Clear();
                completedQuests.Clear();
                notStartedQuests.Clear();

                var startedElement = root.Element("started");
                if(startedElement != null ) {
                    startedElement.Elements("quest").ForEach(questElement => {
                        QuestInfo info = new QuestInfo(questElement);
                        if(info.IsValidData ) {
                            startedQuests[info.Data.id] = info;
                        }
                    });
                }

                var completedElement = root.Element("completed");
                if(completedElement != null ) {
                    completedElement.Elements("quest").ForEach(questElement => {
                        QuestInfo info = new QuestInfo(questElement);
                        if(info.IsValidData) {
                            completedQuests[info.Data.id] = info;
                        }
                    });
                }

                resourceService.questList.ForEach(questData => {
                    if ((!startedQuests.ContainsKey(questData.id)) && (!completedQuests.ContainsKey(questData.id))) {
                        notStartedQuests.Add(questData.id, new QuestInfo(questData));
                    }
                });
                isLoaded = true;
            }
            return isLoaded;
        }

        public void OnLoaded() {
            Debug.Log($"Quests loaded, not started => {notStartedQuests.Count}, started => {startedQuests.Count}, completed => {completedQuests.Count}");
        }

        public void OnRegister() {
            Debug.Log("Quests service registered on SAVE".Colored(ColorType.aqua));
        } 
        #endregion
    }
}
