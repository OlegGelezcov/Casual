using Casual.Ravenhill.Data;
using Casual.Ravenhill.UI.Map;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Casual.Ravenhill.Map {

    public class RoomMapPlank : RavenhillGameBehaviour {

        private const float kViewRemoveTimeout = 0.5f;

#pragma warning disable 0649
        [SerializeField]
        private string m_RoomId;

        [SerializeField]
        private GameObject m_RoomNameUIViewPrefab;

        [SerializeField]
        private Transform m_BindUIParent;

        [SerializeField]
        private Vector2 m_BindOffset;

        [SerializeField]
        private GameObject m_NightObject;

        [SerializeField]
        private GameObject m_LockedObject;

        [SerializeField]
        private GameObject m_OpenedDoorObject;
#pragma warning restore 0649

        private RoomInfo m_RoomInfo;


        private GameObject nightObject => m_NightObject;
        private GameObject lockedObject => m_LockedObject;
        private GameObject openedDoorObject => m_OpenedDoorObject;

        private RoomMapUIView uiInstance { get; set; } = null;

        private GameObject roomNameUIViewPrefab => m_RoomNameUIViewPrefab;
        private Transform bindUIParent => m_BindUIParent;
        private Vector2 bindOffset => m_BindOffset;

        private float lastViewRemoveTime { get; set; }

        
        private string roomId => m_RoomId;
        

        private RoomInfo roomInfo {
            get {
                if(m_RoomInfo == null ) {
                    m_RoomInfo = ravenhillGameModeService.GetRoomInfo(roomId);
                }
                return m_RoomInfo;
            }
        }

        private void UpdateLockedState() {
            if(roomInfo.isUnlocked ) {
                lockedObject.DeactivateSelf();
                openedDoorObject.ActivateSelf();
            } else {
                lockedObject.ActivateSelf();
                openedDoorObject.DeactivateSelf();
            }
        }

        private void UpdateSearchModeState() {
            if(roomInfo.searchMode == Data.SearchMode.Day ) {
                nightObject.DeactivateSelf();
            } else if(roomInfo.searchMode == Data.SearchMode.Night ) {
                nightObject.ActivateSelf();
            }
        }

        private void CreateUI() {
            if(uiInstance == null ) {
                GameObject uiGameObject = Instantiate<GameObject>(roomNameUIViewPrefab);
                canvasService.AddToFirstGroup(uiGameObject.transform);
                uiInstance = uiGameObject.GetComponent<RoomMapUIView>();
                uiInstance.Setup(roomId, bindUIParent, bindOffset);
            }
        }

        private void DestroyUI() {
            if(uiInstance != null && uiInstance) {
                Destroy(uiInstance.gameObject);
                uiInstance = null;
            }
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.ViewAdded += OnViewRemoved;
            RavenhillEvents.Touch += OnTouch;
            RavenhillEvents.RoomUnlocked += OnRoomUnlocked;
            RavenhillEvents.SearchModeChanged += OnSearchModeChanged;
            UpdateLockedState();
            UpdateSearchModeState();
            CreateUI();
        }

        public override void OnDisable() {
            base.OnDisable();
            DestroyUI();
            RavenhillEvents.ViewAdded -= OnViewRemoved;
            RavenhillEvents.Touch -= OnTouch;
            RavenhillEvents.RoomUnlocked -= OnRoomUnlocked;
            RavenhillEvents.SearchModeChanged -= OnSearchModeChanged;
        }



        public override void Update() {
            base.Update();
        }

        private void EnterRoom() {
            IViewService viewService = engine.GetService<IViewService>();
            if (!viewService.hasModals) {
                if (!EventSystem.current.IsPointerOverGameObject()) {
                    RoomInfo roomInfo = engine.GetService<IGameModeService>().Cast<RavenhillGameModeService>().GetRoomInfo(roomId);
                    //engine.Cast<RavenhillEngine>().EnterSearchRoom(roomInfo);
                    viewService.ShowView(RavenhillViewType.enter_room_view, roomInfo);
                }
            }
        }

        private void OnViewRemoved(RavenhillViewType viewType) {
            lastViewRemoveTime = Time.time;
        }

        private void OnSearchModeChanged(SearchMode oldSearchMode, SearchMode newSearchMode, RoomInfo roomInfo ) {
            if(roomInfo.id == roomId ) {
                UpdateSearchModeState();
            }
        }

        private void OnRoomUnlocked(RoomInfo roomInfo ) {
            if(roomInfo.id == roomId ) {
                UpdateLockedState();
            }
        }

        private void OnTouch(Vector2 position ) {
            if (Time.time - lastViewRemoveTime >= kViewRemoveTimeout) {
                if (Utility.RayHitObjectName2D(position) == name) {
                    EnterRoom();
                }
            }
        }

    }
}
