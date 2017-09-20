using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ravtool {
    public class FindKeyCommandExecutor : IExecutor {
        private readonly FindKeyCommand command;
        private readonly IStringSource source;

        public FindKeyCommandExecutor(FindKeyCommand command , IStringSource source ) {
            this.command = command;
            this.source = source;
        }


        public void Execute() {
            foreach(var entry in source.Strings.Where(str => str.Key.ToLower().Trim() == command.Key.ToLower().Trim())) {
                Console.WriteLine(entry.ToString());
            }
        }
    }
}
