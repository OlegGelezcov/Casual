using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual {
    public interface IResourceService : IService {
        void Load();
        bool isLoaded { get;  }
    }
}
