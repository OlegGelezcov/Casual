using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.Ravenhill.Net {
    public class GiftsRequest : BaseRequest {

        private readonly IResourceService resourceService;

        public GiftsRequest(INetService netService, 
            string url, 
            INetErrorFactory errorFactory, IResourceService resourceService):
            base(netService, url, errorFactory) {
            this.resourceService = resourceService;
        }

       
        public void GetGifts() {
            GetGifts(netService.LocalPlayer, (result) => {
                netService.OnGiftsReceived(result);
            }, (error) => {
                netService.OnNetErrorOccured("get_gifts", error);
            });
        }

        public void SendGift(ISender receiver, InventoryItemData itemData ) {
            SendGift(netService.LocalPlayer, receiver, itemData, (gift) => {
                netService.OnGiftSended(gift);
            }, (error) => {
                netService.OnNetErrorOccured("send_gift", error);
            });
        }

        public void TakeGift(string giftId) {
            TakeGift(giftId, (gift) => {
                netService.OnGiftTaken(gift);
            }, (error) => {
                netService.OnNetErrorOccured("take_gift", error);
            });
        }


        private void GetGifts(ISender receiver, Action<Dictionary<string, NetGift>> onSuccess, Action<INetError> onError) {
            WWWForm form = new WWWForm();
            form.AddField("op", "get_gifts");
            form.AddField("receiver_id", receiver.GetId());

            MakeRequest(form, (json) => {
                Debug.Log(json.ToString());
                try {
                    Dictionary<string, object> gifts = MiniJSON.Json.Deserialize(json) as Dictionary<string, object>;
                    if(gifts == null ) {
                        onError?.Invoke(errorFactory.Create(NetErrorCode.json, string.Empty));
                    } else {
                        Dictionary<string, NetGift> result = new Dictionary<string, NetGift>();
                        foreach(var kvp in gifts) {
                            Dictionary<string, object> gDict = kvp.Value as Dictionary<string, object>;
                            if(gDict != null ) {
                                NetGift netGift = new NetGift(gDict, resourceService);
                                result[kvp.Key] = netGift;
                            }
                        }
                        onSuccess?.Invoke(result);
                    }
                } catch(Exception exception ) {
                    onError?.Invoke(errorFactory.Create(NetErrorCode.json, string.Empty));
                }
            }, onError);
        }

        private void SendGift(ISender sender, ISender receiver, InventoryItemData itemData, 
            Action<NetGift> onSuccess, Action<INetError> onError) {
            WWWForm form = new WWWForm();
            form.AddField("op", "send_gift");
            form.AddField("sender_id", sender.GetId());
            form.AddField("receiver_id", receiver.GetId());
            form.AddField("item_type", itemData.type.ToString());
            form.AddField("item_id", itemData.id);

            MakeRequest(form, (json) => {
                try {
                    Dictionary<string, object> dict = MiniJSON.Json.Deserialize(json) as Dictionary<string, object>;
                    if(dict != null ) {
                        NetGift netGift = new NetGift(dict, resourceService);
                        onSuccess?.Invoke(netGift);
                    } else {
                        onError?.Invoke(errorFactory.Create(NetErrorCode.json, string.Empty));
                    }
                } catch(Exception exception) {
                    onError?.Invoke(errorFactory.Create(NetErrorCode.json, string.Empty));
                }
            }, onError);
        }

        private void TakeGift(string giftId, Action<NetGift> onSuccess, Action<INetError> onError ) {
            WWWForm form = new WWWForm();
            form.AddField("op", "take_gift");
            form.AddField("gift_id", giftId);

            MakeRequest(form, (json) => {
                try {
                    Dictionary<string, object> dict = MiniJSON.Json.Deserialize(json) as Dictionary<string, object>;
                    if (dict != null) {
                        int status = dict.GetIntOrDefault("status");
                        if (status != 0) {
                            if(dict.ContainsKey("gift")) {
                                Dictionary<string, object> netGiftDict = dict["gift"] as Dictionary<string, object>;
                                if(netGiftDict != null ) {
                                    NetGift netGift = new NetGift(netGiftDict, resourceService);
                                    onSuccess?.Invoke(netGift);
                                }
                            }

                        } else {
                            onError?.Invoke(errorFactory.Create(NetErrorCode.take_gift_fail, string.Empty));
                        }
                    } else {
                        onError?.Invoke(errorFactory.Create(NetErrorCode.json, string.Empty));
                    }
                } catch(Exception exception) {
                    onError?.Invoke(errorFactory.Create(NetErrorCode.json, string.Empty));
                }
            }, onError);
        }
    }
}
