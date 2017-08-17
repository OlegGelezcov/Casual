namespace Casual.Ravenhill {
    public class RavenhillGameElement : GameElement {

        private RavenhillResourceService m_ResourceService = null;

        public RavenhillResourceService resourceService {
            get {
                if (m_ResourceService == null) {
                    m_ResourceService = engine.GetService<IResourceService>().Cast<RavenhillResourceService>();
                }
                return m_ResourceService;
            }
        }

    }
}
