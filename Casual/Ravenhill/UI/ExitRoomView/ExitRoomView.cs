using Casual.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public class ExitRoomView : RavenhillCloseableView {

        [SerializeField]
        private Text m_RoomNameText;

        [SerializeField]
        private InventoryItemListView m_ListView;

        [SerializeField]
        private GameObject m_InventoryItemViewPrefab;

        [SerializeField]
        private Text m_SearchStatusText;

        [SerializeField]
        private GameObject m_ExpSilverRewardParent;

        [SerializeField]
        private NumericTextProgress m_ExpRewardCountText;

        [SerializeField]
        private NumericTextProgress m_SilverRewardCountText;

        [SerializeField]
        private MarketAdItemView m_MarketAdItemView;


        private Text roomNameText => m_RoomNameText;
        private InventoryItemListView listView => m_ListView;
        private GameObject inventoryItemViewPrefab => m_InventoryItemViewPrefab;
        private Text searchStatusText => m_SearchStatusText;
        private GameObject expSilverRewardParent => m_ExpSilverRewardParent;
        private NumericTextProgress expRewardCountText => m_ExpRewardCountText;
        private NumericTextProgress silverRewardCountText => m_SilverRewardCountText;
        private MarketAdItemView marketAdItemView => m_MarketAdItemView;

        public override bool isModal => true;

        public override int siblingIndex => 1;

        public override string listenerName => "exit_room_view";

        public override RavenhillViewType viewType => RavenhillViewType.exit_room_view;

        private SearchSession session { get; set; }

        public override void Setup(object data = null) {
            base.Setup(data);

            session = data as SearchSession;
            if(session == null ) {
                throw new ArgumentException($"ExitRoomView.Setup() required argument of type {typeof(SearchSession).Name}");
            }

            roomNameText.text = resourceService.GetString(session.roomData.nameId);
            //listView.Setup()
        }
    }
}
