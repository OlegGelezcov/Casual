using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.UI {
    public abstract class RavenhillUIBehaviour : RavenhillGameBehaviour {

        protected void ClearViews<T>(List<T> list) where T : RavenhillGameBehaviour {
            foreach(T obj in list) {
                if(obj) {
                    Destroy(obj.gameObject);
                }
            }
            list.Clear();
        }
    }
}
