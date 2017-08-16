using Casual.Ravenhill.UI.Map;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Casual.Ravenhill.Map {

    public class RoomMapPlank : RavenhillBaseListenerBehaviour {

        private const float kViewRemoveTimeout = 0.5f;

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
        

        public override string listenerName => "room_map_plank" + roomId;

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
            AddHandler(GameEventName.view_removed, OnViewRemoved);
            AddHandler(GameEventName.touch, OnTouch);
            AddHandler(GameEventName.room_unlocked, OnRoomUnlockedChanged);
            AddHandler(GameEventName.search_mode_changed, OnSearchModeChanged);
            UpdateLockedState();
            UpdateSearchModeState();
            CreateUI();
        }

        public override void OnDisable() {
            base.OnDisable();
            DestroyUI();
        }

        public override void Update() {
            base.Update();
        }



        private void EnterRoom() {
            IViewService viewService = engine.GetService<IViewService>();
            if (!viewService.hasModals) {
                if (!EventSystem.current.IsPointerOverGameObject()) {
                    //var roomData = engine.GetService<IResourceService>().Cast<RavenhillResourceService>().GetRoomData(roomId);
                    RoomInfo roomInfo = engine.GetService<IGameModeService>().Cast<RavenhillGameModeService>().GetRoomInfo(roomId);
                    engine.Cast<RavenhillEngine>().EnterSearchRoom(roomInfo);
                }
            }
        }

        private void OnSearchModeChanged(EventArgs<GameEventName> inargs ) {
            SearchModeChangedEventArgs args = inargs as SearchModeChangedEventArgs;
            if(args == null ) { return; }
            if(args.roomInfo.id == roomId ) {
                UpdateSearchModeState();
            }
        }

        private void OnViewRemoved(EventArgs<GameEventName> inargs) {
            RavenhillViewRemovedEventArgs args = inargs as RavenhillViewRemovedEventArgs;
            if(args != null ) {
                lastViewRemoveTime = Time.time;
            }
        }

        private void OnRoomUnlockedChanged(EventArgs<GameEventName> inargs ) {
            RoomUnlockedEventArgs args = inargs as RoomUnlockedEventArgs;
            if(args == null ) { return; }

            if(args.roomInfo.id == roomId ) {
                UpdateLockedState();
            }
        }

        private void OnTouch(EventArgs<GameEventName> inargs ) {
            TouchEventArgs args = inargs as TouchEventArgs;
            if(args != null ) {
                if(Time.time - lastViewRemoveTime >= kViewRemoveTimeout ) {
                    if(Utility.RayHitObjectName2D(args.position) == name ) {
                        EnterRoom();
                    }
                }
            }
        }
    }
}
