using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI{
    public partial class QuestStartView : RavenhillCloseableView {

        public override RavenhillViewType viewType => RavenhillViewType.quest_start_view;

        public override bool isModal => true;

        public override int siblingIndex => 3;

        public override void Setup(object data = null) {
            base.Setup(data);

            QuestData quest = data as QuestData;
            if(quest == null ) {
                throw new ArgumentException($"{this.GetType().Name} must have setup argument {typeof(QuestData).Name}");
            }

            nameText.text = resourceService.GetString(quest.nameId);
            descriptionText.text = resourceService.GetString(quest.startTextId);
            iconImage.overrideSprite = resourceService.GetSprite(quest);

            var expReward = quest.expReward;
            if(expReward != null ) {
                expRewardParent.ActivateSelf();
                expRewardCount.text = expReward.count.ToString();
            } else {
                expRewardParent.DeactivateSelf();
            }

            var silverReward = quest.silverReward;
            if(silverReward != null ) {
                silverRewardParent.ActivateSelf();
                silverRewardCount.text = silverReward.count.ToString();
            } else {
                silverRewardParent.DeactivateSelf();
            }

            if(ravenhillGameModeService.gameModeName == GameModeName.map ) {
                showTargetButton.interactable = true;
                showTargetButton.SetListener(() => Debug.Log("show target..."));
            } else {
                showTargetButton.interactable = false;
            }
        }


    }

    public partial class QuestStartView : RavenhillCloseableView {

        [SerializeField]
        private Text nameText;

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private Text descriptionText;

        [SerializeField]
        private GameObject expRewardParent;

        [SerializeField]
        private Text expRewardCount;

        [SerializeField]
        private GameObject silverRewardParent;

        [SerializeField]
        private Text silverRewardCount;

        [SerializeField]
        private Button showTargetButton;
    }
}
