using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.UI {

    [RequireComponent(typeof(RectTransform))]
    public class Bezier2Movable : GameBehaviour {

        private Vector2 startPoint;
        private Vector2 midPoint;
        private Vector2 endPoint;
        private float speed;
        private Action endAction;

        private bool isStarted = false;
        private float t;
        private RectTransform rectTransform;
        private bool isChangeSize = false;
        private Vector2 targetSize = Vector2.zero;
        private Vector2 startSize = Vector2.zero;

        public void ChangeSize(Vector2 size) {
            startSize = GetComponent<RectTransform>().sizeDelta;
            targetSize = size;
            isChangeSize = true;
        }

        public void Move(Vector2 startPoint, Vector2 midPoint, Vector2 endPoint, float speed, Action endAction ) {
            this.startPoint = startPoint;
            this.midPoint = midPoint;
            this.endPoint = endPoint;
            this.speed = speed;
            this.endAction = endAction;
            this.t = 0;
            rectTransform = GetComponent<RectTransform>();
            isStarted = true;
        }

        public void Stop() {
            isStarted = false;
            isChangeSize = false;
        }

        public override void Update() {
            base.Update();
            if(isStarted) {
                t += Time.deltaTime * speed;
                float sourcet = t;
                t = Mathf.Clamp01(t);

                Vector2 currentPoint = Utility.Bezier2(t, startPoint, midPoint, endPoint);
                rectTransform.anchoredPosition = currentPoint;

                if(isChangeSize) {
                    Vector2 currentSize = Vector2.Lerp(startSize, targetSize, t);
                    rectTransform.sizeDelta = currentSize;
                }

                if(sourcet >= 1.0f ) {
                    isStarted = false;
                    endAction?.Invoke();
                }
            }
        }
    }

}
