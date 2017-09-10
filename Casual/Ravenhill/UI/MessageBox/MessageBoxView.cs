using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.UI {
    using UnityEngine;
    using UnityEngine.UI;

    public partial class MessageBoxView : RavenhillCloseableView {

        public override RavenhillViewType viewType => RavenhillViewType.message_box_view;

        public override bool isModal => true;

        public override int siblingIndex => 60;

        public class Data {

            public MessageBoxSetupType setupType {
                get {
                    if(buttonConfigs == null) {
                        throw new ArgumentException("buttonConfigs");
                    }
                    if(buttonConfigs.Length == 0 ) {
                        return MessageBoxSetupType.NoButton;
                    } else if(buttonConfigs.Length == 1 ) {
                        return MessageBoxSetupType.OneButton;
                    } else {
                        return MessageBoxSetupType.TwoButton;
                    }
                }
            }

            public string content;
            public ControlColor textPanColor;
            public ButtonConfig[] buttonConfigs;
        }

        private Data data = null;

        public override void Setup(object objdata = null) {
            base.Setup(objdata);
            data = objdata as Data;
            if(data == null ) {
                throw new ArgumentException("objdata");
            }

            descriptionText.text = data.content;
            textImages.Setup(data.textPanColor);

            CorrectButtonActions();

            switch(data.setupType) {
                case MessageBoxSetupType.NoButton: {
                        oneButtonCollection.DeactivateSelf();
                        twoButtonCollection.DeactivateSelf();
                    }
                    break;
                case MessageBoxSetupType.OneButton: {
                        oneButtonCollection.ActivateSelf();
                        oneButtonCollection.Setup(data.buttonConfigs);
                        twoButtonCollection.DeactivateSelf();
                    }
                    break;
                case MessageBoxSetupType.TwoButton: {
                        oneButtonCollection.DeactivateSelf();
                        twoButtonCollection.ActivateSelf();
                        twoButtonCollection.Setup(data.buttonConfigs);
                    }
                    break;
            }
        }

        private void CorrectButtonActions() {
            if(data != null) {
                if(data.buttonConfigs == null ) {
                    data.buttonConfigs = new ButtonConfig[] { };
                }
                foreach(var config in data.buttonConfigs ) {
                    var oldAction = config.action;
                    config.action = () => {
                        Close();
                        oldAction?.Invoke();
                    };
                }
            }
        }
    }

    public partial class MessageBoxView : RavenhillCloseableView {

        [SerializeField]
        private Text descriptionText;

        [SerializeField]
        private ColoredImageCollection textImages;

        [SerializeField]
        private ColoredActionButtonCollection oneButtonCollection;

        [SerializeField]
        private ColoredActionButtonCollection twoButtonCollection;
    }

    public enum MessageBoxSetupType {
        NoButton,
        OneButton,
        TwoButton
    }

}
