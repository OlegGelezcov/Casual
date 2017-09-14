using Casual.Ravenhill.Data;

namespace Casual.Ravenhill.UI {
    public partial class ClockToolView : BaseToolView {

        private SearchTimerView timerView;

        protected override void OnUse(InventoryItemData data) {
            base.OnUse(data);
            TimerView.SetPause(40);
        }

        protected override bool IsValid(ToolData data) {
            return !TimerView.isPaused;
        }

        public override void Update() {
            base.Update();
            button.interactable = !TimerView.isPaused;
        }

        private SearchTimerView TimerView {
            get {
                if(!timerView) {
                    timerView = FindObjectOfType<SearchTimerView>();
                }
                return timerView;
            }
        }
    }
}
