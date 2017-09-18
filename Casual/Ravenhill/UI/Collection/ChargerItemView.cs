using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class ChargerItemView : RavenhillUIBehaviour {

        private ChargerData data;
        private CollectionData collectionData;

        public void Setup(CollectionData collectionData, ChargerData data) {
            this.data = data;
            this.collectionData = collectionData;
            iconImage.overrideSprite = resourceService.GetSprite(data);

            var info = collectionData.GetChargeData(data.id);
            int playerCount = playerService.GetItemCount(data);

            countText.text = string.Format($"{playerCount}/{info.count}");

            if(playerCount >= info.count ) {
                countText.color = allowColor;
                iconImage.color = nonEmptyIconColor;
            } else {
                countText.color = notAllowColor;
                iconImage.color = emptyIconColor;
            }

            trigger.SetEventTriggerClick((eventData) => {
                Debug.Log($"Click on charger: {data.id}");
                PointerEventData pointerData = eventData as PointerEventData;
                if(pointerData != null ) {
                    if(pointerData.GetPointerObjectName() == trigger.name ) {
                        viewService.ShowView(RavenhillViewType.collection_buy_charger_view, collectionData);
                    }
                }
            }, engine.GetService<IAudioService>());
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
            if (data != null && collectionData != null) {
                if (data.type == type && data.id == id) {
                    Setup(collectionData, data);
                }
            }
        }
    }

    public partial class ChargerItemView : RavenhillUIBehaviour {

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private Text countText;

        [SerializeField]
        private EventTrigger trigger;

        [SerializeField]
        private Color allowColor = Color.black;

        [SerializeField]
        private Color notAllowColor = Color.red;

        [SerializeField]
        private Color emptyIconColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);

        [SerializeField]
        private Color nonEmptyIconColor = new Color(1, 1, 1, 1);

    }
}
