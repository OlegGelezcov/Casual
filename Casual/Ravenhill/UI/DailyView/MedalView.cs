using System.Collections.Generic;
using System.Linq;

namespace Casual.Ravenhill.UI {
    using Casual.Ravenhill.Data;
    using UnityEngine;
    using UnityEngine.UI;

    public partial class MedalView : RavenhillUIBehaviour {
        
        public void Setup(int currentDay) {
            if(day > currentDay) {
                medalIcon.overrideSprite = grayMedalSprite;
            }

            List<DropItem> rewards = resourceService.GetDailyRewards(day);
            if(rewards.Count == 1) {
                plusIcon.DeactivateSelf();
                itemFrame.DeactivateSelf();
            } else if(rewards.Count > 1) {
                plusIcon.ActivateSelf();
                itemFrame.ActivateSelf();

                if(day > currentDay) {
                    plusIcon.overrideSprite = grayPlusSprite;
                }

                DropItem itemDrop = rewards.Where(di => di.type == DropType.item).FirstOrDefault();
                if(itemDrop != null ) {
                    itemIcon.overrideSprite = resourceService.GetSprite(itemDrop.itemData);
                }
            }

            DropItem silverOrGoldItem = rewards.Where(di => di.type == DropType.silver || di.type == DropType.gold).FirstOrDefault();
            if(silverOrGoldItem != null ) {
                priceText.text = silverOrGoldItem.count.ToString();
                if(silverOrGoldItem.type == DropType.silver) {
                    priceImage.overrideSprite = resourceService.GetPriceSprite(new PriceData(1, 0));
                } else if(silverOrGoldItem.type == DropType.gold ) {
                    priceImage.overrideSprite = resourceService.GetPriceSprite(new PriceData(0, 1));
                }
            }

            if(day == currentDay ) {
                toggle.isOn = true;
            } 
        }
    }

    public partial class MedalView : RavenhillUIBehaviour {

        [SerializeField]
        private Toggle toggle;

        [SerializeField]
        private int day;

        [SerializeField]
        private Image medalIcon;

        [SerializeField]
        private Sprite grayMedalSprite;

        [SerializeField]
        private Image plusIcon;

        [SerializeField]
        private Sprite grayPlusSprite;

        [SerializeField]
        private Image itemFrame;

        [SerializeField]
        private Image itemIcon;

        [SerializeField]
        private Text priceText;

        [SerializeField]
        private Image priceImage;

    }
}
