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

    public partial class AchievmentListItemView : ListItemView<AchievmentData> {

        public override void Setup(AchievmentData data) {
            base.Setup(data);

            achievmentIconImage.overrideSprite = resourceService.GetSprite(data);
            achievmentNameText.text = resourceService.GetString(data.nameId);

            AchievmentInfo aInfo = engine.GetService<IAchievmentService>().GetAchievment(data);
            AchievmentTierData nextTierData = aInfo.NextTier;
            if(nextTierData != null ) {
                descriptionText.text = resourceService.GetString(data.rankDescriptionId).ReplaceVar("%count%", nextTierData.count);
                progressImage.fillAmount = Mathf.Clamp01((float)aInfo.Count / (float)nextTierData.count);
                countText.text = $"{aInfo.Count}/{nextTierData.count}";

                if(nextTierData.rewardItem != null && nextTierData.rewardItem != null) {
                    rewardIconImage.overrideSprite = resourceService.GetSprite(nextTierData.rewardItem);
                    rewardCountText.text = nextTierData.rewardItem.count.ToString();
                }  else {
                    ClearRewardView();
                }
            } else {
                descriptionText.text = resourceService.GetString(data.descriptionId);
                progressImage.fillAmount = 1.0f;
                countText.text = string.Empty;
                ClearRewardView();
            }

            foreach(AchievmentMedalView medalView in medalViews ) {
                medalView.Setup(data);
            }

            if(aInfo.HasReward ) {
                takeButton.ActivateSelf();
                takeButton.SetListener(() => {
                    if (aInfo.HasReward && nextTierData != null) {
                        engine.Cast<RavenhillEngine>().DropItems(new List<DropItem> { aInfo.NextTier.rewardItem });
                        aInfo.GoToNextTier();
                        Setup(data);
                        RavenhillEvents.OnAchievmentRewarded(data, nextTierData);
                    }
                });
            } else {
                takeButton.DeactivateSelf();
            }
        }

        private void ClearRewardView() {
            rewardIconImage.overrideSprite = resourceService.transparent;
            rewardCountText.text = string.Empty;
        }
    }

    public partial class AchievmentListItemView : ListItemView<AchievmentData> {

        [SerializeField]
        private Image achievmentIconImage;

        [SerializeField]
        private Text achievmentNameText;

        [SerializeField]
        private Text descriptionText;

        [SerializeField]
        private Image progressImage;

        [SerializeField]
        private Text countText;

        [SerializeField]
        private Image rewardIconImage;

        [SerializeField]
        private Text rewardCountText;

        [SerializeField]
        private AchievmentMedalView[] medalViews;

        [SerializeField]
        private Button takeButton;
    }
}
