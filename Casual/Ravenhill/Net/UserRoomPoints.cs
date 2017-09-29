using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Net {
    public class UserRoomPoints {
        private INetUser user;
        private INetRoomPoints roomPoints;

        public UserRoomPoints(INetUser user, INetRoomPoints roomPoints ) {
            this.user = user;
            this.roomPoints = roomPoints;
        }

        public UserRoomPoints(Dictionary<string, object> dict) {
            this.user = new NetPlayer(dict, true);
            this.roomPoints = new NetRoomPoints(dict);
        }

        public INetUser User => user;
        public INetRoomPoints RoomPoints => roomPoints;

        public override string ToString() {
            return $"{User.ToString()}{Environment.NewLine}{RoomPoints.ToString()}";
        }
    }
}
