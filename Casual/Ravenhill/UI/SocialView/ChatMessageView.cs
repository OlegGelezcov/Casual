using Casual.Ravenhill.Data;
using Casual.Ravenhill.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI  {
    public partial class ChatMessageView : RavenhillUIBehaviour {

        private readonly List<AttachmentView> attachmentViews = new List<AttachmentView>();

        private ChatMessage message = null;

        public void Setup(ChatMessage message) {
            this.message = message;

            AvatarData avatarData = resourceService.GetAvatarData(message.avatar_id);
            if(avatarData != null ) {
                avatarIconImage.overrideSprite = resourceService.GetSprite(avatarData);
            }

            nameText.text = message.sender_name;
            levelText.text = message.sender_level.ToString();
            messageText.text = message.content;
            ClearAttachments();
            if(message.AttachmentCount == 0 ) {
                attachmentLayout.gameObject.DeactivateSelf();
            } else {
                attachmentLayout.gameObject.ActivateSelf();

                foreach(ChatAttachment attachment in message.attachments) {
                    GameObject attachmentInstance = Instantiate<GameObject>(attachmentViewPrefab);
                    attachmentInstance.transform.SetParent(attachmentLayout, false);
                    attachmentInstance.GetComponent<AttachmentView>().Setup(message, attachment);
                    attachmentViews.Add(attachmentInstance.GetComponent<AttachmentView>());
                }
            }
        }

        private void ClearAttachments() {
            foreach(AttachmentView view in attachmentViews) {
                if(view) {
                    Destroy(view.gameObject);
                }
            }
            attachmentViews.Clear();
        }
    }

    public partial class ChatMessageView : RavenhillUIBehaviour {

        [SerializeField]
        private Image avatarIconImage;

        [SerializeField]
        private Text nameText;

        [SerializeField]
        private Text levelText;

        [SerializeField]
        private Text messageText;

        [SerializeField]
        private Transform attachmentLayout;

        [SerializeField]
        private GameObject attachmentViewPrefab;


    }
}
