using Casual.Ravenhill.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI.Map {
    public class RoomMapUIView : RavenhillUIBehaviour {

        private string m_RoomId;

        [SerializeField]
        private Text m_RoomNameText;

        [SerializeField]
        private EventTrigger m_EventTrigger;

        private float viewRemovedLastTime { get; set; } = 0;

        private RoomData m_RoomData;

        private string roomId => m_RoomId;
        private Text roomNameText => m_RoomNameText;
        private EventTrigger eventTrigger => m_EventTrigger;

        private RoomData roomData {
            get {
                if(m_RoomData == null ) {
                    m_RoomData = resourceService.GetRoomData(roomId);
                }
                return m_RoomData;
            }
        }

        public override string listenerName => $"room_map_ui_view: {roomId}";

        private RectTransformBinding rectTransformBinding { get; set; }
        private Vector2 offset { get; set; }

        public void Setup(string roomId, Transform bindParent, Vector2 offset) {
            this.offset = offset;
            m_RoomId = roomId;
            roomNameText.text = resourceService.GetString(roomData.nameId);
            eventTrigger.SetEventTriggerClick((pointer) => {
                if (ravenhillViewService.noModals) {
                    if (viewRemovedInterval >= 0.5f) {
                        Debug.Log($"click on ui room {roomId}");
                    }
                }
            });
            rectTransformBinding = gameObject.GetOrAdd<RectTransformBinding>();
            rectTransformBinding.Bind(bindParent, offset, 0.02f);
        }

        public void SetOffset(Vector2 newOffset) {
            offset = newOffset;
            if(rectTransformBinding ) {
                rectTransformBinding.SetOffset(offset);
            }
        }

        public override void OnEnable() {
            base.OnEnable();
            AddHandler(GameEventName.view_removed, OnViewRemoved);
        }

        public override void OnDisable() {
            base.OnDisable();

        }


        private void OnViewRemoved(EventArgs<GameEventName> inargs ) {
            RavenhillViewRemovedEventArgs args = inargs as RavenhillViewRemovedEventArgs;
            if (args != null ) {
                viewRemovedLastTime = Time.time;
            }
        }

        private float viewRemovedInterval {
            get {
                return (Time.time - viewRemovedLastTime);
            }
        }
    }
}
