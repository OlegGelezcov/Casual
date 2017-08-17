using System;
using Casual.Ravenhill.Data;
using System.Collections.Generic;
using System.Linq;

namespace Casual.Ravenhill {
    public class RavenhillGameModeService : GameModeService, ISaveable {
        public RoomData previousRoom { get; private set; }
        public RoomData currentRoom { get; private set; }
        public SearchSession searchSession { get; private set; } = new SearchSession();
        public RoomMode roomMode { get; private set; } = RoomMode.normal;
        public RoomManager roomManager { get; } = new RoomManager();

        public override void Start() {
            base.Start();
            engine.GetService<ISaveService>().Register(this);
        }


        public void SetRoomMode(RoomMode roomMode ) {
            RoomMode oldRoomMode = this.roomMode;
            this.roomMode = roomMode;

            if(oldRoomMode != this.roomMode ) {
                engine.GetService<IEventService>()?.SendEvent(new RoomModeChangedEventArgs(oldRoomMode, this.roomMode));
            }
        }

        public void StartSession(RoomInfo roomInfo ) {
            searchSession.StartSession(roomInfo);
        }

        public void EndSession(SearchStatus status, int time) {
            List<InventoryItem> drops = null;
            if(status == SearchStatus.success) {
                drops = GenerateRoomDrop(searchSession.roomData, GetRoomInfo(searchSession.roomData.id).roomLevel);
            } else {
                drops = new List<InventoryItem>();
            }

            searchSession.EndSession(status, time, drops);
        }

        public void ChangeRoom(string roomId ) {
            previousRoom = currentRoom;
            currentRoom = engine.GetService<IResourceService>().Cast<RavenhillResourceService>().GetRoomData(roomId);
        }

        public RoomInfo GetRoomInfo(string roomId) {
            return roomManager.GetRoomInfo(roomId);
        }

        //Algorithm of generation room drop...
        public List<InventoryItem> GenerateRoomDrop(RoomData roomData, RoomLevel roomLevel) {
                
            RavenhillResourceService resourceService = engine.GetService<IResourceService>() as RavenhillResourceService;
            List<InventoryItem> result = new List<InventoryItem>();

            List<CollectableData> collectables = resourceService.GetCollectables(roomData.id).FindAll((item) => {
                return ((int)roomLevel >= (int)item.roomLevel) &&
                    (UnityEngine.Random.value < item.prob);
            });
            result.AddRange(collectables.Select(c => new InventoryItem(c, 1)).ToList());

            List<WeaponData> weapons = resourceService.weaponList.FindAll((w) => UnityEngine.Random.value < w.prob);
            result.AddRange(weapons.Select(w => new InventoryItem(w, 1)).ToList());

            List<IngredientData> ingredients = resourceService.GetIngredients(roomData.id).FindAll(item => {
                return UnityEngine.Random.value < item.prob;
            });
            result.AddRange(ingredients.Select(i => new InventoryItem(i, 1)).ToList());

            List<ChargerData> chargers = resourceService.chargerList.FindAll(item => UnityEngine.Random.value < item.prob);
            result.AddRange(chargers.Select(c => new InventoryItem(c, 1)).ToList());

            return result;
        }


        #region ISaveable
        public string saveId => "gamemode";

        public bool isLoaded { get; private set; }

        public string GetSave() {
            UXMLWriteElement writeElement = new UXMLWriteElement(saveId);
            writeElement.AddAttribute("room_mode", roomMode.ToString());
            writeElement.Add(roomManager.GetSave());
            return writeElement.ToString();
        }

        public bool Load(string saveStr) {
            if(string.IsNullOrEmpty(saveStr)) {
                InitSave();
            } else {
                UXMLDocument document = new UXMLDocument();
                document.Parse(saveStr);
                UXMLElement gameModeElement = document.Element(saveId);

                roomMode = gameModeElement?.GetEnum<RoomMode>("room_mode") ?? RoomMode.normal;

                UXMLElement roomsElement = gameModeElement.Element("rooms");
                if(roomsElement != null ) {
                    roomManager.Load(roomsElement);
                } else {
                    roomManager.InitSave();
                }

                isLoaded = true;
            }
            return true;
        }

        public void InitSave() {
            roomMode = RoomMode.normal;
            roomManager.InitSave();
            isLoaded = true;
        }

        public void OnRegister() {
            
        }

        public void OnLoaded() {
            
        } 
        #endregion
    }

    public class SearchSession : GameElement {
        public RoomInfo roomInfo { get; private set; }
        public SearchStatus searchStatus { get; private set; }
        public int searchTime { get; private set; }
        public bool isStarted { get; private set; }
        public List<InventoryItem> roomDropList { get; } = new List<InventoryItem>();

        public SearchSession() {
        }

        private void SetRoomInfo(RoomInfo roomInfo) {
            this.roomInfo = roomInfo;
        }

        private void SetSearchStatus(SearchStatus status ) {
            searchStatus = status;
        }

        private void SetSearchTime(int time ) {
            searchTime = time;
        }

        public void StartSession(RoomInfo roomInfo) {
            if(!isStarted) {
                SetRoomInfo(roomInfo);
                roomDropList.Clear();
                isStarted = true;
                engine.GetService<IEventService>()?.SendEvent(new SearchSessionStartedEventArgs(this));
            }
        }

        public void EndSession(SearchStatus status, int time, List<InventoryItem> drops) {
            if(isStarted) {
                SetSearchStatus(status);
                SetSearchTime(time);
                roomDropList.Clear();
                roomDropList.AddRange(drops);
                isStarted = false;
                engine.GetService<IEventService>()?.SendEvent(new SearchSessionEndedEventArgs(this));
            }
        }

        

        public string roomId {
            get => roomInfo?.id ?? string.Empty;
        }

        public RoomData roomData {
            get {
                return roomInfo?.roomData ?? null;
            }
        }
    }
}
