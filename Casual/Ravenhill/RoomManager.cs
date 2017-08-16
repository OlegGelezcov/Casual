using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill {
    public class RoomManager : ISaveElement {

        public Dictionary<string, RoomInfo> rooms { get; } = new Dictionary<string, RoomInfo>();

        public RoomInfo GetRoomInfo(string id) {
            if(rooms.ContainsKey(id)) {
                return rooms[id];
            } else {
                var newRoom = RoomInfo.CreateNew(id);
                rooms.Add(id, newRoom);
                return newRoom;
            }
        }

        public UXMLWriteElement GetSave() {
            UXMLWriteElement element = new UXMLWriteElement("rooms");
            foreach(KeyValuePair<string, RoomInfo> room in rooms ) {
                element.Add(room.Value.GetSave());
            }
            return element;
        }

        public void Load(UXMLElement element) {
            rooms.Clear();
            foreach(UXMLElement roomElement in element.Elements("room")) {
                RoomInfo roomInfo = new RoomInfo();
                roomInfo.Load(roomElement);
                rooms[roomInfo.id] = roomInfo;
            }
        }

        public void InitSave() {
            rooms.Clear();
        }
    }
}
