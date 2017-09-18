using Casual.Ravenhill.Data;
using Casual.Ravenhill.Net;
using Casual.Ravenhill.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Casual.Ravenhill {

    public class RavenhillEngine : CasualEngine {

        private const string FOCUS_LOST_TIME = "FOCUS_LOST_TIME";
        private const string kMapRoomId = "r0";

        private bool m_IsServicesRegistered = false;
        private bool isLostFocusIntervalReceived = false;
        private int lostFocusInterval = 0;

        public string listenerName => "engine";

        private static bool s_IsCreated = false;


        public override void Awake() {
            base.Awake();

            if (s_IsCreated) {
                Destroy(gameObject);
                return;
            } else {
                DontDestroyOnLoad(gameObject);
                
                RegisterServices();
                SetupServices();

                SceneManager.sceneLoaded += (scene, mode) => {
                    var gameModeService = GetService<IGameModeService>()?.Cast<RavenhillGameModeService>();

                    if (gameModeService != null) {
                        var roomData = GetService<IResourceService>()?.Cast<RavenhillResourceService>()?.GetRoomDataBySceneName(scene.name, gameModeService.roomMode);
                        if (roomData != null) {
                            switch (roomData.roomType) {
                                case Data.RoomType.hallway: {
                                        gameModeService.SetGameModeName(GameModeName.hallway);
                                    }
                                    break;
                                case Data.RoomType.map: {
                                        gameModeService.SetGameModeName(GameModeName.map);
                                    }
                                    break;
                                case Data.RoomType.search: {
                                        gameModeService.SetGameModeName(GameModeName.search);
                                    }
                                    break;
                            }
                        }
                    }

                    RavenhillEvents.OnSceneLoaded(scene.name);
                };



                s_IsCreated = true;

                var resourceSerice = GetService<IResourceService>().Cast<RavenhillResourceService>();
                var mapData = resourceSerice.GetRoomData(RoomType.map);
                if(mapData != null ) {
                    LoadScene(mapData.id);
                }

            }
        }

        public override T GetService<T>() {
            RegisterServices();
            return base.GetService<T>();
        }

        private void RegisterServices() {
            if(!m_IsServicesRegistered) {
                ServiceRegistration();
                m_IsServicesRegistered = true;
            }
        }

        protected virtual void ServiceRegistration() {
            Register<IResourceService, RavenhillResourceService>(new RavenhillResourceService());
            Register<ISaveService, SaveService>(FindObjectOfType<SaveService>());

            Register<IViewService, RavenhillViewService>(new RavenhillViewService());
            Register<ICanvasSerive, CanvasService>(FindObjectOfType<CanvasService>());
            Register<IGameModeService, RavenhillGameModeService>(FindObjectOfType<RavenhillGameModeService>());
            Register<IPlayerService, PlayerService>(FindObjectOfType<PlayerService>());
            Register<IDebugService, DebugService>(FindObjectOfType<DebugService>());
            Register<INetService, NetService>(FindObjectOfType<NetService>());
            Register<IOfferService, OfferService>(FindObjectOfType<OfferService>());
            Register<IPurchaseService, PurchaseService>(FindObjectOfType<PurchaseService>());
            Register<IJournalService, JournalService>(FindObjectOfType<JournalService>());
            Register<IQuestService, QuestService>(FindObjectOfType<QuestService>());
            Register<IVideoService, VideoService>(FindObjectOfType<VideoService>());
            Register<INpcService, NpcService>(FindObjectOfType<NpcService>());
            Register<IAchievmentService, AchievmentService>(FindObjectOfType<AchievmentService>());
            Register<IAudioService, AudioService>(FindObjectOfType<AudioService>());
        }

        protected virtual void SetupServices() {
            GetService<IResourceService>().Setup(null);
            GetService<ISaveService>().Setup(null);
            GetService<IViewService>().Setup(Resources.Load<TextAsset>("Data/Temp/views").text);
            GetService<ICanvasSerive>().Setup(null);
            GetService<IGameModeService>().Setup(null);
            GetService<IPlayerService>().Setup(null);
            GetService<IDebugService>().Setup(null);
            GetService<INetService>().Setup(null);
            GetService<IOfferService>().Setup(null);
            GetService<IPurchaseService>().Setup(null);
            GetService<IJournalService>().Setup(null);
            GetService<IQuestService>().Setup(null);
            GetService<IVideoService>().Setup(null);
            GetService<INpcService>().Setup(null);
            GetService<IAchievmentService>().Setup(null);
            GetService<IAudioService>().Setup(null);
        }

        public void LoadScene(string roomId  ) {
            if (!GetService<IViewService>().ExistView(RavenhillViewType.loader_view)) {
                GetService<IViewService>().ShowView(RavenhillViewType.loader_view, new LoaderView.Data {
                    delay = 0.72f,
                    action = () => {
                        var roomData = GetService<IResourceService>().Cast<RavenhillResourceService>().GetRoomData(roomId);

                        if (roomData != null) {
                            RavenhillEvents.OnExitCurrentScene();
                            var gameModeService = GetService<IGameModeService>()?.Cast<RavenhillGameModeService>();
                            gameModeService.ChangeRoom(roomId);
                            SceneManager.LoadSceneAsync(roomData.GetScene(gameModeService.roomMode));
                        }
                    }
                });
            }
        }

        public void LoadPreviousScene() {
            RavenhillGameModeService gameModeService = GetService<IGameModeService>()?.Cast<RavenhillGameModeService>();
            LoadScene(gameModeService.previousRoom?.id ?? kMapRoomId);
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.SearchSessionStarted += OnSearchSessionStarted;
            RavenhillEvents.GameModeChanged += OnGameModeChanged;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.SearchSessionStarted -= OnSearchSessionStarted;
            RavenhillEvents.GameModeChanged -= OnGameModeChanged;
        }



        private void OnSearchSessionStarted(SearchSession session ) {
            LoadScene(session.roomId);
        }

        private void OnGameModeChanged(GameModeName oldGameMode, GameModeName newGameMode) {
            if (newGameMode == GameModeName.hallway || newGameMode == GameModeName.map) {
                GetService<IViewService>().ShowView(RavenhillViewType.hud);
            } else {
                GetService<IViewService>().RemoveView(RavenhillViewType.hud);
            }
        }


        public override  void OnApplicationPause(bool pause) {
            base.OnApplicationPause(pause);
            if(pause) {
                GetService<ISaveService>()?.Save();
            }
        }

        private void SaveLostFocusInterval() {
            PlayerPrefs.SetInt(FOCUS_LOST_TIME, Utility.unixTime);
            isLostFocusIntervalReceived = false;
        }

        private void LoadLostFocusInterval() {
            if (!isLostFocusIntervalReceived) {
                if (PlayerPrefs.HasKey(FOCUS_LOST_TIME)) {
                    int lostTime = PlayerPrefs.GetInt(FOCUS_LOST_TIME);
                    lostFocusInterval = Utility.unixTime - lostTime;
                    Debug.Log($"Load Lost Focus interval {lostFocusInterval}".Colored(ColorType.brown));
                }
                isLostFocusIntervalReceived = true;
            }
        }

        public int LostFocusInterval {
            get {
                LoadLostFocusInterval();
                return lostFocusInterval;
            }
        }

        private void OnApplicationFocus(bool focus) {
            if (!focus) {
                GetService<ISaveService>().Save();
                SaveLostFocusInterval();
            } else {
                LoadLostFocusInterval();
            }
        }

        private void OnApplicationQuit() {
            GetService<ISaveService>().Save();
        }

        #region Public API
        public void EnterSearchRoom(RoomInfo roomInfo) {
            var gameService = GetService<IGameModeService>()?.Cast<RavenhillGameModeService>();
            gameService.StartSession(roomInfo);
        }

        public void EndSearchSession(SearchStatus status, int time) {
            RavenhillGameModeService gameModeService = GetService<IGameModeService>()?.Cast<RavenhillGameModeService>();
            gameModeService.EndSession(status, time);
        } 
        #endregion

        public override void DropItems(List<DropItem> dropItems, Transform parent = null, System.Func<bool> dropPredicate = null) {
            StartCoroutine(CorDropItems(dropItems, parent, 0.1f, dropPredicate));
        } 

        private System.Collections.IEnumerator CorDropItems(List<DropItem> dropItems, Transform parent, float delay, System.Func<bool> dropPredicate) {

            if(dropPredicate != null ) {
                yield return new WaitUntil(dropPredicate);
            }

            foreach(DropItem item in dropItems ) {
                DropObject.Create(item, parent);
                yield return new WaitForSeconds(delay);
            }
        }

        public void Run(System.Action action, float delay) {
            StartCoroutine(CorRun(action, delay));
        }

        public void Run(System.Action action, System.Func<bool> predicate) {
            StartCoroutine(CorRun(action, predicate));
        }

        private System.Collections.IEnumerator CorRun(System.Action action, float delay) {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }

        private System.Collections.IEnumerator CorRun(System.Action action, System.Func<bool> predicate) {
            yield return new WaitUntil(predicate);
            action?.Invoke();
        }
    }
}
