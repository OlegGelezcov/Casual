using Casual.Ravenhill.Data;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Casual.Ravenhill {

    public class RavenhillEngine : CasualEngine, IEventListener<GameEventName> {


        private const string kMapRoomId = "r0";

        private bool m_IsServicesRegistered = false;

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
            Register<IEventService, RavenhillEventService>(new RavenhillEventService());
            Register<IResourceService, RavenhillResourceService>(new RavenhillResourceService());
            Register<ISaveService, SaveService>(FindObjectOfType<SaveService>());

            Register<IViewService, RavenhillViewService>(new RavenhillViewService());
            Register<ICanvasSerive, CanvasService>(FindObjectOfType<CanvasService>());
            Register<IGameModeService, RavenhillGameModeService>(FindObjectOfType<RavenhillGameModeService>());
            Register<IPlayerService, PlayerService>(FindObjectOfType<PlayerService>());
            
        }

        protected virtual void SetupServices() {
            GetService<IEventService>().Setup(null);
            GetService<IResourceService>().Setup(null);
            GetService<ISaveService>().Setup(null);
            GetService<IViewService>().Setup(Resources.Load<TextAsset>("Data/Temp/views").text);
            GetService<ICanvasSerive>().Setup(null);
            GetService<IGameModeService>().Setup(null);
            GetService<IPlayerService>().Setup(null);
        }

        public void LoadScene(string roomId  ) {   
            var roomData = GetService<IResourceService>().Cast<RavenhillResourceService>().GetRoomData(roomId);

            if(roomData != null ) {
                var gameModeService = GetService<IGameModeService>()?.Cast<RavenhillGameModeService>();
                gameModeService.ChangeRoom(roomId);
                SceneManager.LoadSceneAsync(roomData.GetScene(gameModeService.roomMode));
            }
        }

        public override void OnEnable() {
            base.OnEnable();

            GetService<IEventService>().Add(GameEventName.search_session_started, this);
            GetService<IEventService>().Add(GameEventName.search_session_ended, this);
            GetService<IEventService>().Add(GameEventName.game_mode_changed, this);
        }

        public override void OnDisable() {
            base.OnDisable();
            GetService<IEventService>().Remove(GameEventName.search_session_started, this);
            GetService<IEventService>().Remove(GameEventName.search_session_ended, this);
            GetService<IEventService>().Remove(GameEventName.game_mode_changed, this);
        }

        public override  void OnApplicationPause(bool pause) {
            base.OnApplicationPause(pause);
            if(pause) {
                GetService<ISaveService>()?.Save();
            }
        }

        private void OnApplicationQuit() {
            GetService<ISaveService>()?.Save();
        }

        public void OnEvent(EventArgs<GameEventName> args) {

            switch(args.eventName) {
                case GameEventName.search_session_started: {
                        OnSearchSessionStarted(args as SearchSessionStartedEventArgs);
                    }
                    break;
                case GameEventName.search_session_ended: {
                        OnSearchSessionEnded(args as SearchSessionEndedEventArgs);
                    }
                    break;
                case GameEventName.game_mode_changed: {
                        OnGameModeChanged(args as GameModeChangedEventArgs);
                    }
                    break;
            }
        }

        private void OnGameModeChanged(GameModeChangedEventArgs args ) {
            if(args == null ) { return; }
            if(args.newGameModeName == GameModeName.hallway || args.newGameModeName == GameModeName.map ) {
                GetService<IViewService>().ShowView(RavenhillViewType.hud);
            } else {
                GetService<IViewService>().RemoveView(RavenhillViewType.hud);
            }
        }

        private void OnSearchSessionStarted(SearchSessionStartedEventArgs args) {
            if(args != null ) {
                LoadScene(args.session.roomId);
            }
        }

        private void OnSearchSessionEnded(SearchSessionEndedEventArgs args) {
            if(args != null ) {
                RavenhillGameModeService gameModeService = GetService<IGameModeService>()?.Cast<RavenhillGameModeService>();
                LoadScene(gameModeService.previousRoom?.id ?? kMapRoomId);
            }
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
    }
}
