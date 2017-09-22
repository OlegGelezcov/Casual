using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Net {
    public interface INetService : IService {
        NetRoomScore GetBestRoomScore(SearchSession session);
        NetRoomScore GetPlayerBestRoomScore(SearchSession session);
        NetRoomScore GetBestRoomScore(RoomInfo roomInfo);
        NetRoomScore GetPlayerBestRoomScore(RoomInfo roomInfo);
        int GetRank(SearchSession session);
        NetPlayer LocalPlayer { get; }
        bool IsLocalPlayer(ISender sender);
        void SendGift(IGift gift);
    }
}
