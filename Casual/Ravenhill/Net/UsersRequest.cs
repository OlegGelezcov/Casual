using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Net {
    using UnityEngine;
    using Casual.MiniJSON;

    public class UsersRequest : BaseRequest {

        private IResourceService resourceService;

        public UsersRequest(INetService netService, string url, INetErrorFactory errorFactory, IResourceService resourceService) : 
            base(netService, url, errorFactory) {
            this.resourceService = resourceService;
        }


        public void WriteUser(INetUser user, Dictionary<string, string> wishlist) {
            WriteUser(user, MiniJSON.Json.Serialize(wishlist), (netUser) => {
                netService.OnNetUserWritten(netUser);
            }, (error) => {
                netService.OnNetErrorOccured("write_user", error);
            });
        }

        public void WritePoints(UserRoomPoints userRoomPoints ) {
            WritePoints(userRoomPoints, (returnedPoints) => {
                netService.OnUserRoomPointsWritten(returnedPoints);
            }, (error) => {
                netService.OnNetErrorOccured("write_points", error);
            });
        }

        public void ReadPoints(INetUser user, INetRoom room) {
            ReadPoints(user, room, rank => {
                netService.OnRoomNetRankReaded(user, room, rank);
            }, (error) => {
                netService.OnNetErrorOccured("read_points", error);
            });}

        public void ReadAllRoomPoints(INetUser user, List<string> roomIds ) {
            ReadAllRoomPoints(user, roomIds, (ranks) => {
                netService.OnRoomNetRanksReceived(ranks);
            }, (error) => {
                netService.OnNetErrorOccured("read_points", error);
            });
        }

        public void ReadAllRoomPoints(List<string> roomIds ) {
            ReadAllRoomPoints(netService.LocalPlayer, roomIds);
        }

        private void WriteUser(INetUser user, string wishlist, System.Action<INetUser> onSuccess, System.Action<INetError> onError) {
            WWWForm form = new WWWForm();
            form.AddField("op", "write_user");
            form.AddField("user_id", user.id);
            form.AddField("user_name", user.name);
            form.AddField("avatar_id", user.avatarId);
            form.AddField("level", user.level);
            form.AddField("wishlist", wishlist);

            MakeRequest(form, (json) => {
                try {
                    Dictionary<string, object> userObj = Json.Deserialize(json) as Dictionary<string, object>;
                    if(userObj == null ) {
                        onError?.Invoke(errorFactory.Create(NetErrorCode.json, string.Empty));
                        return;
                    }
                    NetPlayer player = new NetPlayer(userObj, true, resourceService);
                    onSuccess?.Invoke(player);

                } catch(Exception exception) {
                    onError?.Invoke(errorFactory.Create(NetErrorCode.json, string.Empty));
                }
            }, onError);

        }

        private void WritePoints(UserRoomPoints userRoomPoints, System.Action<UserRoomPoints> onSuccess, System.Action<INetError> onError ) {
            WWWForm form = new WWWForm();
            form.AddField("op", "write_points");
            form.AddField("room_id", userRoomPoints.RoomPoints.RoomId);
            form.AddField("room_mode", userRoomPoints.RoomPoints.RoomMode.ToString());
            form.AddField("user_id", userRoomPoints.User.id);
            form.AddField("user_name", userRoomPoints.User.name);
            form.AddField("avatar_id", userRoomPoints.User.avatarId);
            form.AddField("level", userRoomPoints.User.level);
            form.AddField("points", userRoomPoints.RoomPoints.Points);

            MakeRequest(form, (json) => {
                try {
                    Dictionary<string, object> dict = Json.Deserialize(json) as Dictionary<string, object>;
                    if (dict == null) {
                        onError?.Invoke(errorFactory.Create(NetErrorCode.json, string.Empty));
                        return;
                    }
                    UserRoomPoints returnedPoints = new UserRoomPoints(dict, resourceService);
                    onSuccess?.Invoke(returnedPoints);
                } catch(Exception exception ) {
                    onError?.Invoke(errorFactory.Create(NetErrorCode.json, string.Empty));
                }
            }, onError);
        }

        private void ReadPoints(INetUser user, INetRoom room, System.Action<NetRoomPlayerRank> onSuccess, System.Action<INetError> onError ) {
            WWWForm form = new WWWForm();
            form.AddField("op", "read_points");
            form.AddField("room_id", room.RoomId);
            form.AddField("room_mode", room.RoomMode.ToString());
            form.AddField("user_id", user.id);
            form.AddField("user_name", user.name);
            form.AddField("avatar_id", user.avatarId);
            form.AddField("level", user.level);

            MakeRequest(form, json => {
                try {
                    Dictionary<string, object> dict = Json.Deserialize(json) as Dictionary<string, object>;
                    if (dict == null) {
                        onError?.Invoke(errorFactory.Create(NetErrorCode.json, string.Empty));
                        return;
                    }
                    NetRoomPlayerRank rank = new NetRoomPlayerRank(dict, resourceService);
                    onSuccess?.Invoke(rank);
                } catch(Exception exception ) {
                    onError?.Invoke(errorFactory.Create(NetErrorCode.json, string.Empty));
                }
            }, onError);
        }

        private void ReadAllRoomPoints(INetUser user, List<string> roomIds, Action<Dictionary<string, NetRoomPlayerRank>> onSuccess, Action<INetError> onError ) {
            WWWForm form = new WWWForm();
            form.AddField("op", "read_all_rooms_points");
            form.AddField("user_id", user.id);
            form.AddField("user_name", user.name);
            form.AddField("avatar_id", user.avatarId);
            form.AddField("level", user.level);
            form.AddField("rooms", roomIds.JoinToString(","));
            MakeRequest(form, (json) => {
                try {
                    Dictionary<string, NetRoomPlayerRank> ranks = ParseRoomRanks(json);
                    onSuccess?.Invoke(ranks);

                } catch(Exception exception ) {
                    onError?.Invoke(errorFactory.Create(NetErrorCode.json, string.Empty));
                }
            }, onError);
        }

        private Dictionary<string, NetRoomPlayerRank> ParseRoomRanks(string json) {
            Dictionary<string, NetRoomPlayerRank> ranks = new Dictionary<string, NetRoomPlayerRank>();
            Dictionary<string, object> rootDict = Json.Deserialize(json) as Dictionary<string, object>;
            if (rootDict == null) {
                throw new FormatException("json");
            }
            foreach(var kvp in rootDict ) {
                Dictionary<string, object> rankDict = kvp.Value as Dictionary<string, object>;
                if(rankDict != null ) {
                    NetRoomPlayerRank rank = new NetRoomPlayerRank(rankDict, resourceService);
                    ranks[kvp.Key] = rank;
                }
            }
            return ranks;
        }
    }

    
}
