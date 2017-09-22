using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.UI {
    using Casual.Ravenhill.Net;
    using UnityEngine;
    using UnityEngine.UI;

    public partial class ChatView : RavenhillUIBehaviour {

        private readonly UpdateTimer inputUpdater = new UpdateTimer();

        public void Setup() {
            messageList.Setup();
            wishlistView.Setup();
            var chatService = engine.GetService<IChatService>();

            sendButton.SetListener(() => {
                if(messageInput.text.IsValid() && chatService.IsCanChat) {
                    chatService.SendMessage(
                        engine.GetService<INetService>().LocalPlayer,
                        messageInput.text, 
                        new List<IAttachment>(), 
                        MessageType.Normal
                        );
                }
            }, engine.GetService<IAudioService>());

            inputUpdater.Setup(1, (delay) => {
                if(chatService.IsCanChat ) {
                    messageInput.interactable = true;
                    sendButton.interactable = true;
                } else {
                    messageInput.interactable = false;
                    sendButton.interactable = false;
                }
            });


        }

        public override void Update() {
            base.Update();
            inputUpdater.Update();
        }
    }

    public partial class ChatView : RavenhillUIBehaviour {
        [SerializeField]
        private ChatMessageList messageList;

        [SerializeField]
        private InputField messageInput;

        [SerializeField]
        private Button sendButton;

        [SerializeField]
        private WishlistView wishlistView;

    }

}
