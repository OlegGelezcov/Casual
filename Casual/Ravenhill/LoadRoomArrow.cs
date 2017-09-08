using Casual.Ravenhill.Data;
using Casual.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Casual.Ravenhill {
    public partial class LoadRoomArrow : RavenhillGameBehaviour {

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.Touch += OnTouch;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.Touch -= OnTouch;
        }

        private void OnTouch(Vector2 position ) {
            if (viewService.noModals && (!EventSystem.current.IsPointerOverGameObject())) {
                if (IsHittedOnMe2D(position)) {
                    if (viewService.LastViewRemoveInterval >= kClickDelayAfterRemoveView) {
                        RoomData roomData = resourceService.GetRoomData(roomId);

                        if (roomData != null) {
                            LoadRoom(roomData);
                        }
                    }
                }
            }
        }

        private void LoadRoom(RoomData roomData) {

            float start = transform.localScale.x;
            float end = start + 0.1f;

            gameObject.GetOrAdd<TransformAnimScale>().StartAnim(new MCFloatAnimData {
                duration = 0.15f,
                start = start,
                end = end,
                overwriteEasing = new OverwriteEasing { type = MCEaseType.EaseInOutCubic },
                endAction = () => {
                    gameObject.GetOrAdd<TransformAnimScale>().StartAnim(new MCFloatAnimData {
                        duration = 0.15f,
                        end = start,
                        start = end,
                        overwriteEasing = new OverwriteEasing { type = MCEaseType.EaseInOutCubic },
                        endAction = () => {
                            switch (roomData.roomType) {
                                case RoomType.search: {
                                        RoomInfo roomInfo = ravenhillGameModeService.GetRoomInfo(roomData.id);
                                        viewService.ShowView(RavenhillViewType.enter_room_view, roomInfo);
                                    }
                                    break;
                                default: {
                                        engine.Cast<RavenhillEngine>().LoadScene(roomData.id);
                                    }
                                    break;
                            }
                        }
                    });
                }
            });

        }
    }

    public partial class LoadRoomArrow : RavenhillGameBehaviour {

        [SerializeField]
        private string roomId;

    }
}
