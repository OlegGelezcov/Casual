using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill {
    public class SearchSession : GameElement {
        public RoomInfo roomInfo { get; private set; }
        public SearchStatus searchStatus { get; private set; }
        public int searchTime { get; private set; }
        public bool isStarted { get; private set; }
        public List<InventoryItem> roomDropList { get; } = new List<InventoryItem>();


        public SearchSession() {
        }

        public void SetRoomInfo(RoomInfo roomInfo) {
            this.roomInfo = roomInfo;
        }

        private void SetSearchStatus(SearchStatus status) {
            searchStatus = status;
        }

        private void SetSearchTime(int time) {
            searchTime = time;
        }

        public void StartSession(RoomInfo roomInfo) {
            if (!isStarted) {
                SetRoomInfo(roomInfo);
                //roomDropList.Clear();
                isStarted = true;
                RavenhillEvents.OnSearchSessionStarted(this);
            }
        }

        public void EndSession(SearchStatus status, int time, List<InventoryItem> drops) {
            if (isStarted) {
                OnEndSession(status, time, drops);
            }
        }

        /// <summary>
        /// Dont call (only for debug)
        /// </summary>
        /// <param name="status"></param>
        /// <param name="time"></param>
        /// <param name="drops"></param>
        internal void OnEndSession(SearchStatus status, int time, List<InventoryItem> drops) {
            SetSearchStatus(status);
            SetSearchTime(time);
            roomDropList.Clear();
            roomDropList.AddRange(drops);
            isStarted = false;
            RavenhillEvents.OnSearchSessionEnded(this);
        }


        public bool isSearchSuccessed {
            get {
                return (searchStatus == SearchStatus.success);
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

        public SearchMode searchMode {
            get {
                return roomInfo.searchMode;
            }
        }

        public int currentScore {
            get {
                return 100;
            }
        }
    }

}
