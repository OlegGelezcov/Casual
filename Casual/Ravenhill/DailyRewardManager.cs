using System;
using UnityEngine;

namespace Casual.Ravenhill {
    public class DailyRewardManager : RavenhillGameElement, ISaveElement  {

        private const float CHECK_INTERVAL = 10.0f;

        private int lastRewardTime = 0;
        private int lastDay = 0;
        private bool isLoaded = false;
        private float checkTimer = CHECK_INTERVAL;

        public UXMLWriteElement GetSave() {
            UXMLWriteElement root = new UXMLWriteElement("daily_reward");
            root.AddAttribute("last_reward_time", lastRewardTime);
            root.AddAttribute("last_day", lastDay);
            return root;
        }

        public void InitSave() {
            lastRewardTime = 0;
            lastDay = 0;
            isLoaded = true;
        }

        public void Load(UXMLElement element) {
            if (element != null) {
                lastRewardTime = element.GetInt("last_reward_time");
                lastDay = element.GetInt("last_day");
                isLoaded = true;
            } else {
                InitSave();
            }
        }

        private void CheckDaily() {
            int interval = Utility.unixTime - lastRewardTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(interval);
            if(1.0f <= timeSpan.TotalDays && timeSpan.TotalDays < 2.0f ) {
                lastRewardTime = Utility.unixTime;
                lastDay += 1;
                if(lastDay > 5) {
                    lastDay = 1;
                }
                ShowView(lastDay);
            } else if(timeSpan.TotalDays >= 2.0f ) {
                lastDay = 1;
                lastRewardTime = Utility.unixTime;
                ShowView(lastDay);
            }
        }

        private void ShowView(int day) {
            viewService.ShowViewWithCondition(RavenhillViewType.daily_reward_view, () => {
                return (gameModeService.gameModeName == GameModeName.hallway || gameModeService.gameModeName == GameModeName.map) &&
                    viewService.noModals;
            }, day);
        }

        public void Start() {

        }

        public void Update() {
            checkTimer -= Time.deltaTime;
            if(checkTimer <= 0.0f ) {
                checkTimer += CHECK_INTERVAL;
                CheckDaily();
            }
        }
    }
}
