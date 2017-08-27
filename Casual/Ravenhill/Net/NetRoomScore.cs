using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Net {
    public class NetRoomScore {
        public string roomId { get; private set; }
        public RoomMode roomMode { get; private set; }
        public SearchMode searchMode { get; private set; }
        public int score { get; private set; }
        public int rank { get; private set; }
        public NetPlayer player { get; private set; }

        public NetRoomScore(string roomId, RoomMode roomMode, SearchMode searchMode, int score, int rank, NetPlayer player ) {
            this.roomId = roomId;
            this.roomMode = roomMode;
            this.searchMode = searchMode;
            this.score = score;
            this.rank = rank;
            this.player = player;
        }
    }
}
