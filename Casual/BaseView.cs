using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual {
    public abstract class BaseView<T> : Ravenhill.RavenhillGameBehaviour {

#pragma warning disable 0649
        [SerializeField]
        private Animator m_Animator;

        [SerializeField]
        private float m_DestroyTimeout = 0.5f;
#pragma warning restore 0649

        protected Animator animator {
            get => m_Animator;
        }

        protected float destroyTimeout { get => m_DestroyTimeout; }

        public abstract T viewType { get; }
        public abstract bool isModal { get; }
        public abstract int siblingIndex { get; }

        public virtual void Setup(object objdata = null) { }
        public virtual void OnViewWillBeClosed() { }

        public virtual void FadeOut(bool destroy = true ) {
            if(animator) {
                animator.SetTrigger("fadeout");
                Destroy(gameObject, destroyTimeout);
            } else {
                Destroy(gameObject);
            }
        }
    }
}
