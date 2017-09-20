namespace Casual.Ravenhill.UI {
    using Casual.UI;
    using UnityEngine;
    using UnityEngine.UI;

    public partial class RoomModeSwitcherView : RavenhillBaseView {

        public override RavenhillViewType viewType => RavenhillViewType.room_mode_switcher_view;

        public override bool isModal => false;

        public override int siblingIndex => -4;

        private IRoomMode roomMode = null;

        private readonly UpdateTimer updateTimer = new UpdateTimer();

        public override void Setup(object objdata = null) {
            base.Setup(objdata);
            roomMode = objdata as IRoomMode;
            UpdateProgress();
            UpdateText();
            updateTimer.Setup(1, (delay) => {
                UpdateText();
                UpdateProgressValue();
            });

        }

        public override void Update() {
            base.Update();
            updateTimer.Update();
        }

        private void UpdateText() {
            timerText.text = Utility.FormatHMS(roomMode.Switcher.Timer);
        }

        private void UpdateProgressValue() {
            CurrentProgress.gameObject.GetOrAdd<ImageProgress>().SetValue((float)roomMode.Switcher.Timer / (float)roomMode.Switcher.Interval);
        }

        private void UpdateProgress() {
            if(roomMode.CurrentRoomMode == Data.RoomMode.normal) {
                normalProgressImage.ActivateSelf();
                scaryProgressImage.DeactivateSelf();
            } else {
                normalProgressImage.DeactivateSelf();
                scaryProgressImage.ActivateSelf();
            }
            UpdateProgressValue();
        }

        private Image CurrentProgress {
            get {
                if(roomMode.CurrentRoomMode == Data.RoomMode.normal) {
                    return normalProgressImage;
                } else {
                    return scaryProgressImage;
                }
            }
        }
    }

    public partial class RoomModeSwitcherView : RavenhillBaseView {

        [SerializeField]
        private Image normalProgressImage;

        [SerializeField]
        private Image scaryProgressImage;

        [SerializeField]
        private Text timerText;
    }
}
