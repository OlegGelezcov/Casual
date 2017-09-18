using Casual.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class EnterRoomView : RavenhillCloseableView {

        public override RavenhillViewType viewType => RavenhillViewType.enter_room_view;

        public override bool isModal => true;

        public override int siblingIndex => 1;

        private RoomInfo roomInfo = null;

        public override void Setup(object data = null) {
            base.Setup(data);

            RoomInfo roomInfo = data as RoomInfo;
            this.roomInfo = roomInfo;

            if(roomInfo == null ) {
                throw new UnityException($"EnterRoomView: need setup parameter typeof RoomInfo");
            }

            roomNameText.text = resourceService.GetString(roomInfo.roomData.nameId);
            listView.Setup(new ListView<Data.InventoryItem>.ListViewData {
                 dataList = ravenhillGameModeService.FilterCollectableForRoom(roomInfo)
            });

            UpdateUnlockLevelText(roomInfo);
            UpdateButtons(roomInfo);

            marketAdView.Setup(resourceService.marketItems.RandomElement());
            roomInfoView.Setup(roomInfo);
            bestResultView.Setup(netService.GetBestRoomScore(roomInfo));
            myBestResultView.Setup(netService.GetPlayerBestRoomScore(roomInfo));
            closeBigButton.SetListener(Close);
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.RoomUnlocked += OnRoomUnlocked;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.RoomUnlocked -= OnRoomUnlocked;
        }

        private void UpdateUnlockLevelText(RoomInfo roomInfo) {
            if(roomInfo.isUnlocked) {
                unlockLevelText.text = string.Empty;
                unlockLevelText.color = Color.black;
            } else {
                int playerLevel = playerService.level;

                if(playerLevel >= roomInfo.roomData.level ) {
                    unlockLevelText.color = Color.green;                  
                } else {
                    unlockLevelText.color = Color.red;
                }
                unlockLevelText.text = string.Format(resourceService.GetString("req_level_fmt"), roomInfo.roomData.level);
            }
        }

        private void UpdateButtons(RoomInfo roomInfo ) {
            if(roomInfo.isUnlocked ) {
                enterButton.ActivateSelf();
                unlockButton.DeactivateSelf();
                resourceIcon.ActivateSelf();
                resourceCountText.ActivateSelf();
                resourceIcon.overrideSprite = resourceService.healthSprite.Sprite;

                resourceCountText.text = roomInfo.roomSetting.health.ToString();
            } else {
                enterButton.DeactivateSelf();
                int playerLevel = playerService.level;
                if(playerLevel >= roomInfo.roomData.level ) {
                    unlockButton.ActivateSelf();
                    resourceIcon.ActivateSelf();
                    resourceCountText.ActivateSelf();
                    resourceIcon.overrideSprite = resourceService.GetPriceSprite(roomInfo.roomData.price);
                    resourceCountText.text = roomInfo.roomData.price.price.ToString();
                } else {
                    unlockButton.DeactivateSelf();
                    resourceIcon.DeactivateSelf();
                    resourceCountText.DeactivateSelf();
                }
            }

            enterButton.SetListener(() => {
                int playerHealth = Mathf.FloorToInt(playerService.health);
                if(playerHealth >= roomInfo.roomSetting.health ) {
                    playerService.RemoveHealth(roomInfo.roomSetting.health);
                    Close();
                    engine.Cast<RavenhillEngine>().EnterSearchRoom(roomInfo);
                } else {
                    Debug.Log("Health is very low...");
                    viewService.ShowView(RavenhillViewType.store, InventoryTab.Foods);
                }
            });

            unlockButton.SetListener(() => {
                if(playerService.HasCoins(roomInfo.roomData.price)) {
                    playerService.RemoveCoins(roomInfo.roomData.price);
                    ravenhillGameModeService.roomManager.Unlock(roomInfo.roomData.id);
                } else {
                    Debug.Log("No coins for action".Colored(ColorType.yellow));
                    viewService.ShowView(RavenhillViewType.bank);
                }
            });

            closeBigButton.SetListener(() => Close());
        }

        private void OnRoomUnlocked(RoomInfo room ) {
            if (roomInfo != null) {
                UpdateUnlockLevelText(roomInfo);
                UpdateButtons(roomInfo);
            }
        }
    }

    public partial class EnterRoomView : RavenhillCloseableView {
        [SerializeField]
        private Text roomNameText;

        [SerializeField]
        private InventoryItemListView listView;

        [SerializeField]
        private Text unlockLevelText;

        [SerializeField]
        private MarketAdItemView marketAdView;

        [SerializeField]
        private RoomInfoView roomInfoView;

        [SerializeField]
        private NetRoomScoreView bestResultView;

        [SerializeField]
        private NetRoomScoreView myBestResultView;

        [SerializeField]
        private Button unlockButton;

        [SerializeField]
        private Button enterButton;

        [SerializeField]
        private Image resourceIcon;

        [SerializeField]
        private Text resourceCountText;

        [SerializeField]
        private Button closeBigButton;
    }
}
