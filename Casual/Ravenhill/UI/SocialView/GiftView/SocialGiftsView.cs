using Casual.Ravenhill.Net;
using Casual.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.Ravenhill.UI {
    public partial class SocialGiftsView : RavenhillUIBehaviour {

        public void Setup() {
            listView.Clear();
            SocialGiftListView.ListViewData listData = new ListView<NetGift>.ListViewData {
                dataList = engine.GetService<INetService>().Gifts
            };
            listView.Setup(listData);
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.GiftsReceived += OnGiftsReceived;
            RavenhillEvents.GiftSendedSuccess += OnGiftSended;
            RavenhillEvents.GiftTaken += OnGiftTaken;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.GiftsReceived -= OnGiftsReceived;
            RavenhillEvents.GiftSendedSuccess -= OnGiftSended;
            RavenhillEvents.GiftTaken -= OnGiftTaken;
        }

        private void OnGiftsReceived(Dictionary<string, NetGift> gifts) {
            Setup();
        }

        private void OnGiftSended(IGift gift) {
            Setup();
        }

        private void OnGiftTaken(NetGift gift) {
            Setup();
        }
    }

    public partial class SocialGiftsView : RavenhillUIBehaviour {

        [SerializeField]
        private SocialGiftListView listView;


    }
}
