﻿using Casual.Ravenhill.Data;
using Casual.Ravenhill.Net;
using Casual.Ravenhill.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Casual.Ravenhill {

    public class RavenhillEngine : CasualEngine {


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
            Register<IResourceService, RavenhillResourceService>(new RavenhillResourceService());
            Register<ISaveService, SaveService>(FindObjectOfType<SaveService>());

            Register<IViewService, RavenhillViewService>(new RavenhillViewService());
            Register<ICanvasSerive, CanvasService>(FindObjectOfType<CanvasService>());
            Register<IGameModeService, RavenhillGameModeService>(FindObjectOfType<RavenhillGameModeService>());
            Register<IPlayerService, PlayerService>(FindObjectOfType<PlayerService>());
            Register<IDebugService, DebugService>(FindObjectOfType<DebugService>());
            Register<INetService, NetService>(FindObjectOfType<NetService>());
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
        }

        public void LoadScene(string roomId  ) {   
            var roomData = GetService<IResourceService>().Cast<RavenhillResourceService>().GetRoomData(roomId);

            if(roomData != null ) {
                var gameModeService = GetService<IGameModeService>()?.Cast<RavenhillGameModeService>();
                gameModeService.ChangeRoom(roomId);
                SceneManager.LoadSceneAsync(roomData.GetScene(gameModeService.roomMode));
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

        private void OnApplicationFocus(bool focus) {
            if (!focus) {
                GetService<ISaveService>().Save();
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

        public void DropItems(List<DropItem> dropItems, Transform parent = null, System.Func<bool> dropPredicate = null) {
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
    }
}
