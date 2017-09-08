using System;

namespace Casual.Ravenhill.UI {
    using UnityEngine;

    public partial class LoaderView : RavenhillBaseView {

        public class Data {
            public System.Action action;
            public float delay;
        }

        private Data data = null;

        public override RavenhillViewType viewType => RavenhillViewType.loader_view;

        public override bool isModal => true;

        public override int siblingIndex => 200;

        public override void Setup(object inData = null) {
            base.Setup(data);
            data = inData as Data;
            //StopAllCoroutines();

            if(data == null ) {
                throw new ArgumentException();
            }

            StartCoroutine(CorProcess());
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.SceneLoaded += OnSceneLoaded;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.SceneLoaded -= OnSceneLoaded;
        }

        private System.Collections.IEnumerator CorProcess() {
            yield return new WaitForSeconds(data.delay);
            data.action?.Invoke();
        }

        private void OnSceneLoaded(string sceneName) {
            StartCoroutine(CorStay());
        }

        private System.Collections.IEnumerator CorStay() {
            yield return new WaitForSeconds(stayDelay);
            viewService.RemoveView(RavenhillViewType.loader_view);
        }
    }

    public partial class LoaderView : RavenhillBaseView {

        [SerializeField]
        private float stayDelay = 1.0f;
    }
}
