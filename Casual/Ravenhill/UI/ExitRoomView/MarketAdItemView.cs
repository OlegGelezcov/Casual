using Casual.Ravenhill.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public class MarketAdItemView : RavenhillGameBehaviour {

        [SerializeField]
        private Button m_ActionButton;

        [SerializeField]
        private Image m_IconImage;

        [SerializeField]
        private Text m_DescriptionText;


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
