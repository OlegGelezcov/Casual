using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ravtool {
    public interface ICommand {
        CommandType Type { get; }
    }
}
