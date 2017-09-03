using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.UI {

    public interface IBankContext {
        ButtonState GoldButtonState { get; }
        ButtonState SilverButtonState { get; }
        ButtonState BestButtonState { get; }
    }
}
