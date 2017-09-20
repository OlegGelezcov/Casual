using Casual.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public class RoomInfoView : RavenhillUIBehaviour {

#pragma warning disable 0649
        [SerializeField]
        private Image m_RoomIconImage;

        [SerializeField]
        private Text m_RoomLevelText;

        [SerializeField]
        private ImageProgress m_RoomLevelProgress;
#pragma warning restore 0649



        private Image roomIconImage => m_RoomIconImage;
        private Text roomLevelText => m_RoomLevelText;
        private ImageProgress roomLevelProgress => m_RoomLevelProgress;

        public void Setup(RoomInfo roomInfo) {

            roomIconImage.overrideSprite = resourceService.GetSprite(
                roomInfo.roomData.GetIconKey(ravenhillGameModeService.CurrentRoomMode), 
                roomInfo.roomData.GetIcon(ravenhillGameModeService.CurrentRoomMode)
                );
            roomLevelText.text = resourceService.GetRoomLevelName(roomInfo.roomLevel);
            roomLevelProgress.SetValue(0.0f, roomInfo.progress);
        }
    }
}
