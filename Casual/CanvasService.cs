using Casual.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual {
    [RequireComponent(typeof(Canvas))]
    public class CanvasService : GameBehaviour, ICanvasSerive {

        [SerializeField]
        private FirstSiblingTransform m_FirstSiblingTransform;

        [SerializeField]
        private LastSiblingTransform m_LastSiblingTransform;

        private FirstSiblingTransform firstSiblingTransform => m_FirstSiblingTransform;
        private LastSiblingTransform lastSiblingTransform => m_LastSiblingTransform;

        public void Add(Transform view) {
            view.SetParent(transform, false);
        }

        public void AddToFirstGroup(Transform view) {
            if(firstSiblingTransform) {
                view.SetParent(firstSiblingTransform.transform, false);
            }
        }

        public void AddToLastGroup(Transform view) {
            if(lastSiblingTransform) {
                view.SetParent(lastSiblingTransform.transform, false);
            }
        }

        public void RestoreSiblings() {
            firstSiblingTransform?.transform?.SetAsFirstSibling();
            lastSiblingTransform?.transform?.SetAsLastSibling();
        }

        public void Setup(object data) { }
    }
}
