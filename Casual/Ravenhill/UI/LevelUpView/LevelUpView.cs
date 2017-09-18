using System.Collections.Generic;

namespace Casual.Ravenhill.UI {
    using Casual.Ravenhill.Data;
    using UnityEngine;
    using UnityEngine.UI;

    public partial class LevelUpView : RavenhillCloseableView {
        public override RavenhillViewType viewType => RavenhillViewType.level_up_view;

        public override bool isModal => true;

        public override int siblingIndex => 2;

        public override void Setup(object objdata = null) {
            base.Setup(objdata);

            int level = (int)objdata;

            levelText.text = resourceService.GetString("LU_YetAlive").ReplaceVar("%level%", level);
            goldText.text = resourceService.GetString("LU_GoldReward").ReplaceVar("%count%", resourceService.GetSetting("gold_for_level").ValueAsInt);
            hpText.text = resourceService.GetString("LU_HealthReward").ReplaceVar("%count%", resourceService.GetSetting("max_hp_for_level").ValueAsInt);
            goldCount.text = resourceService.GetSetting("gold_for_level").ValueAsString;
            hpCount.text = resourceService.GetSetting("max_hp_for_level").ValueAsString;

        }

        protected override void OnClose() {
            base.OnClose();

            DropItem goldItem = new DropItem(DropType.gold, resourceService.GetSetting("gold_for_level").ValueAsInt);
            DropItem hpItem = new DropItem(DropType.max_health, resourceService.GetSetting("max_hp_for_level").ValueAsInt);
            engine.DropItems(new List<DropItem> { goldItem, hpItem });
        }
    }

    public partial class LevelUpView : RavenhillCloseableView {

        [SerializeField]
        private Text levelText;

        [SerializeField]
        private Text goldText;

        [SerializeField]
        private Text hpText;

        [SerializeField]
        private Text hpCount;

        [SerializeField]
        private Text goldCount;
    }
}
