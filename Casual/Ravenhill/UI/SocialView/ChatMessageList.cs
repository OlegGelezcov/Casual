using Casual.Ravenhill.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.Ravenhill.UI {
    public partial class ChatMessageList : RavenhillUIBehaviour {

        private const int MAX_VISIBLE_MESSAGES = 50;

        private readonly List<ChatMessageView> messageViews = new List<ChatMessageView>();

        public void Setup() {
            ClearViews(messageViews);

            var sourceMessages = engine.GetService<IChatService>().Messages;
            for (int i = 0; i < Mathf.Min(MAX_VISIBLE_MESSAGES, sourceMessages.Count); i++ ) {
                IMessage message = sourceMessages[i];
                AddMessage(message);
            }
            RemoveExceededMessages();
        }

        private void AddMessage(IMessage message) {
            GameObject messageInstance = Instantiate<GameObject>(messagePrefab);
            ChatMessageView view = messageInstance.GetComponent<ChatMessageView>();
            messageInstance.transform.SetParent(messageLayout, false);
            view.Setup(message as ChatMessage);
            messageViews.Add(view);
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.ChatMessagesReceived += OnChatMessagesReceived;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.ChatMessagesReceived -= OnChatMessagesReceived;
        }

        private void OnChatMessagesReceived(List<ChatMessage> messages ) {
            List<ChatMessage> sortedMessages = messages.OrderByDescending(m => m.GetTime()).ToList();

            foreach(ChatMessage message in sortedMessages) {
                AddMessage(message);
            }
            RemoveExceededMessages();
        }

        private void RemoveExceededMessages() {
            while(messageViews.Count > MAX_VISIBLE_MESSAGES ) {
                RemoveLastMessage();
            }
        }

        private void RemoveLastMessage() {
            if(messageViews.Count > 0 ) {
                int index = messageViews.Count - 1;
                var view = messageViews[index];
                messageViews.RemoveAt(index);
                Destroy(view.gameObject);
                view = null;
            }
        }
    }

    public partial class ChatMessageList : RavenhillUIBehaviour {

        [SerializeField]
        private Transform messageLayout;

        [SerializeField]
        private GameObject messagePrefab;


    }
}
