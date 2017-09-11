using Casual.Ravenhill.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Casual.Ravenhill {

    public class BuffManager : RavenhillGameElement, ISaveElement{

        private readonly Dictionary<string, BuffInfo> buffs = new Dictionary<string, BuffInfo>();
        private int lostFocusInterval = 0;
        private bool isLoaded = false;

        private readonly UpdateTimer updateBuffsTimer = new UpdateTimer();
        private readonly UpdateTimer removeExpiredTimer = new UpdateTimer();

        public BuffManager() {
            updateBuffsTimer.Setup(1, (delta) => {
                foreach (var pair in buffs) {
                    pair.Value.Update(delta);
                }
            });
            removeExpiredTimer.Setup(1, (delta) => {
                List<string> keys = new List<string>();
                foreach(var pair in buffs) {
                    if(!pair.Value.IsValid) {
                        keys.Add(pair.Key);
                    }
                }

                foreach(string key in keys ) {
                    BuffInfo buff = buffs[key];
                    buffs.Remove(key);
                    //buff removed event
                    RavenhillEvents.OnBuffRemoved(buff);
                }
            });
        }

        public void OnApplicationFocus(bool focus) {
            if(focus) {
                lostFocusInterval = engine.Cast<RavenhillEngine>().LostFocusInterval;
            }
        }

        public void Update() {
            updateBuffsTimer.Update();
            removeExpiredTimer.Update();


            if(isLoaded && lostFocusInterval > 0 ) {
                foreach(var pair in buffs) {
                    pair.Value.RemoveTime(lostFocusInterval);
                }
                Debug.Log($"remove lost focus interval from buffs: {lostFocusInterval}");
                lostFocusInterval = 0;
            }
        }


        public bool HasBuff(string id) {
            return buffs.ContainsKey(id);
        }

        public float GetValue(string id) {
            BuffInfo buff = buffs.GetOrDefault(id);
            if(buff != null ) {
                return buff.Value;
            }
            return 0.0f;
        }

        public void AddBuff(BonusData data) {
            BuffInfo info = new BuffInfo(data);
            buffs[info.Id] = info;
            RavenhillEvents.OnBuffAdded(info);
        }

        public List<BuffInfo> BuffList {
            get {
                List<BuffInfo> list = new List<BuffInfo>(buffs.Values);
                list = list.OrderBy(b => b.RemainTime).ToList();
                return list;
            }
        }

        public UXMLWriteElement GetSave() {
            UXMLWriteElement root = new UXMLWriteElement("buffs");
            foreach(var pair in buffs ) {
                root.Add(pair.Value.GetSave());
            }
            return root;
        }

        public void InitSave() {
            buffs.Clear();
            isLoaded = true;
        }

        public void Load(UXMLElement element) {
            buffs.Clear();
            foreach(UXMLElement buffElement in element.Elements("buff")) {
                BuffInfo buff = new BuffInfo();
                buff.Load(buffElement);
                if(buff.IsValid) {
                    buffs.Add(buff.Id, buff);
                }
            }
            isLoaded = true;
        }
    }

    public class BuffInfo : RavenhillGameElement, ISaveElement {
        
        public string Id { get; private set; }
        public float RemainTime { get; private set; }

        private BonusData data = null;

        public BuffInfo() { }

        public BuffInfo(BonusData data) {
            this.data = data;
            Id = data.id;
            RemainTime = data.interval;
        }

        public BuffInfo(string id, float remainTime) {
            Id = id;
            RemainTime = remainTime;
            data = resourceService.GetBonus(Id);
        }

        public void RemoveTime(float dt) {
            RemainTime -= dt;
        }

        public BonusData Data {
            get {
                if(data == null ) {
                    data = resourceService.GetBonus(Id);
                }
                return data;
            }
        }

        public float Value {
            get {
                return Data?.value ?? 0.0f;
            }
        }

        public bool IsValid {
            get {
                return (Data != null) && (RemainTime > 0.0f);
            }
        }

        public void Update(float deltaTime) {
            RemoveTime(deltaTime);
        }

        public UXMLWriteElement GetSave() {
            UXMLWriteElement element = new UXMLWriteElement("buff");
            element.AddAttribute("id", Id);
            element.AddAttribute("time", RemainTime);
            return element;
        }

        public void Load(UXMLElement element) {
            Id = element.GetString("id");
            RemainTime = element.GetFloat("time");
            data = resourceService.GetBonus(Id);         
        }

        public void InitSave() {
            if(Data != null ) {
                RemainTime = Data.interval;
            }
        }
    }
}
