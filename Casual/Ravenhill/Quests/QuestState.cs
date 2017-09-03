using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill{
    public enum QuestState : byte {
        not_started = 0,
        started = 1,
        ready = 2,
        completed = 3
    }
}
