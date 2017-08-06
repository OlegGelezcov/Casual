using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual {
    public abstract class ResourceService : GameElement, IResourceService {

        public abstract bool isLoaded { get; }

        public abstract void Load();

        public abstract void Setup(object data);

    }
}
