using Casual.Ravenhill.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class CollectionItemView : RavenhillUIBehaviour {

        private CollectionData data;

        public void Setup(CollectionData data) {
            this.data = data;

            nameText.text = resourceService.GetString(data.nameId);
            iconImage.overrideSprite = resourceService.GetSprite(data);

            int playerCount = playerService.GetItemCount(data);
            if(playerCount > 0 ) {
                iconImage.color = nonEmptyIconColor;
                countPanObj.ActivateSelf();
                countText.text = playerCount.ToString();
            } else {
                iconImage.color = emptyIconColor;
                countText.text = string.Empty;
                countPanObj.DeactivateSelf();
            }

            var expItem = data.GetDropItem(DropType.exp);
            if(expItem != null ) {
                expCountText.text = expItem.count.ToString();
            } else {
                expCountText.text = string.Empty;
            }

            var silverItem = data.GetDropItem(DropType.silver);
            if(silverItem != null ) {
                silverCountText.text = silverItem.count.ToString();
            } else {
                silverCountText.text = string.Empty;
            }
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.InventoryChanged += OnInventoryChanged;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.InventoryChanged -= OnInventoryChanged;
        }

        private void OnInventoryChanged(InventoryItemType type, string id, int count ) {
            if(data != null ) {
                if(type == data.type && id == data.id ) {
                    Setup(data);
                }
            }
        }
    }

    public partial class CollectionItemView : RavenhillUIBehaviour {

        [SerializeField]
        private Text nameText;

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private GameObject countPanObj;

        [SerializeField]
        private Text countText;

        [SerializeField]
        private Text expCountText;

        [SerializeField]
        private Text silverCountText;

        [SerializeField]
        private Color emptyIconColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);

        [SerializeField]
        private Color nonEmptyIconColor = new Color(1, 1, 1, 1);
    }


}
