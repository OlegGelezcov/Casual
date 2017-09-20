using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.UI {
    using Casual.Ravenhill.Data;
    using Casual.UI;
    using UnityEngine;
    using UnityEngine.UI;

    public partial class ScreenQuestsView : RavenhillBaseView {
        public override RavenhillViewType viewType => RavenhillViewType.screen_quest_list;

        public override bool isModal => false;

        public override int siblingIndex => -5;

        public override void Setup(object objdata = null) {
            base.Setup(objdata);
            UpdateList();
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.QuestStarted += OnQuestStarted;
            RavenhillEvents.QuestCompleted += OnQuestCompleted;
            RavenhillEvents.QuestRewarded += OnQuestRewarded;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.QuestStarted -= OnQuestStarted;
            RavenhillEvents.QuestCompleted -= OnQuestCompleted;
            RavenhillEvents.QuestRewarded -= OnQuestRewarded;
        }

        private void OnQuestStarted(QuestInfo quest) {
            UpdateList();
        }

        private void OnQuestCompleted(QuestInfo quest) {
            UpdateList();
        }

        private void OnQuestRewarded(QuestInfo quest) {
            UpdateList();
        }

        private void UpdateList() {
            listView.Clear();

            List<QuestInfo> storyQuests = engine.GetService<IQuestService>().GetQuests((quest) =>
                (quest.state == QuestState.started || quest.state == QuestState.ready) &&
                (quest.type == QuestType.story || quest.type == QuestType.charge_story_collection));
            List<QuestInfo> otherQuests = engine.GetService<IQuestService>().GetQuests((quest) =>
                (quest.state == QuestState.started || quest.state == QuestState.ready) &&
                (quest.type != QuestType.story) && (quest.type != QuestType.charge_story_collection));
            storyQuests.AddRange(otherQuests);
            ScreenQuestListView.ListViewData listData = new ListView<QuestInfo>.ListViewData {
                dataList = storyQuests
            };
            listView.Setup(listData);
        }
    }

    public partial class ScreenQuestsView : RavenhillBaseView {

        [SerializeField]
        private ScreenQuestListView listView;


    }
}
