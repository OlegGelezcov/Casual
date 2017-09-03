using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill {
    public enum ConditionType : int {
        none = 0,
        level_ge = 1,
        quest_completed = 2,
        has_story_collection = 3,
        room_mode = 4,
        random = 5,
        has_collection = 6,
        last_search_room = 7,
        has_collectable = 8,
        search_counter_ge = 9
    }
}
