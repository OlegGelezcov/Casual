using System.Linq;

namespace Casual.Ravenhill.UI {
    using Casual.Ravenhill.Net;
    using Casual.UI;

    public class FriendsView : RavenhillUIBehaviour {

        public SocialFriendListView friendListView;

        public void Setup() {
            friendListView.Clear();
            FriendCollection friends = engine.GetService<INetService>().FriendRequest.Friends;
            SocialFriendListView.ListViewData listData = new ListView<NetPlayer>.ListViewData {
                dataList = friends.Friends.Cast<NetPlayer>().ToList()
            };
            friendListView.Setup(listData);
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.FriendsUpdated += OnFriendsUpdated;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.FriendsUpdated -= OnFriendsUpdated;
        }

        private void OnFriendsUpdated(FriendCollection friends ) {
            Setup();
        }
    }
}
