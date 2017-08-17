using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public abstract class RavenhillCloseableView : RavenhillBaseView {

        [SerializeField]
        private Button m_CloseButton;

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
