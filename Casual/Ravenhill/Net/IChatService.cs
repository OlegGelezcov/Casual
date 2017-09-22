using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Net {
    public interface IChatService : IService {

        IChatClient Client { get; set; }
        void SendMessage(ISender sender, string content, List<IAttachment> attachments, MessageType messageType = MessageType.Normal);
        List<IMessage> Messages { get; }
        int Count { get; }
        int MaxCount { get; }
        bool IsCanChat { get; }
    }
}
