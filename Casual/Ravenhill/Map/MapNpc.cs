using System.Linq;
using UnityEngine;

namespace Casual.Ravenhill {
    public partial class MapNpc : RavenhillGameBehaviour {

        private NpcInfo npc;
        private NpcWalkArea walkArea;
        private Vector3 targetPosition;
        private bool isMoving = false;
        private float moveInterval;
        private float moveTimer = 0.0f;
        private float viewRemovedLastTime = 0.0f;


        public string RoomId {
            get {
                return npc?.RoomId ?? string.Empty;
            }
        }

        public void Setup(NpcInfo npc) {
            gameObject.name = "NPC_" + npc.RoomId + "_" + (npc.IsEmpty ? "empty" : npc.Data.id);

            this.npc = npc;

            if (npc.Data != null) {
                spriteRenderer.sprite = resourceService.GetSprite(npc.Data);
            } else {
                spriteRenderer.sprite = resourceService.transparent;
            }

            walkArea = FindObjectsOfType<NpcWalkArea>().Where(walkArea => walkArea.RoomId == npc.RoomId).FirstOrDefault();
            if(walkArea != null ) {
                transform.SetParent(walkArea.transform.parent);
                transform.localPosition = walkArea.transform.localPosition;
                UpdateTargetPosition();
                isMoving = true;
            } else {
                isMoving = false;
            }
        }

        private void UpdateTargetPosition() {
            if(walkArea != null ) {
                targetPosition = walkArea.NextPoint;
                moveInterval = (targetPosition - transform.position).magnitude / speed;
                moveTimer = 0.0f;
            }
        }

        private Vector3 direction {
            get {
                return (targetPosition - transform.position).normalized;
            }
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.Touch += OnTouch;
            RavenhillEvents.ViewRemoved += OnViewRemoved;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.Touch -= OnTouch;
            RavenhillEvents.ViewRemoved -= OnViewRemoved;
        }

        
        public override void Update() {
            base.Update();
            if(isMoving) {
                transform.position += direction * speed * Time.deltaTime;
                moveTimer += Time.deltaTime;
                if(moveTimer >= moveInterval ) {
                    UpdateTargetPosition();
                }
            }
        }

        public void Pause() {
            isMoving = false;
        }

        public void Restore() {
            isMoving = true;
        }

        private void OnTouch(Vector2 position ) {
            if(Utility.RayHitObjectName2D(position) == name ) {
                if (!viewService.hasModals) {
                    if((Time.time - viewRemovedLastTime) >= 0.8f ) {
                        Debug.Log($"touch on npc {npc.Data.id}");
                        if(npc != null && npc.Data != null) {
                            if(npc.Data.type == Data.NpcType.enemy ) {
                                viewService.ShowView(RavenhillViewType.kill_enemy_view, npc);
                            } else if(npc.Data.type == Data.NpcType.patient ) {
                                viewService.ShowView(RavenhillViewType.patient_reward_view, npc);
                            }
                        }
                    }
                    
                }
            }
        }

        private void OnViewRemoved(RavenhillViewType viewType) {
            viewRemovedLastTime = Time.time;
        }
    }

    public partial class MapNpc : RavenhillGameBehaviour {

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private float speed = 10.0f;

    }
}
