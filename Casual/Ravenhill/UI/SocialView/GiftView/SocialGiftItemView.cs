using Casual.Ravenhill.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.UI{
    using Casual.UI;
    using UnityEngine;
    using UnityEngine.UI;

    public partial class SocialGiftItemView : ListItemView<NetGift> {

        public override void Setup(NetGift data) {
            base.Setup(data);
            var sender = data.GetSender();
            if(sender != null ) {
                var avatarData = resourceService.GetAvatarData(sender.GetAvatar());
                if (avatarData != null) {
                    senderAvatarIconImage.overrideSprite = resourceService.GetSprite(avatarData);
                } else {
                    senderAvatarIconImage.overrideSprite = resourceService.transparent;
                }
                senderLevelText.text = sender.GetLevel().ToString();
                senderNameText.text = sender.GetName();
            } else {
                ClearSender();
            }

            var itemData = data.GetItemData();
            if(itemData != null ) {
                iconImage.overrideSprite = resourceService.GetSprite(itemData);
                descriptionText.text = resourceService.GetString(itemData.descriptionId);
                takeButton.ActivateSelf();
                takeButton.SetListener(() => {
                    engine.GetService<INetService>().TakeGift(data.Id);
                    takeButton.DeactivateSelf();
                }, engine.GetService<IAudioService>());
            } else {
                ClearItem();
            }
        }

        private void ClearSender() {
            senderAvatarIconImage.overrideSprite = resourceService.transparent;
            senderLevelText.text = string.Empty;
            senderNameText.text = string.Empty;
        }

        private void ClearItem() {
            iconImage.overrideSprite = resourceService.transparent;
            descriptionText.text = string.Empty;
            takeButton.DeactivateSelf();
        }
    }

    public partial class SocialGiftItemView : ListItemView<NetGift> {

        [SerializeField]
        private Image senderAvatarIconImage;

        [SerializeField]
        private Text senderLevelText;

        [SerializeField]
        private Text senderNameText;

        [SerializeField]
        private Text descriptionText;

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private Button takeButton;
    }
}
