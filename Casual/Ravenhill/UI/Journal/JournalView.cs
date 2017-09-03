using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI{
    public partial class JournalView : RavenhillCloseableView {

        public override RavenhillViewType viewType => RavenhillViewType.journal;


        public override bool isModal => true;

        public override int siblingIndex => 2;

        private int currentPage = 0;

        public override void Setup(object data = null) {
            base.Setup(data);
            detailView.Setup(engine.GetService<IJournalService>().GetAt(currentPage));


            listToggle.SetListener((isOn) => {
                if(isOn) {
                    questsView.ActivateSelf();
                    detailView.DeactivateSelf();
                    questsView.Setup();
                }
            });

            detailToggle.SetListener((isOn) => {
                if(isOn) {
                    questsView.DeactivateSelf();
                    detailView.ActivateSelf();
                    detailView.Setup(engine.GetService<IJournalService>().GetAt(currentPage));
                }
            });

            listToggle.isOn = true;
        }

        public void OpenDetail(QuestData quest ) {
            var journalService = engine.GetService<IJournalService>();
            var journalEntry = journalService.GetEntry(quest);
            if(journalEntry != null ) {
                int index = journalService.GetEntryIndex(journalEntry);
                currentPage = index;
                detailToggle.isOn = true;
            }
        }
    }

    public partial class JournalView : RavenhillCloseableView {

        [SerializeField]
        private EntryDetailView detailView;

        [SerializeField]
        private JournalQuestsView questsView;

        [SerializeField]
        private Toggle listToggle;

        [SerializeField]
        private Toggle detailToggle;

    }
}
