using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.Ravenhill.Net {
    public class ChatService : RavenhillGameBehaviour, IChatService {

        private IChatClient client = null;
        private readonly List<IMessage> messages = new List<IMessage>();
        private MessageFactory messageFactory = null;
        private int maxCount = 100;
        private readonly UpdateTimer reconnectTimer = new UpdateTimer();

        public void Setup(object data) {
            reconnectTimer.Setup(10, (delay) => {
                GameModeName gameModeName = ravenhillGameModeService?.gameModeName ?? GameModeName.loading;
                if(gameModeName == GameModeName.map || gameModeName == GameModeName.hallway ) {
                    if(Client != null && Client.IsDisconnected && IsNetworkReachable ) {
                        Client.Connect(engine.GetService<INetService>().LocalPlayer.id);
                    }
                    Debug.Log($"Client not null: {Client != null}");
                    if(Client != null ) {
                        Debug.Log($"Is Disconnected: {Client.IsDisconnected}");
                        Debug.Log($"Is Network Reachanble: {IsNetworkReachable}");
                    }
                }
            });
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.GameModeChanged += OnGameModeChanged;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.GameModeChanged -= OnGameModeChanged;
        }

        public override void Update() {
            base.Update();
            Client?.Service();
            reconnectTimer.Update();
        }

        private void OnGameModeChanged(GameModeName oldName, GameModeName newName ) {
            if(Client != null && !Client.IsConnected ) {
                if(newName == GameModeName.map || newName == GameModeName.hallway ) {
                    if(IsNetworkReachable ) {
                        Client.Connect(engine.GetService<INetService>().LocalPlayer?.id ?? string.Empty);

                        Debug.Log($"Client not null: {Client != null}");
                        if (Client != null) {
                            Debug.Log($"Is Disconnected: {Client.IsDisconnected}");
                            Debug.Log($"Is Network Reachanble: {IsNetworkReachable}");
                        }
                    }
                }
            }
        }

        private void OnApplicationQuit() {
            Client?.Disconnect();
        }

        public IChatClient Client {
            get {
                return client;
            }
            set {
                if(client != null ) {
                    client.MessagesReceived -= OnMessagesReceived;
                }
                client = value;
                if(client != null ) {
                    client.MessagesReceived += OnMessagesReceived;
                }
            }
        }

        public MessageFactory MessageFactory {
            get {
                if(messageFactory == null ) {
                    messageFactory = new DefaultMessageFactory();
                }
                return messageFactory;
            }
            set {
                if(messageFactory == null ) {
                    messageFactory = value;
                }
            }
        }

        private void OnMessagesReceived(string json ) {
            ChatMessageContainer container = new ChatMessageContainer();
            container.LoadFromJSON(json);
            if(container.messages != null && container.messages.Count > 0 ) {
                List<ChatMessage> containerMessages = container.messages;
                containerMessages = containerMessages.OrderByDescending(m => m.GetTime()).ToList();
                messages.InsertRange(0, containerMessages);
                while(messages.Count > MaxCount ) {
                    messages.RemoveAt(messages.Count - 1);
                }
                RavenhillEvents.OnChatMessagesReceived(containerMessages);
            }
        }

        public void SendMessage(ISender sender, string content, List<IAttachment> attachments, MessageType messageType = MessageType.Normal) {
            if (Client != null && Client.IsCanChat) {
                ChatMessageContainer container = new ChatMessageContainer(MessageFactory.Create(sender, content, attachments, messageType));
                Client?.SendChatMessage(container.ToJSON());
            }
        }

        private bool IsNetworkReachable => Utility.IsNetworkReachable;

        public List<IMessage> Messages => messages;

        public int Count => messages.Count;

        public int MaxCount {
            get {
                return maxCount;
            }
            set {
                if(value > 0 ) {
                    maxCount = value;
                }
            }
        }

        public bool IsCanChat {
            get {
                return Client?.IsCanChat ?? false;
            }
        }
    }
}
