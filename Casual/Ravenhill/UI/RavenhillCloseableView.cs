using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public abstract class RavenhillCloseableView : RavenhillBaseView {

#pragma warning disable 0649
        [SerializeField]
        private Button m_CloseButton;
#pragma warning restore 0649

        private Button closeButton => m_CloseButton;

        protected virtual void OnClose() { }

        protected virtual void Close() {
            OnClose();
            viewService.RemoveView(viewType);
        }

        public override void Setup(object data = null) {
            base.Setup(data);
            closeButton?.SetListener(Close);
        }
    }
}
