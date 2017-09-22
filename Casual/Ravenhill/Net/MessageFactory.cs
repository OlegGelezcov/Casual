using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Net {

    public abstract class MessageFactory {
        public abstract IMessage Create(ISender sender, string content, List<IAttachment> attachments, MessageType messageType = MessageType.Normal);
    }

    public interface ISender {
        string GetId();
        int GetLevel();
        string GetAvatar();
        string GetName();
    }

    public interface IAttachment {
        string GetId();
        int GetItemType();
    }
}
