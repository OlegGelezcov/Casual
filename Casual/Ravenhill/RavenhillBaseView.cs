using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill {
    public abstract class RavenhillBaseView : BaseView<RavenhillViewType> {

        public override void Setup(object objdata = null) {
            base.Setup(objdata);
            if (IsPlayShowRemoveSound && isModal) {
                engine.GetService<IAudioService>().PlaySound(SoundType.view_open, false);
            }
        }

        public override void OnViewWillBeClosed() {
            base.OnViewWillBeClosed();
            if (IsPlayShowRemoveSound && isModal) {
                engine.GetService<IAudioService>().PlaySound(SoundType.view_close, false);
            }
        }

        protected virtual bool IsPlayShowRemoveSound => true;
    }
}
