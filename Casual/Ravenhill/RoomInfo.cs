using Casual.Ravenhill.Data;
using UnityEngine;

namespace Casual.Ravenhill {


    public class RoomInfo : GameElement, ISaveElement {

        private RoomData m_RoomData;
        

        public string id { get; private set; } = string.Empty;
        public bool isUnlocked { get; private set; } = false;
        public RoomLevel roomLevel { get; private set; } = RoomLevel.Beginner;
        public int roomProgress { get; private set; } = 0;
        public SearchMode searchMode { get; private set; } = SearchMode.Day;
        public int recordSearchTime { get; private set; } = int.MaxValue;
        private RoomSettingData m_RoomSettingData;

        public void RollSearchMode() {
            int value = UnityEngine.Random.Range(0, 10);
            SetSearchMode((value % 2 == 0) ? SearchMode.Day : SearchMode.Night);

        }

        public float progress {
            get {
                return Mathf.Clamp01((float)roomProgress / 100.0f);
            }
        }

        public RoomSettingData roomSetting {
            get {
                if ((m_RoomSettingData == null) || (m_RoomSettingData.roomLevel != roomLevel)) {
                    m_RoomSettingData = (engine.GetService<IResourceService>() as RavenhillResourceService).GetRoomSetting(roomLevel);
                }
                return m_RoomSettingData;
            }
        }

        public RoomData roomData {
            get {
                if(m_RoomData == null ) {
                    RavenhillResourceService resourceService = engine.GetService<IResourceService>() as RavenhillResourceService;
                    m_RoomData = resourceService.GetRoomData(id);
                }
                return m_RoomData;
            }
        }

        public RoomSettingData currentRoomSetting {
            get {
                if(m_RoomSettingData == null || (m_RoomSettingData.roomLevel != roomLevel)) {
                    m_RoomSettingData = engine.GetService<IResourceService>().Cast<RavenhillResourceService>().GetRoomSetting(roomLevel);
                }
                return m_RoomSettingData;
            }
        }

        public RoomInfo() { }

        public RoomInfo(string id) {
            this.id = id;

        }

        public static RoomInfo CreateNew(string id ) {
            return new RoomInfo(id);
        }

        public void Unlock(bool value) {
            bool oldIsUnlocked = isUnlocked;
            isUnlocked = value;
            if(isUnlocked != oldIsUnlocked ) {
                RavenhillEvents.OnRoomUnlocked(this);
            }
        }

        public bool TrySetRecordTime(int time ) {
            if (time < recordSearchTime) {
                int oldTime = recordSearchTime;
                recordSearchTime = time;
                RavenhillEvents.OnRoomRecordTimeChanged(oldTime, recordSearchTime, this);
                return true;
            }
            return false;
        }

        public void SetSearchMode(SearchMode searchMode) {
            SearchMode oldSearchMode = this.searchMode;
            this.searchMode = searchMode;
            if (oldSearchMode != searchMode) {
                RavenhillEvents.OnSearchModeChanged(oldSearchMode, searchMode, this);
            }
        }


        public void SetRoomLevel(RoomLevel newRoomLevel) {
            RoomLevel oldRoomLevel = roomLevel;
            roomLevel = newRoomLevel;
            if (oldRoomLevel != newRoomLevel) {
                RavenhillEvents.OnRoomLevelChanged(oldRoomLevel, roomLevel, this);
            }
        }

        private bool AddRoomProgress(int count) {
            bool isLevelChanged = false;
            if(count > 0 ) {
                int oldRoomProgress = roomProgress;
                roomProgress += count;
              
                if(roomProgress >= 100) {
                    if (roomLevel != RoomLevel.Hunter) {
                        roomProgress = 0;
                        isLevelChanged = true;
                    } else {
                        roomProgress = 100;
                    }
                }
                RavenhillEvents.OnRoomProgressChanged(oldRoomProgress, roomProgress, this);
            }
            return isLevelChanged;
        }

        public void AddProgress() {
            
            switch(roomLevel) {
                case RoomLevel.Beginner: {
                        if(AddRoomProgress(roomSetting.roomProgress)) {
                            SetRoomLevel(RoomLevel.Advanced);
                        }
                    }
                    break;
                case RoomLevel.Advanced: {
                        if (AddRoomProgress(roomSetting.roomProgress)) {
                            SetRoomLevel(RoomLevel.Detective);
                        }
                    }
                    break;
                case RoomLevel.Detective: {
                        if (AddRoomProgress(roomSetting.roomProgress)) {
                            SetRoomLevel(RoomLevel.Explorer);
                        }
                    }
                    break;
                case RoomLevel.Explorer: {
                        if (AddRoomProgress(roomSetting.roomProgress)) {
                            SetRoomLevel(RoomLevel.Hunter);
                        }
                    }
                    break;
                case RoomLevel.Hunter: {
                        AddRoomProgress(roomSetting.roomProgress);
                    }
                    break;
            }
        }
        #region ISaveElement

        public UXMLWriteElement GetSave() {
            UXMLWriteElement element = new UXMLWriteElement("room");
            element.AddAttribute("id", id);
            element.AddAttribute("is_unlocked", isUnlocked);
            element.AddAttribute("level", roomLevel.ToString());
            element.AddAttribute("progress", roomProgress);
            element.AddAttribute("search_mode", searchMode.ToString());
            element.AddAttribute("record_time", recordSearchTime.ToString());
            return element;
        }

        public void Load(UXMLElement element) {
            if(element == null ) { return; }
            id = element.GetString("id");
            isUnlocked = element.GetBool("is_unlocked");
            roomLevel = element.GetEnum("level", RoomLevel.Beginner);
            roomProgress = element.GetInt("progress", 0);
            searchMode = element.GetEnum("search_mode", SearchMode.Day);
            recordSearchTime = element.GetInt("record_time", int.MaxValue);
        } 

        public void InitSave() {
            id = string.Empty;
            isUnlocked = false;
            roomLevel = RoomLevel.Beginner;
            roomProgress = 0;
            searchMode = SearchMode.Day;
            recordSearchTime = int.MaxValue;
        }
        #endregion
    }
}
