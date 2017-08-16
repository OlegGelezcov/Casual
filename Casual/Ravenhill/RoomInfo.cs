using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill {
    public class RoomInfo : GameElement, ISaveElement {

        private RoomData m_RoomData;
        private RoomSettingData m_RoomSettingData;

        public string id { get; private set; } = string.Empty;
        public RoomLevel roomLevel { get; private set; } = RoomLevel.Beginner;
        public int roomProgress { get; private set; } = 0;
        public SearchMode searchMode { get; private set; } = SearchMode.Day;
        public int recordSearchTime { get; private set; } = int.MaxValue;
        public bool isUnlocked { get; private set; } = false;





        public RoomData roomData {
            get {
                if(m_RoomData == null ) {
                    RavenhillResourceService resourceService = engine.GetService<IResourceService>() as RavenhillResourceService;
                    m_RoomData = resourceService.GetRoomData(id);
                }
                return m_RoomData;
            }
        }


        public RoomSettingData roomSetting {
            get {
                if((m_RoomSettingData == null) || (m_RoomSettingData.roomLevel != roomLevel)) {
                    m_RoomSettingData = (engine.GetService<IResourceService>() as RavenhillResourceService).GetRoomSetting(roomLevel);
                }
                return m_RoomSettingData;
            }
        }

        public RoomInfo() { }

        public RoomInfo(string id, RoomLevel roomLevel, int roomProgress, SearchMode searchMode, int recordSearchTime ) {
            this.id = id;
            this.roomLevel = roomLevel;
            this.searchMode = searchMode;
            this.recordSearchTime = recordSearchTime;
        }

        public static RoomInfo CreateNew(string id ) {
            return new RoomInfo(id, RoomLevel.Beginner, 0, SearchMode.Day, int.MaxValue);
        }

        public void Unlock(bool value) {
            bool oldIsUnlocked = isUnlocked;
            isUnlocked = value;
            if(isUnlocked != oldIsUnlocked ) {
                engine.GetService<IEventService>().SendEvent(new RoomUnlockedEventArgs(this));
            }
        }

        public bool TrySetRecordTime(int time ) {
            if(time < recordSearchTime ) {
                int oldTime = recordSearchTime;
                recordSearchTime = time;
                engine.GetService<IEventService>().SendEvent(new RoomRecordTimeChangedEventArgs(oldTime, recordSearchTime, this));
                return true;
            }
            return false;
        }

        private void SetSearchMode(SearchMode searchMode) {
            SearchMode oldSearchMode = this.searchMode;
            this.searchMode = searchMode;
            if(oldSearchMode != searchMode ) {
                engine.GetService<IEventService>().SendEvent(new SearchModeChangedEventArgs(oldSearchMode, searchMode, this));
            }
        }

        private void SetRoomLevel(RoomLevel newRoomLevel) {
            RoomLevel oldRoomLevel = this.roomLevel;
            this.roomLevel = newRoomLevel;
            if(oldRoomLevel != newRoomLevel) {
                engine.GetService<IEventService>().SendEvent(new RoomLevelChangedEventArgs(oldRoomLevel, this.roomLevel, this));
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
                engine.GetService<IEventService>().SendEvent(new RoomProgressChangedEventArgs(oldRoomProgress, this.roomProgress, this));
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
                        if(AddRoomProgress(roomSetting.roomProgress)) {
                            SetRoomLevel(RoomLevel.Detective);
                        }
                    }
                    break;
                case RoomLevel.Detective: {
                        if(AddRoomProgress(roomSetting.roomProgress)) {
                            SetRoomLevel(RoomLevel.Explorer);
                        }
                    }
                    break;
                case RoomLevel.Explorer: {
                        if(AddRoomProgress(roomSetting.roomProgress)) {
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
            element.AddAttribute("room_level", roomLevel.ToString());
            element.AddAttribute("room_progress", roomProgress);
            element.AddAttribute("search_mode", searchMode.ToString());
            element.AddAttribute("record_time", recordSearchTime);
            element.AddAttribute("is_unlocked", isUnlocked);
            return element;
        }

        public void Load(UXMLElement element) {
            if(element == null ) { return; }
            id = element.GetString("id");
            roomLevel = element.GetEnum<RoomLevel>("room_level", RoomLevel.Beginner);
            roomProgress = element.GetInt("room_progress", 0);
            searchMode = element.GetEnum<SearchMode>("search_mode");
            recordSearchTime = element.GetInt("record_time", int.MaxValue);
            isUnlocked = element.GetBool("is_unlocked");
        } 

        public void InitSave() {
            id = string.Empty;
            roomLevel = RoomLevel.Beginner;
            roomProgress = 0;
            searchMode = SearchMode.Day;
            recordSearchTime = int.MaxValue;
            isUnlocked = false;
        }
        #endregion
    }
}
