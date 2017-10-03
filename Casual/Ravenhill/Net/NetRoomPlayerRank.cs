using System.Collections.Generic;

namespace Casual.Ravenhill.Net {
    public class NetRoomPlayerRank : INullObject {
        private RankedRoomNetPlayer maxPlayer;
        private RankedRoomNetPlayer myPlayer;

        public NetRoomPlayerRank(Dictionary<string, object> dict, IResourceService resourceService) {

            if (dict == null || dict.Count == 0) {
                maxPlayer = RankedRoomNetPlayer.Null;
                myPlayer = RankedRoomNetPlayer.Null;

            } else {

                if (dict.ContainsKey("max_user")) {
                    Dictionary<string, object> maxDict = dict["max_user"] as Dictionary<string, object>;
                    if (maxDict != null) {
                        maxPlayer = new RankedRoomNetPlayer(maxDict, resourceService);
                    } else {
                        maxPlayer = RankedRoomNetPlayer.Null;
                    }
                }

                if (dict.ContainsKey("my_user")) {
                    Dictionary<string, object> myDict = dict["my_user"] as Dictionary<string, object>;
                    if (myDict != null) {
                        myPlayer = new RankedRoomNetPlayer(myDict, resourceService);
                    } else {
                        myPlayer = RankedRoomNetPlayer.Null;
                    }
                }
            }
        }

        public RankedRoomNetPlayer MaxPlayer => maxPlayer;

        public RankedRoomNetPlayer MyPlayer => myPlayer;

        public NetRoomPlayerRank(RankedRoomNetPlayer myPlayer, RankedRoomNetPlayer maxPlayer ) {
            this.myPlayer = myPlayer;
            this.maxPlayer = maxPlayer;
        }

        public bool IsNull => maxPlayer.IsNull && myPlayer.IsNull;

        public static NetRoomPlayerRank Null => new NetRoomPlayerRank(RankedRoomNetPlayer.Null, RankedRoomNetPlayer.Null);
    }
}
