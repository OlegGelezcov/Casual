using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casual.Ravenhill.Data;
using UnityEngine;

namespace Casual.Ravenhill {
    public class NpcService : RavenhillGameBehaviour, INpcService, ISaveable {

        private readonly Dictionary<string, NpcInfo> npcs = new Dictionary<string, NpcInfo>();

        public void Setup(object data) {
        }

        public override void Start() {
            base.Start();
            engine.GetService<ISaveService>().Register(this);
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.GameModeChanged += OnGameModeChanged;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.GameModeChanged -= OnGameModeChanged;
        }

        private void OnGameModeChanged(GameModeName oldGameMode, GameModeName newGameMode ) {
            if(newGameMode == GameModeName.map || newGameMode == GameModeName.hallway ) {
                TryGenerateNPCs();
            }
        }

        private void TryGenerateNPCs() {
            foreach(var pair in Npcs) {
                var info = pair.Value;
                if(info.IsEmpty) {
                    NpcData targetData = null;
                    foreach(NpcData data in resourceService.npcList.Shuffled()) {
                        if(UnityEngine.Random.value < data.prob ) {
                            targetData = data;
                            break;
                        }
                    }
                    if(targetData != null ) {
                        info.SetNpc(targetData);
                        RavenhillEvents.OnNpcCreated(pair.Key, pair.Value);
                    }
                }
            }
        }

        public void RemoveNpc(string roomId ) {
            var dict = Npcs;
            if(dict.ContainsKey(roomId)) {
                if(!dict[roomId].IsEmpty) {
                    NpcData targetData = dict[roomId].Data;
                    npcs[roomId].RemoveNpc();
                    RavenhillEvents.OnNpcRemoved(roomId, targetData);
                }
            }
        }

        public List<BuffData> GetBuffs(string roomId) {
            List<BuffData> result = new List<BuffData>();
            foreach(var pair in Npcs ) {
                if(!pair.Value.IsEmpty) {
                    var sourceHallway = resourceService.GetRoomData(pair.Key);
                    if(sourceHallway.IsLinked(roomId)) {
                        if(pair.Value.Data.buffId.IsValid()) {
                            result.Add(resourceService.GetBuff(pair.Value.Data.buffId));
                        }
                    }
                }
            }
            return result;
        }


        public Dictionary<string, NpcInfo> Npcs {
            get {
                ValidateNpcDictionary();
                return npcs;
            }
        }

        private void ValidateNpcDictionary() {
            foreach(RoomData roomData in resourceService.roomList.Where(r => r.roomType == RoomType.hallway)) {
                if(!npcs.ContainsKey(roomData.id)) {
                    npcs.Add(roomData.id, new NpcInfo(roomData.id));
                }
            }
        }

        #region ISaveable
        public string saveId => "npc_service";

        public bool isLoaded { get; private set; }

        public string GetSave() {
            UXMLWriteElement root = new UXMLWriteElement(saveId);
            foreach(var pair in npcs ) {
                root.Add(pair.Value.GetSave());
            }
            return root.ToString();
        }

        public bool Load(string saveStr) {
            if(saveStr.IsValid()) {
                UXMLDocument document = new UXMLDocument();
                document.Parse(saveStr);

                npcs.Clear();
                document.Element(saveId).Elements("npc").ForEach(npcElement => {
                    NpcInfo info = new NpcInfo();
                    info.Load(npcElement);
                    npcs[info.RoomId] = info;
                });
                ValidateNpcDictionary();
                isLoaded = true;
            } else {
                InitSave();
            }
            return isLoaded;
        }

        public void InitSave() {
            npcs.Clear();
            ValidateNpcDictionary();
            isLoaded = true;
        }

        public void OnRegister() {
            Debug.Log("NpcService.OnRegister()");
        }

        public void OnLoaded() {
            Debug.Log("NpcService.OnLoaded()");
        } 
        #endregion
    }

    public class NpcInfo : RavenhillGameElement, ISaveElement {

        private string roomId;

        private NpcData data;

        public NpcInfo(string roomId = "", NpcData data = null) {
            this.roomId = roomId;
            this.data = data;
        }

        public NpcData Data {
            get {
                return data;
            }
        }

        public bool IsEmpty {
            get {
                return (Data == null);
            }
        }

        public string RoomId {
            get {
                return roomId;
            }
        }

        public void SetNpc(NpcData data) {
            this.data = data;
        }

        public void RemoveNpc() {
            this.data = null;
        }

        public UXMLWriteElement GetSave() {
            UXMLWriteElement element = new UXMLWriteElement("npc");
            element.AddAttribute("room_id", RoomId);
            element.AddAttribute("npc_id", IsEmpty ? string.Empty : Data.id);
            return element;
        }

        public void Load(UXMLElement element) {
            if(element == null ) {
                InitSave();
            } else {
                roomId = element.GetString("room_id");
                string npcId = element.GetString("npc_id");
                if(npcId.IsValid()) {
                    data = resourceService.GetNpc(npcId);
                } else {
                    InitSave();
                }
            }
        }

        public void InitSave() {
            data = null;
        }
    }
}
