using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Net {
    public class DefaultMessageFactory : MessageFactory {

        public override IMessage Create(ISender sender, string content, List<IAttachment> attachments, MessageType messageType = MessageType.Normal) {
            ChatMessage message = new ChatMessage(sender, content, attachments, messageType);
            return message;
        }
    }
}
