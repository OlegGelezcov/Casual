using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual {
    public abstract class PositionObject {

        public abstract bool IsValid { get; }
        public abstract Vector3 Position { get; }

    }
}
