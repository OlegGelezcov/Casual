using Casual.Ravenhill.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Casual.Ravenhill {
    public class AchievmentService : RavenhillGameBehaviour, IAchievmentService, ISaveable {

        private readonly Dictionary<string, AchievmentInfo> achievments = new Dictionary<string, AchievmentInfo>();

        public void Setup(object data) {

        }

        public AchievmentInfo GetAchievment(AchievmentData data) {
            if(HasAchievment(data)) {
                return achievments[data.id];
            } else {
                achievments.Add(data.id, new AchievmentInfo(data));
                return achievments[data.id];
            }
        }

        public AchievmentInfo GetAchievment(string id) {
            return GetAchievment(resourceService.GetAchievment(id));
        }

        public bool HasAchievment(AchievmentData data) {
            return achievments.ContainsKey(data.id);
        }

        public bool IsTierUnlocked(AchievmentData data, int tier) {
            return GetAchievment(data).IsUnlocked(tier);
        }

        public string saveId => "achievments";

        public bool isLoaded { get; private set; } = false;

        public string GetSave() {
            UXMLWriteElement root = new UXMLWriteElement(saveId);
            foreach(var a in resourceService.achievmentList ) {
                var info = GetAchievment(a);
                root.Add(info.GetSave());
            }
            return root.ToString();
        }

        public void InitSave() {
            achievments.Clear();
            foreach(var a in resourceService.achievmentList) {
                var info = GetAchievment(a);
            }
            isLoaded = true;
        }

        public bool Load(string saveStr) {
            if (saveStr.IsValid()) {
                UXMLDocument document = new UXMLDocument();
                document.Parse(saveStr);
                achievments.Clear();
                var root = document.Element(saveId);
                foreach (UXMLElement element in root.Elements("achievment")) {
                    AchievmentInfo info = new AchievmentInfo(element);
                    achievments.Add(info.Id, info);
                }
                foreach (AchievmentData data in resourceService.achievmentList) {
                    AchievmentInfo dumpInfo = GetAchievment(data);
                }
                isLoaded = true;
            } else {
                InitSave();
            }
            return isLoaded;
        }

        public void OnLoaded() {
            Debug.Log($"AchievmentService is loaded".Colored(ColorType.aqua));
        }

        public void OnRegister() {
            Debug.Log("AchievmentService is registered for save".Colored(ColorType.black));
        }


    }
}
