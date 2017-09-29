using Casual.Ravenhill.Data;

namespace Casual.Ravenhill.Net {
    public class NetRoomScore {

        private INetRoomPoints roomPoints;
        
        public int rank { get; private set; }
        public INetUser player { get; private set; }

        public NetRoomScore(INetRoomPoints roomPoints, int rank, INetUser player ) {
            this.rank = rank;
            this.player = player;
            this.roomPoints = roomPoints;
        }

        public INetRoomPoints RoomPoints => roomPoints;
    }
}
