using System.Collections.Generic;

namespace Casual.Ravenhill.Net {
    public interface INetService : IService, ICoroutineExecutor {
        NetRoomScore GetBestRoomScore(INetRoom room);
        NetRoomScore GetPlayerBestRoomScore(INetRoom room);
        int GetLocalPlayerRank(SearchSession session);
        NetPlayer LocalPlayer { get; }
        bool IsLocalPlayer(ISender sender);
        void SendGift(IGift gift);
        void TakeGift(string giftId);
        List<NetGift> Gifts { get; }

        UsersRequest UsersRequest { get; }
        FriendRequest FriendRequest { get; }

        void OnNetUserWritten(INetUser user);
        void OnNetErrorOccured(string operation, INetError error);
        void OnUserRoomPointsWritten(UserRoomPoints roomPoints);
        void OnRoomNetRankReaded(INetUser user, INetRoom room, NetRoomPlayerRank rank);
        void OnRoomNetRanksReceived(Dictionary<string, NetRoomPlayerRank> ranks);

        void OnGiftsReceived(Dictionary<string, NetGift> gifts);
        void OnGiftSended(NetGift gift);
        void OnGiftTaken(NetGift gift);

    }

    public interface ICoroutineExecutor {
        void ExecuteCoroutine(System.Collections.IEnumerator coroutine);
    }

    
}
