using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual {
    public enum MovingFuncType : byte {
        /// <summary>
        /// Linear moving (lerp)
        /// </summary>
        Bezier1,
        /// <summary>
        /// Bezier 2-degree moving
        /// </summary>
        Bezier2,
        /// <summary>
        /// Bezier 3-degree moving
        /// </summary>
        Bezier3
    }
}
