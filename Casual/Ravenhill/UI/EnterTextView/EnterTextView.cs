using System;

namespace Casual.Ravenhill.UI {
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public partial class EnterTextView : RavenhillCloseableView {

        public override RavenhillViewType viewType => RavenhillViewType.enter_text_view;

        public override bool isModal => true;

        public override int siblingIndex => 2;

        public class Data {
            public string title = string.Empty;
            public string startInputText = string.Empty;
            public Func<string, bool> OnCheck = (str) => false;
            public Action<string> OnSubmit = (str) => { };
        }

        private Data data = null;
        private string targetText = string.Empty;

        public override void Setup(object objdata = null) {
            base.Setup(objdata);
            data = objdata as Data;
            if(data == null ) {
                throw new ArgumentException(nameof(objdata));
            }

            titleText.text = data.title;

            UpdateSubmit();
            submitButton.SetListener(() => {
                data.OnSubmit(input.text);
                Close();
            });
            input.Select();
            input.OnPointerClick(new PointerEventData(EventSystem.current));

            input.text = data.startInputText;

            input.characterValidation = InputField.CharacterValidation.Alphanumeric;
            input.characterLimit = 10;
            input.onValueChanged.RemoveAllListeners();
            input.onValidateInput = (text, index, ch) => {
                if(char.IsLetterOrDigit(ch) || ch == '_') {
                    return ch;
                }
                return '\0';
            };

        }

        public override void Update() {
            base.Update();
            UpdateSubmit();
        }

        private void UpdateSubmit() {
            if(data != null  ) {
                submitButton.interactable = data.OnCheck(input.text);
            } else {
                submitButton.interactable = false;
            }
        }
    }

    public partial class EnterTextView : RavenhillCloseableView {

        [SerializeField]
        private Text titleText;

        [SerializeField]
        private InputField input;

        [SerializeField]
        private Button submitButton;

    }
}
