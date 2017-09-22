using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.Ravenhill.Net {

    [Serializable]
    public class ChatMessageContainer {

        public List<ChatMessage> messages;

        public ChatMessageContainer() { }

        public ChatMessageContainer(IMessage message) {
            ChatMessage concreteMessage = message as ChatMessage;
            if(concreteMessage != null ) {
                messages = new List<ChatMessage> { concreteMessage };
            }
        }
        
        
        public string ToJSON() {
            return JsonUtility.ToJson(this, true);
        }

        public void LoadFromJSON(string jsonText ) {
            JsonUtility.FromJsonOverwrite(jsonText, this);
        }
    }
}
