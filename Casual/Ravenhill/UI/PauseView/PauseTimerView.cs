using System;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class PauseTimerView : RavenhillBaseView {
        public override RavenhillViewType viewType => RavenhillViewType.pause_timer_view;

        public override bool isModal => false;

        public override int siblingIndex => -10;

        private IPauseCounterSource data = null;
        public override void Setup(object objdata = null) {
            base.Setup(objdata);
            data = objdata as IPauseCounterSource;
            if(data == null ) {
                throw new ArgumentException("objdata");
            }
            timerText.text = data.GetPauseTimer().ToString();
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.SearchTimerPauseChanged += OnPausedChanged;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.SearchTimerPauseChanged -= OnPausedChanged;
        }

        public override void Update() {
            base.Update();
            if (data != null) {
                timerText.text = data.GetPauseTimer().ToString();
            }
        }

        private void OnPausedChanged(bool oldPause, bool newPaused, int interval ) {
            if(!newPaused ) {
                viewService.RemoveView(RavenhillViewType.pause_timer_view);
            }
        }
    }

    public partial class PauseTimerView : RavenhillBaseView {

        [SerializeField]
        private Text timerText;
    }

    public interface IPauseCounterSource {
        int GetPauseTimer();
    }
}
