using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ravtool {
    public interface IStringEntry {
        string Key { get; }
        string EnText { get; }
        string RuText { get; }
    }
}
