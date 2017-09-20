using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ravtool {
    public class CommandBuilder : ICommandBuilder {

        public ICommand Build(string[] commandArgs) {
            if(commandArgs.Length > 0 ) {
                switch(commandArgs[0].ToLower().Trim()) {
                    case "-findkey": {
                            if(commandArgs.Length > 1) {
                                string key = commandArgs[1];
                                FindKeyCommand command = new FindKeyCommand(key);
                                return command;
                            }
                        }
                        break;
                }
            }
            return null;
        }
    }
}
