using UnityEngine;

namespace Casual {
    public class StaticPositionObject : PositionObject {

        private readonly Vector3 position;

        public StaticPositionObject(Vector3 position ) {
            this.position = position;
        }

        public override bool IsValid => true;

        public override Vector3 Position => position;
    }
}
