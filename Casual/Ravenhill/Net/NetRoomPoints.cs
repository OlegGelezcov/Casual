using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;

namespace Casual.Ravenhill.Net {

    public class NetRoom : INetRoom {
        private readonly string roomId;
        private readonly RoomMode roomMode;

        public NetRoom(string roomId, RoomMode roomMode) {
            this.roomId = roomId;
            this.roomMode = roomMode;
        }

        public NetRoom(Dictionary<string, object> dict) {
            if (dict.ContainsKey("room_id")) {
                roomId = dict["room_id"].ToString();
            }
            if (dict.ContainsKey("room_mode")) {
                string strRoomMode = dict["room_mode"].ToString();
                if (!Enum.TryParse<RoomMode>(strRoomMode, out roomMode)) {
                    roomMode = RoomMode.normal;
                }
            }
        }

        public string RoomId => roomId;

        public RoomMode RoomMode => roomMode;

        public string GetNetRoomId() {
            if(RoomMode == RoomMode.normal ) {
                return RoomId;
            } else {
                return RoomId + "s";
            }
        }
    }

    public class NetRoomPoints : NetRoom, INetRoomPoints {


        private readonly int points;

        public NetRoomPoints(string roomId, RoomMode roomMode, int points ) : 
            base(roomId, roomMode ) {
            this.points = points;
        }

        public NetRoomPoints(Dictionary<string, object> dict) : 
            base(dict) {
            if(dict.ContainsKey("points")) {
                if(!int.TryParse(dict["points"].ToString(), out points)) {
                    points = 0;
                }
            }
        }

        public int Points => points;

        public override string ToString() {
            return $"Room Id: {RoomId}, Room Mode: {RoomMode}, Points: {Points}";
        }

        public static NetRoomPoints Null => new NetRoomPoints(string.Empty, RoomMode.normal, 0);

        public bool IsNull => string.IsNullOrEmpty(RoomId) && (RoomMode == RoomMode.normal) && (Points == 0);

    }


}
