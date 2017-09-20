using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class LoadingPerspective :  MonoBehaviour {

        public void Start() {
            targetImage.overrideSprite = UnityEngine.Random.Range(1, 100) % 2 == 0 ? normalBackSprite : scaryBackSprite;
        }
    }

    public partial class LoadingPerspective : MonoBehaviour {

        [SerializeField]
        private Sprite normalBackSprite;

        [SerializeField]
        private Sprite scaryBackSprite;

        [SerializeField]
        private Image targetImage;
    }
}
