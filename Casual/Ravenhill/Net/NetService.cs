using Casual.Ravenhill.Data;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Casual.Ravenhill.Net {
    public class NetService : RavenhillGameBehaviour, INetService, ISaveable {

        private NetPlayer localPlayer = null;

        public override void Start() {
            base.Start();
            engine.GetService<ISaveService>().Register(this);
        }


        public NetRoomScore GetBestRoomScore(SearchSession session) {
            return new NetRoomScore(session.roomId, ravenhillGameModeService.roomMode, session.searchMode, UnityEngine.Random.Range(100, 200), 1,
                new NetPlayer(System.Guid.NewGuid().ToString(), "Linux Torvalds", "Avatar2", UnityEngine.Random.Range(1, 20), true));
        }

        public NetRoomScore GetBestRoomScore(RoomInfo roomInfo ) {
            return new NetRoomScore(roomInfo.roomData.id, ravenhillGameModeService.roomMode, roomInfo.searchMode, UnityEngine.Random.Range(100, 200), 1,
                new NetPlayer(System.Guid.NewGuid().ToString(), "Linux Torvands", "Avatar3", UnityEngine.Random.Range(1, 20), true));
        }

        public NetRoomScore GetPlayerBestRoomScore(SearchSession session) {
            return new NetRoomScore(session.roomId, ravenhillGameModeService.roomMode, session.searchMode, UnityEngine.Random.Range(100, 200),
                UnityEngine.Random.Range(1, 100), new NetPlayer("MyPlayer", "Oleg", "Avatar3", 12, true));
        }

        public NetRoomScore GetPlayerBestRoomScore(RoomInfo roomInfo ) {
            return new NetRoomScore(roomInfo.roomData.id, ravenhillGameModeService.roomMode, roomInfo.searchMode, UnityEngine.Random.Range(100, 200),
                UnityEngine.Random.Range(1, 100), new NetPlayer("MyPlayer", "Oleg", "Avatar3", 12, true));
        }

        public int GetRank(SearchSession session) {
            return 23;
        }

        public void Setup(object data) {
            
        }

        public void ShareWishlist(List<InventoryItemData> items ) {
            Debug.Log($"Share items: {items.Count}");
        }

        public void Ask(InventoryItemData data) {
            Debug.Log($"ask item {data.id}");
        }

        public NetPlayer LocalPlayer {
            get {
                if(localPlayer == null || !localPlayer.id.IsValid() ) {
                    localPlayer = new NetPlayer(id: System.Guid.NewGuid().ToString(),
                        name: engine.GetService<IPlayerService>().PlayerName,
                        avatarId: engine.GetService<IPlayerService>().avatarId,
                        level: engine.GetService<IPlayerService>().level,
                        valid: true);
                }
                return localPlayer;
            }
        }

        public string saveId => "net_service";

        public bool isLoaded { get; private set; }

        public string GetSave() {
            UXMLWriteElement root = new UXMLWriteElement(saveId);
            root.Add(LocalPlayer.GetSave());
            return root.ToString();
        }

        public bool Load(string saveStr) {
            UXMLDocument document = UXMLDocument.FromXml(saveStr);
            UXMLElement root = document.Element(saveId);
            if(root != null ) {
                UXMLElement localPlayerELement = root.Element("local_player");
                if(localPlayerELement != null ) {
                    localPlayer = new NetPlayer();
                    localPlayer.Load(localPlayerELement);
                } else {
                    localPlayer = new NetPlayer();
                    localPlayer.InitSave();
                }
            }
            isLoaded = true;
            return isLoaded;
        }

        public void InitSave() {
            if(localPlayer != null ) {
                localPlayer.InitSave();
            }
            isLoaded = true;
        }

        public void OnRegister() {
            
        }

        public void OnLoaded() {
            
        }

        public bool IsLocalPlayer(ISender sender) {
            return (LocalPlayer.id == sender.GetId());
        }

        public void SendGift(IGift gift) {
            if(playerService.GetItemCount(gift.GetItemData()) > 0 ) {
                playerService.RemoveItem(gift.GetItemData(), 1);
                //really gift send here
                RavenhillEvents.OnGiftSendedSuccess(gift);
            }
        }
    }






}
