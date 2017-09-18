using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual {
    public interface ISaveService : IService {
        void Register(ISaveable saveable);
        void Unregister(ISaveable saveable);
        void Save();
        void Load();
        void Restart();
    }
}
