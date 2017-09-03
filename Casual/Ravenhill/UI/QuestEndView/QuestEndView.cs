using Casual.Ravenhill.Data;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class QuestEndView : RavenhillCloseableView {
        public override RavenhillViewType viewType => RavenhillViewType.quest_end_view;

        public override bool isModal => true;

        public override int siblingIndex => 3;

        private QuestData questData = null;

        public override void Setup(object data = null) {
            base.Setup(data);
            questData = data as QuestData;
            if(questData == null ) {
                throw new ArgumentException($"{this.GetType().Name} must have setup parameter of type {typeof(QuestData).Name}");
            }

            nameText.text = resourceService.GetString(questData.nameId);
            descriptionText.text = resourceService.GetString(questData.endTextId);

            var expReward = questData.expReward;
            if(expReward != null ) {
                expRewardParent.ActivateSelf();
                expCountText.text = expReward.count.ToString();
            } else {
                expRewardParent.DeactivateSelf();
            }

            var silverReward = questData.silverReward;
            if(silverReward != null ) {
                silverRewardParent.ActivateSelf();
                silverCountText.text = silverReward.count.ToString();
            } else {
                silverRewardParent.DeactivateSelf();
            }
        }

        protected override void OnClose() {
            base.OnClose();
            if(questData != null ) {
                engine.Cast<RavenhillEngine>().DropItems(questData.rewards);
                engine.GetService<IQuestService>().RewardQuest(questData.id);
            }
        }
    }

    public partial class QuestEndView : RavenhillCloseableView {

        [SerializeField]
        private Text nameText;

        [SerializeField]
        private Text descriptionText;

        [SerializeField]
        private Text expCountText;

        [SerializeField]
        private Text silverCountText;

        [SerializeField]
        private GameObject expRewardParent;

        [SerializeField]
        private GameObject silverRewardParent;
    }
}
