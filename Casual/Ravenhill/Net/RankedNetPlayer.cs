using System;
using System.Collections.Generic;

namespace Casual.Ravenhill.Net {
    public class RankedRoomNetPlayer : INetUser, INullObject {

        private NetPlayer player;
        private int rank;
        private INetRoomPoints roomPoints;

        public RankedRoomNetPlayer(NetPlayer player, INetRoomPoints roomPoints) {
            this.player = player;
            this.rank = int.MaxValue;
            this.roomPoints = roomPoints;
        }

        public RankedRoomNetPlayer(Dictionary<string, object> dict ) {
            this.player = new NetPlayer(dict, true);
            this.roomPoints = new NetRoomPoints(dict);
            if (dict.ContainsKey("rank")) {
                if(!int.TryParse(dict["rank"].ToString(), out rank)) {
                    rank = int.MaxValue;
                }
            }
        }


        public string id => player.id;

        public string name => player.name;

        public string avatarId => player.avatarId;

        public int level => player.level;

        public int Rank => rank;

        public bool isValid => player.isValid;

        public INetRoomPoints RoomPoints => roomPoints;

        public override string ToString() {
            return $"Net Player: {player.ToString()}{Environment.NewLine}Room Points: {RoomPoints.ToString()}{Environment.NewLine}Rank: {Rank}";
        }

        public static RankedRoomNetPlayer Null => new RankedRoomNetPlayer(NetPlayer.Null, NetRoomPoints.Null);

        public bool IsNull => player.IsNull && RoomPoints.IsNull;
    }
}
