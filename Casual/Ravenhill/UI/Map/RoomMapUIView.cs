using Casual.Ravenhill.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI.Map {
    public class RoomMapUIView : RavenhillUIBehaviour {

        private string m_RoomId;

#pragma warning disable 0649
        [SerializeField]
        private Text m_RoomNameText;

        [SerializeField]
        private EventTrigger m_EventTrigger;

        [SerializeField]
        private RoomBuffsView buffsView;

#pragma warning restore 0649

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
            buffsView.Setup(roomId);
        }

        public void SetOffset(Vector2 newOffset) {
            offset = newOffset;
            if(rectTransformBinding ) {
                rectTransformBinding.SetOffset(offset);
            }
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.ViewRemoved += OnViewRemoved;
            RavenhillEvents.NpcCreated += OnNpcCreated;
            RavenhillEvents.NpcRemoved += OnNpcRemoved;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.ViewRemoved -= OnViewRemoved;
            RavenhillEvents.NpcCreated -= OnNpcCreated;
            RavenhillEvents.NpcRemoved -= OnNpcRemoved;
        }


        private void OnViewRemoved(RavenhillViewType viewType ) {
            viewRemovedLastTime = Time.time;
        }

        private float viewRemovedInterval {
            get {
                return (Time.time - viewRemovedLastTime);
            }
        }

        private void OnNpcCreated(string roomId, NpcInfo info ) {
            buffsView.Setup(this.roomId);
        }

        private void OnNpcRemoved(string roomId, NpcData mpcData ) {
            buffsView.Setup(this.roomId);
        }
    }
}
