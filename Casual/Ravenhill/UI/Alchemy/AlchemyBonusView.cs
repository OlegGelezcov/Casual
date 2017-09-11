using System;

namespace Casual.Ravenhill.UI {
    using Casual.Ravenhill.Data;
    using UnityEngine;
    using UnityEngine.UI;

    public partial class AlchemyBonusView : NameIconView {

        private BonusData data = null;

        public override void Setup(IconData objData) {
            base.Setup(objData);

            data = objData as BonusData;
            if(data == null ) {
                throw new ArgumentException(typeof(BonusData).Name);
            }

            int playerCount = playerService.GetItemCount(data);
            countText.text = playerCount.ToString();
            descriptionText.text = resourceService.GetString(data.descriptionId);

            iconImage.color = (playerCount == 0) ? new Color(1, 1, 1, 0.5f) : Color.white;
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.InventoryChanged += OnInventoryChanged;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.InventoryChanged -= OnInventoryChanged;
        }

        private void OnInventoryChanged(InventoryItemType type, string itemId, int count ) {
            if(data != null ) {
                if(data.type == type && data.id == itemId ) {
                    Setup(data);
                }
            }
        }
    }

    public partial class AlchemyBonusView : NameIconView {

        [SerializeField]
        private Text countText;

        [SerializeField]
        private Text descriptionText;
    }
}
