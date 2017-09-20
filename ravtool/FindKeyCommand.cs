using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ravtool {
    public class FindKeyCommand : ICommand {

        public CommandType Type => CommandType.FindKey;

        public string Key { get; private set; }

        public FindKeyCommand(string key) {
            Key = key;
        }
    }
}
