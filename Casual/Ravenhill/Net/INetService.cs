using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Net {
    public interface INetService : IService, ICoroutineExecutor {
        NetRoomScore GetBestRoomScore(INetRoom room);
        NetRoomScore GetPlayerBestRoomScore(INetRoom room);
        int GetLocalPlayerRank(SearchSession session);
        NetPlayer LocalPlayer { get; }
        bool IsLocalPlayer(ISender sender);
        void SendGift(IGift gift);

        UsersRequest UsersRequest { get; }
        void OnNetUserWritten(INetUser user);
        void OnNetErrorOccured(string operation, INetError error);
        void OnUserRoomPointsWritten(UserRoomPoints roomPoints);
        void OnRoomNetRankReaded(INetUser user, INetRoom room, NetRoomPlayerRank rank);
        void OnRoomNetRanksReceived(Dictionary<string, NetRoomPlayerRank> ranks);
    }

    public interface ICoroutineExecutor {
        void ExecuteCoroutine(System.Collections.IEnumerator coroutine);
    }
}
