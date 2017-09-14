using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.UI{
    using UnityEngine;
    using UnityEngine.UI;

    public partial class DailyRewardView : RavenhillCloseableView {

        private int day = 0;

        public override RavenhillViewType viewType => RavenhillViewType.daily_reward_view;

        public override bool isModal => true;

        public override int siblingIndex => 2;

        public override void Setup(object objdata = null) {
            base.Setup(objdata);
            day = (int)objdata;
            day = Mathf.Clamp(day, 1, 5);

            medals.ToList().ForEach(medal => medal.Setup(day));

            acceptButton.SetListener(Close);
        }

        protected override void OnClose() {
            base.OnClose();
            var rewards = resourceService.GetDailyRewards(day);
            engine.DropItems(rewards);
        }
    }

    public partial class DailyRewardView : RavenhillCloseableView {
        [SerializeField]
        private MedalView[] medals;

        [SerializeField]
        private Button acceptButton;

    }
}
