using UnityEngine;

namespace Casual {
    public class TransformPositionObject : PositionObject {

        private readonly Transform transform;

        public TransformPositionObject(Transform transform) {
            this.transform = transform;
        }

        public override bool IsValid => transform;

        public override Vector3 Position {
            get {
                if(IsValid) {
                    return transform.position;
                }
                return Vector3.zero;
            }
        }
    }
}
