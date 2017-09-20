using Casual.Ravenhill.Data;
using UnityEngine;

namespace Casual.Ravenhill {
    public class RoomModeSwitcher : RavenhillGameElement, ISaveElement, IRoomModeSwitcher {

        private float switchTimer = 0.0f;
        

        private readonly float switchInterval = 0.0f;
        private readonly IRoomMode roomMode = null;

        private readonly UpdateTimer eventTimer;

        public int Timer => Mathf.RoundToInt(switchTimer);

        public int Interval => Mathf.RoundToInt(switchInterval);

        public RoomModeSwitcher(float switchInterval, IRoomMode roomMode) {
            this.switchInterval = switchInterval;
            this.switchTimer = switchInterval;
            this.roomMode = roomMode;
            eventTimer = new UpdateTimer();
            eventTimer.Setup(1.0f, (realDelay) => {
                RavenhillEvents.OnRoomModeSwitchTimerChanged(switchTimer, switchInterval);
            });
        }

        public void Update() {
            switchTimer -= Time.deltaTime;
            if(switchTimer <= 0.0f ) {
                switchTimer += switchInterval;
                roomMode.SwitchRoomMode();
            }
            eventTimer.Update();
        }

        #region ISaveElement
        public UXMLWriteElement GetSave() {
            UXMLWriteElement root = new UXMLWriteElement("room_mode_switcher");
            root.AddAttribute("timer", switchTimer);
            root.AddAttribute("save_time", Utility.unixTime);
            return root;
        }

        public void InitSave() {
            switchTimer = switchInterval;
        }

        public void Load(UXMLElement element) {
            switchTimer = element.GetFloat("timer");
            int saveTime = element.GetInt("save_time");
            int nowTime = Utility.unixTime;
            int sleepInterval = nowTime - saveTime;
            switchTimer -= sleepInterval;
            bool needSwitchRoomMode = false;
            while(switchTimer < 0.0f ) {
                switchTimer += switchInterval;
                needSwitchRoomMode = !needSwitchRoomMode;
            }
            if(needSwitchRoomMode) {
                roomMode.SwitchRoomMode();
            }
        } 
        #endregion
    }

    public interface IRoomMode {
        void SwitchRoomMode();
        IRoomModeSwitcher Switcher { get; }
        RoomMode CurrentRoomMode { get; }
    }

    public interface IRoomModeSwitcher {
        int Timer { get; }
        int Interval { get; }
    }
}
