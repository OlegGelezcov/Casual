using System;

namespace Casual.Ravenhill.UI {
    using Casual.Ravenhill.Data;
    using UnityEngine;
    using UnityEngine.UI;

    public partial class AchievmentRankView : RavenhillCloseableView {

        public override RavenhillViewType viewType => RavenhillViewType.achievment_rank_view;

        public override bool isModal => true;

        public override int siblingIndex => 3;

        public class Data {
            public AchievmentInfo info;
            public AchievmentTierData tier;
        }

        private Data data = null;

        public override void Setup(object objdata = null) {
            base.Setup(objdata);
            data = objdata as Data;
            if(data == null) {
                throw new ArgumentException("objdata");
            }

            if(data.tier.rewardItem != null ) {

                InventoryItemData rewardData = data.tier.rewardItem.itemData;
                titleText.text = resourceService.GetString(rewardData.nameId);
                rewardImage.overrideSprite = resourceService.GetSprite(rewardData);
                rewardCountText.text = data.tier.rewardItem.count.ToString();

                string medalKey = resourceService.GetMedalKey(data.tier.tier);
                if(medalKey.IsValid()) {
                    medalImage.overrideSprite = resourceService.GetSprite(medalKey);
                } else {
                    medalImage.overrideSprite = resourceService.transparent;
                }

            }
        }

        protected override void OnClose() {
            base.OnClose();
            if(data != null ) {
                engine.GetService<IAchievmentService>().RewardAchievment(data.info.Id);
            }
        }
    }

    public partial class AchievmentRankView : RavenhillCloseableView {

        [SerializeField]
        private Text titleText;

        [SerializeField]
        private Image medalImage;

        [SerializeField]
        private Image rewardImage;

        [SerializeField]
        private Text rewardCountText;

        
    }
}
