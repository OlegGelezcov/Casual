using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill {
    public static class HelperUtils {
        public static bool IsHallwayGamemode(GameModeName name) {
            return (name == GameModeName.hallway) || (name == GameModeName.map);
        }
    }
}
