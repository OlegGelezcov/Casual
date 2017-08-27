using Casual.Ravenhill.Data;
using Casual.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public class ExitRoomView : RavenhillCloseableView {

#pragma warning disable 0649
        [SerializeField]
        private Text m_RoomNameText;

        [SerializeField]
        private InventoryItemListView m_ListView;

        [SerializeField]
        private GameObject m_NotFoundAnyItemsInfoObject;

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

        [SerializeField]
        private RoomInfoView m_RoomInfoView;

        [SerializeField]
        private NetRoomScoreView m_BestRoomScore;

        [SerializeField]
        private NetRoomScoreView m_MyBestRoomScore;

        [SerializeField]
        private CurrentScoreView m_CurrentScoreView;

        [SerializeField]
        private Text m_TimeText;

        [SerializeField]
        private Button m_CloseBigButton;
#pragma warning restore 0649



        private Text roomNameText => m_RoomNameText;
        private InventoryItemListView listView => m_ListView;
        private Text searchStatusText => m_SearchStatusText;
        private GameObject expSilverRewardParent => m_ExpSilverRewardParent;
        private NumericTextProgress expRewardCountText => m_ExpRewardCountText;
        private NumericTextProgress silverRewardCountText => m_SilverRewardCountText;
        private MarketAdItemView marketAdItemView => m_MarketAdItemView;
        private GameObject notFoundAnyItemsInfoObject => m_NotFoundAnyItemsInfoObject;
        private RoomInfoView roomInfoView => m_RoomInfoView;
        private NetRoomScoreView bestRoomScore => m_BestRoomScore;
        private NetRoomScoreView myBestRoomScore => m_MyBestRoomScore;
        private CurrentScoreView currentScoreView => m_CurrentScoreView;
        private Text timeText => m_TimeText;
        private Button closeBigButton => m_CloseBigButton;

        public override bool isModal => true;

        public override int siblingIndex => 1;

        

        public override RavenhillViewType viewType => RavenhillViewType.exit_room_view;

        private SearchSession session { get; set; }

        public override void Setup(object data = null) {
            base.Setup(data);

            session = data as SearchSession;
            if(session == null ) {
                throw new ArgumentException($"ExitRoomView.Setup() required argument of type {typeof(SearchSession).Name}");
            }

            roomNameText.text = resourceService.GetString(session.roomData.nameId);


            Debug.Log($"session drop list {session.roomDropList.Count}".Colored(ColorType.yellow));

            InventoryItemListView.ListViewData listViewData = new InventoryItemListView.ListViewData {
                dataList = session.roomDropList
            };
            listView.Setup(listViewData);

            if(session.roomDropList.Count == 0 ) {
                notFoundAnyItemsInfoObject?.ActivateSelf();
            } else {
                notFoundAnyItemsInfoObject?.DeactivateSelf();
            }

            if(session.searchStatus == SearchStatus.success) {
                searchStatusText.text = resourceService.GetString("status_success");
            } else {
                searchStatusText.text = resourceService.GetString("status_fail");
            }

            if(session.searchStatus == SearchStatus.success ) {
                expSilverRewardParent.ActivateSelf();
                expRewardCountText.SetValue(0, session.roomData.expReward);
                silverRewardCountText.SetValue(0, session.roomData.silverReward);
            } else {
                expSilverRewardParent.DeactivateSelf();
            }

            marketAdItemView.Setup(resourceService.marketItems.RandomElement());

            roomInfoView.Setup(session.roomInfo);

            bestRoomScore.Setup(netService.GetBestRoomScore(session));
            myBestRoomScore.Setup(netService.GetPlayerBestRoomScore(session));
            currentScoreView.Setup(session);

            timeText.text = Utility.FormatMS(session.searchTime);

            closeBigButton.SetListener(Close);
        }

        protected override void OnClose() {
            base.OnClose();
            ravenhillGameModeService.ExitSessionRoom();
        }
    }
}
