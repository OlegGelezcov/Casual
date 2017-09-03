using Casual.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.Ravenhill.UI {
    public partial class JournalQuestsView : RavenhillUIBehaviour {

        public void Setup() {
            var journalService = engine.GetService<IJournalService>();

            var filteredStartQuests = engine.GetService<IQuestService>()
                .startedQuestList.Where(quest => quest.isHasJournalEntry && journalService.GetEntry(quest.Data) != null)
                .OrderByDescending(quest => quest.startTime)
                .Select(quest => quest.Data)
                .ToList();

            startedQuestView.Clear();
            startedQuestView.Setup(new ListView<Data.QuestData>.ListViewData { dataList = filteredStartQuests });

            var filteredCompletedQuests = engine.GetService<IQuestService>()
                .completedQuestList.Where(quest => quest.isHasJournalEntry && journalService.GetEntry(quest.Data) != null)
                .OrderByDescending(quest => quest.endTime)
                .Take(50)
                .Select(quest => quest.Data)
                .ToList();
            completedQuestView.Clear();
            completedQuestView.Setup(new ListView<Data.QuestData>.ListViewData { dataList = filteredCompletedQuests });
        }
    }

    public partial class JournalQuestsView : RavenhillUIBehaviour {

        [SerializeField]
        private QuestListView startedQuestView;

        [SerializeField]
        private QuestListView completedQuestView;
    }
}
