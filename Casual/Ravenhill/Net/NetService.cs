using Casual.Ravenhill.Data;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Text;

namespace Casual.Ravenhill.Net {
    public class NetService : RavenhillGameBehaviour, INetService, ISaveable {

        private NetPlayer localPlayer = null;
        private readonly UpdateTimer localPlayerUpdater = new UpdateTimer();
        private UsersRequest usersRequest;
        private GiftsRequest giftsRequest;
        private FriendRequest friendRequest;

        private readonly Dictionary<string, NetRoomPlayerRank> ranks = new Dictionary<string, NetRoomPlayerRank>();
        private readonly Dictionary<string, NetGift> gifts = new Dictionary<string, NetGift>();

        private bool isRanksRequested = false;
        private bool isPlayerWrited = false;
        private bool isGiftsRequested = false;
        


        private void UpdateRanks(Dictionary<string, NetRoomPlayerRank> newRanks ) {
            foreach(var kvp in newRanks ) {
                ranks[kvp.Key] = kvp.Value;
            }
        }

        private void UpdateRanks(INetRoom room, NetRoomPlayerRank rank ) {
            ranks[room.GetNetRoomId()] = rank;
        }

        public void Setup(object data) {
            INetErrorFactory errorFactory = new NetErrorFactory();
            usersRequest = new UsersRequest(this, "https://server.playnebula.com/raven/users.php", errorFactory, resourceService);
            giftsRequest = new GiftsRequest(this, "https://server.playnebula.com/raven/gifts.php", errorFactory, resourceService);
            friendRequest = new FriendRequest(this, "https://server.playnebula.com/raven/friends.php", errorFactory, resourceService);
        }

        public UsersRequest UsersRequest => usersRequest;
        public GiftsRequest GiftsRequest => giftsRequest;
        public FriendRequest FriendRequest => friendRequest;

        public List<NetGift> Gifts {
            get {
                List<NetGift> result = new List<NetGift>(gifts.Values);
                result = result.OrderBy(g => g.Id).ToList();
                return result;
            }
        }

        public void OnRoomNetRanksReceived(Dictionary<string, NetRoomPlayerRank> ranks ) {
            RavenhillEvents.OnRoomNetRanksReceived(ranks);
            Debug.Log(("ALL ROOM RANKS RECEIVED: " + (new RoomNetRankFormatter(ranks).ToString())).Colored(ColorType.yellow));
            UpdateRanks(ranks);
        }

        public void OnRoomNetRankReaded(INetUser user, INetRoom room, NetRoomPlayerRank rank) {
            RavenhillEvents.OnRoomNetRankReaded(user, room, rank);
            Debug.Log($"Room Rank Readed: {rank.ToString()}");
            UpdateRanks(room, rank);
        }

        public void OnUserRoomPointsWritten(UserRoomPoints roomPoints) {
            RavenhillEvents.OnUserRoomPointsWritten(roomPoints);
            Debug.Log($"User Room Points Written: {roomPoints.ToString()}");
        }

        public void OnNetUserWritten(INetUser user) {
            RavenhillEvents.OnNetUserWritten(user);
            Debug.Log("User written: " + user.ToString());
        }

        public void OnNetErrorOccured(string operation, INetError error) {
            RavenhillEvents.OnNetErrorOccured(operation, error);
            Debug.Log($"Error: {operation} => {error.ToString()}".Colored(ColorType.red));
        }

        #region Gifts API
        public void OnGiftsReceived(Dictionary<string, NetGift> receivedGifts) {
            isGiftsRequested = true;
            gifts.Clear();
            foreach(var kvp in receivedGifts) {
                gifts.Add(kvp.Key, kvp.Value);
            }
            RavenhillEvents.OnGiftsReceived(gifts);
        }

        public void OnGiftSended(NetGift gift) {
            var itemData = gift.GetItemData();
            if (itemData != null) {
                if (playerService.GetItemCount(itemData) > 0) {
                    playerService.RemoveItem(itemData, 1);
                    RavenhillEvents.OnGiftSendedSuccess(gift);
                }
            }
        }

        public void OnGiftTaken(NetGift gift ) {
            if(gifts.ContainsKey(gift.Id)) {
                gifts.Remove(gift.Id);
                Debug.Log($"Gift removed: {gifts.Count}");
                if(gift.GetItemData() != null ) {
                    DropItem dropItem = new DropItem(DropType.item, 1, gift.GetItemData());
                    engine.DropItems(new List<DropItem> { dropItem });
                }
            }
            RavenhillEvents.OnGiftTaken(gift);
        }
        #endregion

        private void RequestRanks() {
            if(!isRanksRequested) {
                isRanksRequested = true;
                UsersRequest.ReadAllRoomPoints(
                    resourceService.
                    roomList.
                    Where(r => r.roomType == RoomType.search).
                    Select(r => r.id).
                    ToList());
            }
        }

        public override void Start() {
            base.Start();
            engine.GetService<ISaveService>().Register(this);
            localPlayerUpdater.Setup(5, (delay) => {
                UpdateLocalPlayer();
            });
        }

        public override void Update() {
            base.Update();
            localPlayerUpdater.Update();
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.PlayerNameChanged += OnPlayerNameChanged;
            RavenhillEvents.PlayerAvatarChanged += OnPlayerAvatarChanged;
            RavenhillEvents.PlayerLevelChanged += OnPlayerLevelChanged;
            RavenhillEvents.SearchSessionEnded += OnSearchSessionEnded;
            RavenhillEvents.GameModeChanged += OnGameModeChanged;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.PlayerNameChanged -= OnPlayerNameChanged;
            RavenhillEvents.PlayerAvatarChanged -= OnPlayerAvatarChanged;
            RavenhillEvents.PlayerLevelChanged -= OnPlayerLevelChanged;
            RavenhillEvents.SearchSessionEnded -= OnSearchSessionEnded;
            RavenhillEvents.GameModeChanged -= OnGameModeChanged;
        }

        private void OnGameModeChanged(GameModeName oldGameMode, GameModeName newGameMode ) {
            if(HelperUtils.IsHallwayGamemode(newGameMode) ) {
                RequestRanks();
                WritePlayer();
                GetGifts();
            }
            FriendRequest?.OnGameModeChanged(newGameMode);
        }

        private void WritePlayer() {
            if(!isPlayerWrited) {
                isPlayerWrited = true;
                UsersRequest.WriteUser(LocalPlayer, engine.GetService<IPlayerService>().wishlist.JsonCompatibale);
            }
        }

        private void GetGifts() {
            if(!isGiftsRequested) {
                GiftsRequest.GetGifts();
            }
        }

        private void OnSearchSessionEnded(SearchSession session ) {
            if(session.searchStatus == SearchStatus.success ) {
                UsersRequest.WritePoints(new UserRoomPoints(LocalPlayer, new NetRoomPoints(session.roomId, ravenhillGameModeService.roomMode, session.Points)));
            }
        }

        private void OnPlayerNameChanged(string oldName, string newName) {
            UpdateLocalPlayer();
            if (isLoaded) {
                UsersRequest.WriteUser(LocalPlayer, engine.GetService<IPlayerService>().wishlist.JsonCompatibale);
            }
        }

        private void OnPlayerAvatarChanged(string oldAvatar, string newAvatar ) {
            UpdateLocalPlayer();
            if (isLoaded) {
                UsersRequest.WriteUser(LocalPlayer, engine.GetService<IPlayerService>().wishlist.JsonCompatibale);
            }
        }

        private void OnPlayerLevelChanged(int oldLevel, int newLevel ) {
            UpdateLocalPlayer();
            if (isLoaded) {
                UsersRequest.WriteUser(LocalPlayer, engine.GetService<IPlayerService>().wishlist.JsonCompatibale);
            }
        }

        private void UpdateLocalPlayer() {
            if (isLoaded) {
                LocalPlayer.SetName(playerService.PlayerName);
                LocalPlayer.SetAvatar(playerService.avatarId);
                LocalPlayer.SetLevel(playerService.level);
            }
        }

        public NetRoomScore GetBestRoomScore(INetRoom room) {
            var rank = GetRank(room);
            return new NetRoomScore(rank.MaxPlayer.RoomPoints, rank.MaxPlayer.Rank, rank.MaxPlayer);
        }

        public NetRoomScore GetPlayerBestRoomScore(INetRoom room) {
            var rank = GetRank(room);
            return new NetRoomScore(rank.MyPlayer.RoomPoints, rank.MyPlayer.Rank, rank.MyPlayer);
        }


        public NetRoomPlayerRank GetRank(INetRoom room ) {
            if(!ranks.ContainsKey(room.GetNetRoomId())) {
                ranks.Add(room.GetNetRoomId(), new NetRoomPlayerRank(null, resourceService));
            } 
            return ranks[room.GetNetRoomId()];
        }

        public bool HasRank(INetRoom room ) {
            var rank = GetRank(room);
            return (!rank.IsNull);
        }

        public int GetLocalPlayerRank(SearchSession session) {
            return GetRank(new NetRoom(session.roomId, session.RoomMode)).MyPlayer.Rank;
        }



        public void ShareWishlist(List<InventoryItemData> items ) {
            Debug.Log($"Share items: {items.Count}");
            var chatService = engine.GetService<IChatService>();
            if(chatService.IsCanChat && items.Count > 0 ) {
                List<IAttachment> attachments = items.Select(it => new ChatAttachment(it)).Cast<IAttachment>().ToList();
                chatService.SendMessage(LocalPlayer, "Here is my wishlist, send me items< please", attachments, MessageType.Normal);
            }
        }

        public void Ask(InventoryItemData data) {
            Debug.Log($"ask item {data.id}");
        }

        public NetPlayer LocalPlayer {
            get {
                if(localPlayer == null || !localPlayer.id.IsValid() ) {
                    localPlayer = new NetPlayer(id: System.Guid.NewGuid().ToString(),
                        name: engine.GetService<IPlayerService>().PlayerName,
                        avatarId: engine.GetService<IPlayerService>().avatarId,
                        level: engine.GetService<IPlayerService>().level,
                        valid: true);
                }
                return localPlayer;
            }
        }

        public string saveId => "net_service";

        public bool isLoaded { get; private set; }

        public string GetSave() {
            UXMLWriteElement root = new UXMLWriteElement(saveId);
            root.Add(LocalPlayer.GetSave());
            return root.ToString();
        }

        public bool Load(string saveStr) {
            UXMLDocument document = UXMLDocument.FromXml(saveStr);
            UXMLElement root = document.Element(saveId);
            if(root != null ) {
                UXMLElement localPlayerELement = root.Element("local_player");
                if(localPlayerELement != null ) {
                    localPlayer = new NetPlayer();
                    localPlayer.Load(localPlayerELement);
                } else {
                    localPlayer = new NetPlayer();
                    localPlayer.InitSave();
                }
            }
            isLoaded = true;
            return isLoaded;
        }

        public void InitSave() {
            if(localPlayer != null ) {
                localPlayer.InitSave();
            }
            isLoaded = true;
        }

        public void OnRegister() {
            
        }

        public void OnLoaded() {
            
        }

        public bool IsLocalPlayer(ISender sender) {
            return (LocalPlayer.id == sender.GetId());
        }

        public void SendGift(IGift gift) {

            if(playerService.GetItemCount(gift.GetItemData()) > 0 ) {
                
                //really gift send here
                GiftsRequest.SendGift(gift.GetReceiver(), gift.GetItemData());
            }
        }

        public void TakeGift(string giftId ) {
            GiftsRequest.TakeGift(giftId);
        }

        public void ExecuteCoroutine(IEnumerator coroutine) {
            StartCoroutine(coroutine);
        }

        private class RoomNetRankFormatter {
            private Dictionary<string, NetRoomPlayerRank> ranks;

            public RoomNetRankFormatter(Dictionary<string, NetRoomPlayerRank> ranks ) {
                this.ranks = ranks;
            }

            public override string ToString() {
                StringBuilder builder = new StringBuilder();
                List<string> rooms = new List<string>(ranks.Keys);
                rooms.Sort();
                foreach(string room in rooms ) {
                    builder.Append($"Room: {room}{Environment.NewLine}\t\t{ranks[room].ToString()}{Environment.NewLine}");
                }
                return builder.ToString();
            }
        }
    }

    
}
