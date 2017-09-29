using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Net {
    public interface INetRoomPoints : INetRoom, INullObject{

        int Points { get; }
    }

    public interface INetRoom  {
        string RoomId { get; }
        RoomMode RoomMode { get; }

        string GetNetRoomId();
    }

    public interface INullObject {
        bool IsNull { get; }
    }
}
