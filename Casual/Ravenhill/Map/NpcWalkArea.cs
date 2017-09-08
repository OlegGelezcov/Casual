using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill {
    public partial class NpcWalkArea  : RavenhillGameBehaviour {

        public string RoomId => roomId;


        public Vector3 NextPoint {
            get {
                Vector2 offset = UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(0.0f, 1.0f) * radius;
                return transform.localPosition + new Vector3(offset.x, offset.y, 0.0f);
            }
        }

        public void OnDrawGizmos() {
            if(drawGizmo) {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(transform.position, 10);

                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, radius);
            }
        }
    }

    public partial class NpcWalkArea : RavenhillGameBehaviour {

        [SerializeField]
        private float radius;

        [SerializeField]
        private string roomId;

        [SerializeField]
        private bool drawGizmo = true;

    }
}
