using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill {
    public interface IVideoService : IService {
        void PlayVideo(string id, System.Action completeAction);
    }
}
