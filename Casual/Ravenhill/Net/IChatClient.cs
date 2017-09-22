using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Net {
    public interface IChatClient {
        void Connect(string userId);
        void Disconnect();
        bool IsConnected { get; }
        bool IsDisconnected { get; }
        void SendChatMessage(string message);
        void Service();
        event Action<string> MessagesReceived;
        bool IsCanChat { get; }
    }
}
