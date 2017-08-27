using Casual.Ravenhill.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class CollectionChargersView : RavenhillUIBehaviour {

        private CollectionData collectionData;

        public void Setup(CollectionData collectionData) {
            this.collectionData = collectionData;

            for (int i = 0; i < chargerViews.Length; i++) {
                if (i < collectionData.chargers.Count) {
                    var info = collectionData.chargers[i];
                    var chargerData = resourceService.GetCharger(info.id);
                    chargerViews[i].Setup(collectionData, chargerData);
                }
            }

            if (ravenhillGameModeService.IsCollectionReadyToCharge(collectionData)) {
                chargeButton.interactable = true;
            } else {
                chargeButton.interactable = false;
            }

            chargeButton.SetListener(() => {
                ravenhillGameModeService.ChargeCollection(collectionData);
            });
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.InventoryChanged += OnInventoryChanged;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.InventoryChanged -= OnInventoryChanged;
        }

        private void OnInventoryChanged(InventoryItemType type, string id, int count) {
            if(type == InventoryItemType.Collectable ||
                type == InventoryItemType.Charger || 
                type == InventoryItemType.Collection ) {
                if(collectionData != null ) {
                    Setup(collectionData);
                }
            }
        }
    }

    public partial class CollectionChargersView : RavenhillUIBehaviour {

        [SerializeField]
        private Button chargeButton;

        [SerializeField]
        private ChargerItemView[] chargerViews;

    }
}
