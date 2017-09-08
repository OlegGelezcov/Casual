using Casual.Ravenhill.Data;
using UnityEngine;

namespace Casual.Ravenhill {
    public partial class HallwayNpc : RavenhillGameBehaviour {

        public override void Start() {
            base.Start();

            var npcService = engine.GetService<INpcService>();
            string roomId = ravenhillGameModeService.currentRoom?.id ?? string.Empty;

            if(npcService.HasNpc(npcId, roomId)) {

            } else {
                gameObject.DeactivateSelf();
            }
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.Touch += OnTouch;
            RavenhillEvents.NpcRemoved += OnNpcRemoved;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.Touch -= OnTouch;
            RavenhillEvents.NpcRemoved -= OnNpcRemoved;
        }

        private void OnTouch(Vector2 position) {
            if(IsValidTouchOnMe(position)) {
                string roomId = ravenhillGameModeService.currentRoom?.id ?? string.Empty;
                NpcInfo info = engine.GetService<INpcService>().GetNpc(npcId, roomId);
                if(info != null && (!info.IsEmpty)) {
                    switch(info.Data.type) {
                        case NpcType.enemy: {
                                viewService.ShowView(RavenhillViewType.kill_enemy_view, info);
                            }
                            break;
                        case NpcType.patient: {
                                viewService.ShowView(RavenhillViewType.patient_reward_view, info);
                            }
                            break;
                    }
                }
            }
        }

        private void OnNpcRemoved(string roomId, NpcData npcData ) {
            string currentRoomId = ravenhillGameModeService.currentRoom?.id ?? string.Empty;
            if(currentRoomId == roomId && npcId == npcData.id ) {
                gameObject.DeactivateSelf();
            }
        }
    }

    public partial class HallwayNpc : RavenhillGameBehaviour {

        [SerializeField]
        private string npcId;
    }
}
