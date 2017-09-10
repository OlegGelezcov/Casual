using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.Ravenhill.UI {
    public partial class ColoredActionButtonCollection : RavenhillUIBehaviour {

        public void Setup(ButtonConfig[] configs ) {

            ResetAllButtons();

            if(configs.Length > 0 ) {
                SetupConfig(firstButtons, configs[0]);
            }

            if(configs.Length > 1) {
                SetupConfig(secondButtons, configs[1]);
            }
        }

        private void SetupConfig(ButtonSetup[] buttons, ButtonConfig config) {
            foreach(var button in buttons ) {
                if(button.color == config.color ) {
                    if(button.actionButton != null ) {
                        button.actionButton.ActivateSelf();
                        button.actionButton.Setup(config.buttonName, config.action);
                        break;
                    }
                }
            }
        }

        private void ResetAllButtons() {
            
            foreach(var button in firstButtons.Concat(secondButtons) ) {
                if(button.actionButton != null ) {
                    button.actionButton.DeactivateSelf();
                }
            }
        }
    }

    public partial class ColoredActionButtonCollection : RavenhillUIBehaviour {

        [SerializeField]
        private ButtonSetup[] firstButtons;

        [SerializeField]
        private ButtonSetup[] secondButtons;

    }

    public class ButtonConfig {
        public ControlColor color;
        public System.Action action;
        public string buttonName;
    }
}
