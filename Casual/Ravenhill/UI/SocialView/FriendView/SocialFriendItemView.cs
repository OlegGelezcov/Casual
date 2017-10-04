using Casual.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.UI{
    using Casual.Ravenhill.Data;
    using Casual.Ravenhill.Net;
    using UnityEngine.UI;

    public class SocialFriendItemView : ListItemView<NetPlayer> {

        public Image friendIcon;
        public Text friendName;
        public Text friendLevel;
        public FriendWishView[] wishViews;


        public override void Setup(NetPlayer data) {
            base.Setup(data);
            UpdateAvatar();
            UpdateName();
            UpdateLevel();
            UpdateWishViews();
        }

        private void UpdateAvatar() {
            if(!string.IsNullOrEmpty(data.avatarId)) {
                AvatarData avatarData = resourceService.GetAvatarData(data.avatarId);
                friendIcon.overrideSprite = (avatarData != null) ? resourceService.GetSprite(avatarData) : resourceService.transparent;
            } else {
                friendIcon.overrideSprite = resourceService.transparent;
            }
        }

        private void UpdateName() {
            friendName.text = data.name;
        }

        private void UpdateLevel() {
            friendLevel.text = data.level.ToString();
        }

        private void UpdateWishViews() {
            if(data.wishlistRemoted != null) {
                List<InventoryItemData> items = data.wishlistRemoted;
                for(int i = 0; i < wishViews.Length; i++ ) {
                    if (i < items.Count) {
                        wishViews[i].ActivateSelf();
                        wishViews[i].Setup(data, items[i]);
                    } else {
                        wishViews[i].DeactivateSelf();
                    }
                }
            } else {
                DeactivateWishViews();
            }
        }

        private void DeactivateWishViews() {
            foreach(var wishView in wishViews) {
                wishView.DeactivateSelf();
            }
        }
    }
}
