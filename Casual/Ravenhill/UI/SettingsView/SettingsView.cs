using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    public partial class SettingsView : RavenhillCloseableView {
        public override RavenhillViewType viewType => RavenhillViewType.settings_view;

        public override bool isModal => true;

        public override int siblingIndex => 2;

        public override void Setup(object objdata = null) {
            base.Setup(objdata);

            IAudioService service = engine.GetService<IAudioService>();
            musicToggle.isOn = service.IsMusicEnabled;
            soundToggle.isOn = service.IsSoundEnabled;

            musicToggle.SetListener((isOn) => {
                if(isOn) {
                    service.IsMusicEnabled = true;
                } else {
                    service.IsMusicEnabled = false;
                }
            }, engine.GetService<IAudioService>() );

            soundToggle.SetListener((isOn) => {
                if (isOn) {
                    service.IsSoundEnabled = true;
                } else {
                    service.IsSoundEnabled = false;
                }
            }, engine.GetService<IAudioService>());
        }
    }

    public partial class SettingsView : RavenhillCloseableView {

        [SerializeField]
        private Toggle musicToggle;

        [SerializeField]
        private Toggle soundToggle;
    }
}
