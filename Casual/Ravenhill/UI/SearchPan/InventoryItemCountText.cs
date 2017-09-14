namespace Casual.Ravenhill.UI {
    using Casual.Ravenhill.Data;
    using UnityEngine;
    using UnityEngine.UI;

    public partial class InventoryItemCountText : RavenhillUIBehaviour {

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.InventoryChanged += OnInventoryChanged;
            UpdateCountText();
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.InventoryChanged -= OnInventoryChanged;
        }

        private void UpdateCountText() {
            int count = playerService.GetItemCount(resourceService.GetInventoryItemData(itemType, itemId));
            countText.text = count.ToString();
        }

        private void OnInventoryChanged(InventoryItemType itemType, string itemId, int count ) {
            UpdateCountText();
        }
    }

    public partial class InventoryItemCountText : RavenhillUIBehaviour {
        [SerializeField]
        private Text countText;

        [SerializeField]
        private InventoryItemType itemType;

        [SerializeField]
        private string itemId;
    }
}
