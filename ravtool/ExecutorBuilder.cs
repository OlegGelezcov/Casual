using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ravtool {
    public class ExecutorBuilder : IExecutorBuilder {

        private readonly IStringSource source;

        public ExecutorBuilder(IStringSource source ) {
            this.source = source;
        }

        public IExecutor Build(ICommand command) {
            switch(command.Type) {
                case CommandType.FindKey: {
                        return new FindKeyCommandExecutor(command as FindKeyCommand, source);
                    }
            }
            return null;
        }
    }
}
