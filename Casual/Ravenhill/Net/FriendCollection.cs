using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Net {
    public class FriendCollection {
        private string id;
        private readonly List<INetUser> friends;

        public FriendCollection() {
            id = string.Empty;
            friends = new List<INetUser>();
        }

        public FriendCollection(Dictionary<string, object> dict, IResourceService resourceService) {
            id = dict.GetStringOrDefault("user_id");
            friends = new List<INetUser>();
            
            if(dict.ContainsKey("friends")) {
                Dictionary<string, object> dictFriends = dict["friends"] as Dictionary<string, object>;
                if(dictFriends != null ) {
                    foreach(var kvp in dictFriends) {
                        Dictionary<string, object> friend = kvp.Value as Dictionary<string, object>;
                        if(friend != null ) {
                            NetPlayer netPlayer = new NetPlayer(friend, true, resourceService);
                            if(!netPlayer.IsNull) {
                                friends.Add(netPlayer);
                            }
                        }
                    }
                }
            }

            this.friends.Sort((f1, f2) => { return f1.id.CompareTo(f2.id); });
        }

        public string Id => id;
        public List<INetUser> Friends => friends;
        public int Count => Friends.Count;

        public void Replace(FriendCollection other) {
            this.id = other.id;
            this.friends.Clear();
            foreach(var f in other.friends ) {
                this.friends.Add(f);
            }
        }
    }

}
