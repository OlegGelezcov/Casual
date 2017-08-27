using Casual.Ravenhill.Net;

namespace Casual.Ravenhill {
    public class RavenhillGameElement : GameElement {

        private RavenhillResourceService m_ResourceService = null;
        private NetService m_NetService = null;

        public RavenhillResourceService resourceService {
            get {
                if (m_ResourceService == null) {
                    m_ResourceService = engine.GetService<IResourceService>().Cast<RavenhillResourceService>();
                }
                return m_ResourceService;
            }
        }

        


        public NetService netService {
            get {
                if (m_NetService == null) {
                    m_NetService = engine.GetService<INetService>().Cast<NetService>();
                }
                return m_NetService;
            }
        }

    }
}
