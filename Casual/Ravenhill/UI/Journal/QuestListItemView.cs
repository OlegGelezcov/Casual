using Casual.Ravenhill.Data;
using Casual.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class QuestListItemView : ListItemView<QuestData> {

        public override void Setup(QuestData data) {
            base.Setup(data);

            questNameText.text = resourceService.GetString(data.nameId);
            actionButton.SetListener(() => {
                if (engine.GetService<IJournalService>().GetEntry(data) != null) {
                    var journalView = GetComponentInParent<JournalView>();
                    journalView?.OpenDetail(data);
                } else {
                    Debug.Log($"Journal entry for quest {data.id} not founded".Colored(ColorType.yellow));
                }
            });

            var image = actionButton.GetComponent<Image>();
            if (data.type == QuestType.story || data.type == QuestType.charge_story_collection ) { 
                image.sprite = storyQuestSprite;
                image.overrideSprite = storyQuestSprite;
            } else {
                image.sprite = sideQuestSprite;
                image.overrideSprite = sideQuestSprite;
            }
        }
    }

    public partial class QuestListItemView : ListItemView<QuestData> {

        [SerializeField]
        private Button actionButton;

        [SerializeField]
        private Text questNameText;

        [SerializeField]
        private Sprite sideQuestSprite;

        [SerializeField]
        private Sprite storyQuestSprite;

    }
}
