using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ravtool {
    class Program {
        static void Main(string[] args) {

            StringSource source = new StringSource(@"C:\Users\olegg\Documents\Ravenhill\Assets\Resources\Data\Loc");
            ExecutorBuilder executorBuilder = new ExecutorBuilder(source);
            CommandBuilder commandBuilder = new CommandBuilder();
            ICommand command = commandBuilder.Build(args);
            if (command != null) {
                IExecutor executor = executorBuilder.Build(command);
                executor?.Execute();
            }
        }

    }
}
