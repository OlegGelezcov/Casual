using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public class UILocalizator : RavenhillGameBehaviour {

#pragma warning disable 0649
        [SerializeField]
        private string m_StringId;
#pragma warning restore 0649

        private string stringId => m_StringId;

        private bool isLocalized { get; set; } = false;

        public override void Start() {
            base.Start();
            Localize();
        }

        public override void OnEnable() {
            base.OnEnable();
            Localize();
        }

        public override void OnDisable() {
            base.OnDisable();
        }

        private void Localize() {
            if(!isLocalized) {
                Text target = GetComponentInChildren<Text>();
                if(target != null ) {
                    target.text = resourceService.GetString(stringId);
                }
                isLocalized = true;
            }
        }
    }
}
