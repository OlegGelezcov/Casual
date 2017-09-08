using System;
using System.Linq;


namespace Casual.Ravenhill.UI {
    using Casual.Ravenhill.Data;
    using UnityEngine;
    using UnityEngine.UI;

    public partial class PatientRewardView : RavenhillCloseableView {

        public override RavenhillViewType viewType => RavenhillViewType.patient_reward_view;

        public override bool isModal => true;

        public override int siblingIndex => 1;

        private NpcInfo npc;
        private NpcData npcData;

        public override void Setup(object data = null) {
            base.Setup(data);
            npc = data as NpcInfo;
            if(npc == null ) {
                throw new UnityException("PatientRewardView: Setup NpcInfo is null");
            }
            npcData =  npc.Data;
            if(npcData == null ) {
                throw new NullReferenceException("PatientRewardView.Setup(): NpcData is null");
            }

            descriptionText.text = resourceService.GetString(npcData.descriptionId);

            DropItem expDropItem = npcData.rewards.Where(di => di.type == DropType.exp).FirstOrDefault();
            if(expDropItem != null ) {
                expParent.ActivateSelf();
                expCountText.text = expDropItem.count.ToString();
            } else {
                expParent.DeactivateSelf();
            }

            DropItem silverDropItem = npcData.rewards.Where(di => di.type == DropType.silver).FirstOrDefault();
            if(silverDropItem != null ) {
                silverParent.ActivateSelf();
                silverCountText.text = silverDropItem.count.ToString();
            } else {
                silverParent.DeactivateSelf();
            }
        }

        protected override void OnClose() {
            base.OnClose();
            if(npcData != null ) {
                engine.GetComponent<INpcService>().RemoveNpc(npc.RoomId);
                engine.Cast<RavenhillEngine>().DropItems(
                    npcData.rewards,  
                    dropPredicate: () => viewService.noModals
                    );
            }
        }
    }

    public partial class PatientRewardView : RavenhillCloseableView {

        [SerializeField]
        private Text descriptionText;

        [SerializeField]
        private GameObject expParent;

        [SerializeField]
        private Text expCountText;

        [SerializeField]
        private GameObject silverParent;

        [SerializeField]
        private Text silverCountText;
    }
}
