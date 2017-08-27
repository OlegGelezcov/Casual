using UnityEngine;

namespace Casual.CasualDebug {
    public class GizmoObject : MonoBehaviour {

#pragma warning disable 0649
        [SerializeField]
        private Color m_Color = Color.green;

        [SerializeField]
        private float m_Radius;
#pragma warning restore 0649

        private Color color => m_Color;

        private float radius => m_Radius;

        private void OnDrawGizmos() {
            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, radius);
        }
    }
}
