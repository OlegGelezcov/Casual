using UnityEngine;

namespace Casual
{
    public abstract class PositionObject {

        public abstract bool IsValid { get; }
        public abstract Vector3 Position { get; }

    }
}
