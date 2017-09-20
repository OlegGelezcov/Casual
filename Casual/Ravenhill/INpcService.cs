using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill {
    public interface INpcService : IService {
        List<BuffData> GetBuffs(string roomId);
        float GetBuffValue(string roomId, string buffId);
        bool HasNpc(string npcId, string roomId);
        NpcInfo GetNpc(string npcId, string roomId);
        void RemoveNpc(string roomId);
    }
}
