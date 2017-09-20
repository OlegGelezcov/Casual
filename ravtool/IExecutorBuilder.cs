using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ravtool {
    public interface IExecutorBuilder {
        IExecutor Build(ICommand command);
    }
}
