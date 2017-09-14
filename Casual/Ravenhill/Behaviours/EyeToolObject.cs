using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill  {
    using Casual.Ravenhill.Behaviours;
    using UnityEngine;
    using UnityEngine.UI;

    public partial class EyeToolObject : RavenhillGameBehaviour  {

        private MagnifyingGlass magGlassComponent = null;

        public void SetTarget(Transform target ) {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

            gameObject.GetOrAdd<TransformBezierMovable>().Move(new TransformBezierMovable.Data {
                movingType = MovingFuncType.Bezier3,
                speed = speed,
                positions = new PositionObject[] {
                       new StaticPositionObject(transform.position),
                       ComputeMid(1.0f / 3.0f, target),
                       ComputeMid(2.0f / 3.0f, target ),
                       new StaticPositionObject(targetPosition)
                   },
                onComplete = () => {
                    GetComponentInChildren<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
                    magGlassComponent = Camera.main.gameObject.GetOrAdd<MagnifyingGlass>();
                    magGlassComponent.Setup(magGlassMaterial, 0.3f, new Vector2(0.1f, 0.2f), new Vector2(0.3f, 1.0f), targetPosition);
                    StartCoroutine(CorWait());
                }
            });
        }

        private PositionObject ComputeMid(float lenRatio, Transform target) {
            Vector3 direction = new Vector3(target.position.x, target.position.y, transform.position.z) - transform.position;
            float len = direction.magnitude;
            float len_Scaled = lenRatio * len;
            Vector2 offset_1 = UnityEngine.Random.insideUnitCircle * len_Scaled;
            return new StaticPositionObject(transform.position + new Vector3(offset_1.x, offset_1.y, 0));
        }

        private System.Collections.IEnumerator CorWait() {
            yield return new WaitForSeconds(2.4f);
            if(magGlassComponent ) {
                magGlassComponent.RemoveSelf();
                magGlassComponent = null;
            }
            Destroy(gameObject);
        }
    }

    public partial class EyeToolObject : RavenhillGameBehaviour {

        [SerializeField]
        private Material magGlassMaterial;

        [SerializeField]
        private float speed = 1.0f;


    }

}
