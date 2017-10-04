using System;
using System.Collections.Generic;
using UnityEngine;

namespace Casual.Ravenhill.Net {
    public class FriendRequest : BaseRequest {

        private readonly FriendCollection friends = new FriendCollection();
        private IResourceService resourceService;
        private bool isFriendsRequested = false;


        public FriendRequest(INetService netService, string url, INetErrorFactory errorFactory, IResourceService resourceService) : 
            base(netService, url, errorFactory) {
            this.resourceService = resourceService;
        }

        public void OnGameModeChanged(GameModeName newGameMode ) {
            if(HelperUtils.IsHallwayGamemode(newGameMode)) {
                if(!isFriendsRequested) {
                    if (Utility.IsNetworkReachable) {
                        GetFriends();
                        isFriendsRequested = true;
                    }
                }
            }
        }


        public FriendCollection Friends => friends;

        public void GetFriends() {
            GetFriends(netService.LocalPlayer, (newfriends) => {
                this.friends.Replace(newfriends);
                RavenhillEvents.OnFriendsUpdated(friends);
            }, (error) => {
                netService.OnNetErrorOccured("get_friends", error);
            });
        }

        public void AddFriend(INetUser targetUser) {
            AddFriend(netService.LocalPlayer, targetUser, (newfriends) => {
                this.friends.Replace(newfriends);
                RavenhillEvents.OnFriendsUpdated(friends);
            }, (error) => {
                netService.OnNetErrorOccured("add_friend", error);
            });
        }

        public void RemoveFriend(INetUser targetUser ) {
            RemoveFriend(netService.LocalPlayer, targetUser, (newfriends) => {
                this.friends.Replace(newfriends);
                RavenhillEvents.OnFriendsUpdated(friends);
            }, (error) => {
                netService.OnNetErrorOccured("remove_friend", error);
            });
        }

        private void GetFriends(INetUser user, Action<FriendCollection> onSuccess, Action<INetError> onError) {
            WWWForm form = new WWWForm();
            form.AddField("op", "get_friends");
            form.AddField("user_id", user.id);

            MakeRequest(form, (json) => {
                try {
                    Dictionary<string, object> dict = MiniJSON.Json.Deserialize(json) as Dictionary<string, object>;
                    if(dict == null ) {
                        onError?.Invoke(errorFactory.Create(NetErrorCode.json, string.Empty));
                    } else {
                        FriendCollection friendCollection = new FriendCollection(dict, resourceService);
                        onSuccess?.Invoke(friendCollection);
                    }
                } catch(Exception exception ) {
                    onError?.Invoke(errorFactory.Create(NetErrorCode.json, string.Empty));
                }
            }, onError);
        }

        private void AddFriend(INetUser user, INetUser targetUser, Action<FriendCollection> onSuccess, Action<INetError> onError ) {
            WWWForm form = new WWWForm();
            form.AddField("op", "add_friend");
            form.AddField("user_id", user.id);
            form.AddField("target_user_id", targetUser.id);
            MakeRequest(form, json => {
                Dictionary<string, object> dict = MiniJSON.Json.Deserialize(json) as Dictionary<string, object>;
                if (dict == null) {
                    onError?.Invoke(errorFactory.Create(NetErrorCode.json, string.Empty));
                } else {
                    FriendCollection friendCollection = new FriendCollection(dict, resourceService);
                    onSuccess?.Invoke(friendCollection);
                }
            }, onError);
        }

        private void RemoveFriend(INetUser user, INetUser targetUser, Action<FriendCollection> onSuccess, Action<INetError> onError) {
            WWWForm form = new WWWForm();
            form.AddField("op", "add_friend");
            form.AddField("user_id", user.id);
            form.AddField("target_user_id", targetUser.id);
            MakeRequest(form, json => {
                Dictionary<string, object> dict = MiniJSON.Json.Deserialize(json) as Dictionary<string, object>;
                if (dict == null) {
                    onError?.Invoke(errorFactory.Create(NetErrorCode.json, string.Empty));
                } else {
                    FriendCollection friendCollection = new FriendCollection(dict, resourceService);
                    onSuccess?.Invoke(friendCollection);
                }
            }, onError);
        }
    }
}
