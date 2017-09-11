using Casual.Ravenhill.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Casual.Ravenhill.Net {
    public class NetService : RavenhillGameBehaviour, INetService {

        public NetRoomScore GetBestRoomScore(SearchSession session) {
            return new NetRoomScore(session.roomId, ravenhillGameModeService.roomMode, session.searchMode, UnityEngine.Random.Range(100, 200), 1,
                new NetPlayer(System.Guid.NewGuid().ToString(), "Linux Torvalds", "Avatar2", UnityEngine.Random.Range(1, 20), true));
        }

        public NetRoomScore GetBestRoomScore(RoomInfo roomInfo ) {
            return new NetRoomScore(roomInfo.roomData.id, ravenhillGameModeService.roomMode, roomInfo.searchMode, UnityEngine.Random.Range(100, 200), 1,
                new NetPlayer(System.Guid.NewGuid().ToString(), "Linux Torvands", "Avatar3", UnityEngine.Random.Range(1, 20), true));
        }

        public NetRoomScore GetPlayerBestRoomScore(SearchSession session) {
            return new NetRoomScore(session.roomId, ravenhillGameModeService.roomMode, session.searchMode, UnityEngine.Random.Range(100, 200),
                UnityEngine.Random.Range(1, 100), new NetPlayer("MyPlayer", "Oleg", "Avatar3", 12, true));
        }

        public NetRoomScore GetPlayerBestRoomScore(RoomInfo roomInfo ) {
            return new NetRoomScore(roomInfo.roomData.id, ravenhillGameModeService.roomMode, roomInfo.searchMode, UnityEngine.Random.Range(100, 200),
                UnityEngine.Random.Range(1, 100), new NetPlayer("MyPlayer", "Oleg", "Avatar3", 12, true));
        }

        public int GetRank(SearchSession session) {
            return 23;
        }

        public void Setup(object data) {
            
        }

        public void ShareWishlist(List<InventoryItemData> items ) {
            Debug.Log($"Share items: {items.Count}");
        }

        public void Ask(InventoryItemData data) {
            Debug.Log($"ask item {data.id}");
        }
    }

    public interface INetService : IService {
        NetRoomScore GetBestRoomScore(SearchSession session);
        NetRoomScore GetPlayerBestRoomScore(SearchSession session);
        NetRoomScore GetBestRoomScore(RoomInfo roomInfo);
        NetRoomScore GetPlayerBestRoomScore(RoomInfo roomInfo);
        int GetRank(SearchSession session);
    }
}
