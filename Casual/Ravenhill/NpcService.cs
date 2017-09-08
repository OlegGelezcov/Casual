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
        private readonly List<MapNpc> mapNpcs = new List<MapNpc>();

        public void Setup(object data) {
        }

        public override void Start() {
            base.Start();
            engine.GetService<ISaveService>().Register(this);
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.GameModeChanged += OnGameModeChanged;
            RavenhillEvents.ExitCurrentScene += OnExitCurrentScene;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.GameModeChanged -= OnGameModeChanged;
            RavenhillEvents.ExitCurrentScene -= OnExitCurrentScene;
        }

        private void OnGameModeChanged(GameModeName oldGameMode, GameModeName newGameMode ) {
            if(newGameMode == GameModeName.map || newGameMode == GameModeName.hallway ) {
                TryGenerateNPCs();
            }
        }

        private void OnExitCurrentScene() {
            if(ravenhillGameModeService.gameModeName == GameModeName.map ) {
                ClearMapNpcs();
            }
        }

        private void ClearMapNpcs() {
            mapNpcs.ForEach(obj => {
                if (obj != null && obj.gameObject != null) {
                    Destroy(obj.gameObject);
                }
            });
            mapNpcs.Clear();
        }

        private void CreateMapNpcs() {
            ClearMapNpcs();
            engine.Cast<RavenhillEngine>().Run(() => {
                GameObject prefab = resourceService.GetCachedPrefab("map_npc");

                Debug.Log("===========Before creating map npc============");
                Debug.Log(GetNpcsString());

                foreach (var pair in Npcs) {
                    if (!pair.Value.IsEmpty) {
                        GameObject instance = Instantiate<GameObject>(prefab);
                        MapNpc mapNpc = instance.GetComponent<MapNpc>();
                        mapNpc.Setup(pair.Value);
                        mapNpcs.Add(mapNpc);
                    }
                }
            }, () => isLoaded);
        }

        

      
        private string GetNpcsString() {
            System.Text.StringBuilder stringBuilder = new StringBuilder();
            foreach(var pair in Npcs ) {
                stringBuilder.AppendLine($"{pair.Key} => {pair.Value.ToString()}");
            }
            return stringBuilder.ToString();
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

            if(ravenhillGameModeService.gameModeName == GameModeName.map ) {
                CreateMapNpcs();
            }
        }

        private void RemoveMapNpc(string roomId ) {
            var target = mapNpcs.Find(npc => npc.RoomId == roomId);
            if(target != null && target.gameObject) {
                mapNpcs.Remove(target);
                Destroy(target.gameObject);
                target = null;
            }
        }

        public void RemoveNpc(string roomId ) {
            var dict = Npcs;
            if(dict.ContainsKey(roomId)) {
                if(!dict[roomId].IsEmpty) {
                    RemoveMapNpc(roomId);
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

        public bool HasNpc(string npcId, string roomId ) {
            foreach(var pair in Npcs ) {
                if(pair.Key == roomId) {
                    if(!pair.Value.IsEmpty ) {
                        if(pair.Value.NpcId == npcId ) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public NpcInfo GetNpc(string npcId, string roomId ) {
            foreach(var pair in Npcs ) {
                var info = pair.Value;
                if(info.RoomId == roomId && info.NpcId == npcId ) {
                    return info;
                }
            }
            return null;
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
            Debug.Log($"NPC SAVE: {root.ToString()}");
            return root.ToString();
        }

        public bool Load(string saveStr) {
            if(saveStr.IsValid()) {
                Debug.Log($"LOAD NPC from {saveStr}");

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

        public string NpcId => Data?.id ?? string.Empty;

        public override string ToString() {
            return $"{roomId}-{NpcId}";
        }

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
                    Debug.Log($"Loaded npc {roomId}: {data.id}");
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
