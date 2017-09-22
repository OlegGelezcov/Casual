using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Net {
    [Serializable]
    public class ChatMessage : IMessage {

        public string id;
        public int time;
        public string content;
        public string sended_id;
        public int sender_level;
        public string avatar_id;
        public string sender_name;

        public List<ChatAttachment> attachments;
        public int message_type;

        public ChatMessage() {
            
        }

        public ChatMessage(ISender sender, string content, List<IAttachment> attachments, MessageType messageType = MessageType.Normal) {
            id = Guid.NewGuid().ToString();
            time = Utility.unixTime;
            this.content = content;
            sended_id = sender.GetId();
            sender_level = sender.GetLevel();
            avatar_id = sender.GetAvatar();
            this.attachments = new List<ChatAttachment>();
            message_type = (int)messageType;
            sender_name = sender.GetName();

            foreach(IAttachment attach in attachments ) {
                ChatAttachment concreteAttachment = attach as ChatAttachment;
                if(concreteAttachment != null ) {
                    this.attachments.Add(concreteAttachment);
                }
            }
        }

        public string GetId() {
            return id;
        }

        public int GetTime() {
            return time;
        }

        public ISender GetSender() {
            return new BaseSender(sended_id, sender_name, avatar_id, sender_level);
        }

        public int AttachmentCount {
            get {
                if(attachments == null ) {
                    return 0;
                }
                if(attachments.Count == 0 ) {
                    return 0;
                }
                return attachments.Count;
            }
        }
    }
}
