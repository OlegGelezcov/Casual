using Casual.Ravenhill.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public class MarketAdItemView : RavenhillGameBehaviour {

#pragma warning disable 0649
        [SerializeField]
        private Button m_ActionButton;

        [SerializeField]
        private Image m_IconImage;

        [SerializeField]
        private Text m_DescriptionText;
#pragma warning restore 0649

        private Button actionButton => m_ActionButton;
        private Image iconImage => m_IconImage;
        private Text descriptionText => m_DescriptionText;

        public void Setup(InventoryItemData itemData ) {
            if(itemData == null ) {
                Clear();
            } else {
                actionButton?.SetListener(() => {
                    Debug.Log("show market...");
                });
                iconImage?.ActivateSelf();
                descriptionText?.ActivateSelf();
                if(iconImage != null ) {
                    iconImage.overrideSprite = resourceService.GetSprite(itemData);
                }
                if(descriptionText != null ) {
                    descriptionText.text = resourceService.GetString(itemData.descriptionId);
                }
            }
        }

        private void Clear() {
            actionButton?.SetListener(() => { });
            iconImage?.DeactivateSelf();
            descriptionText?.DeactivateSelf();
        }
    }
}
