namespace Casual
{
    public class GameElement : IGameElement {

        private IViewService m_ViewSerice;
        private IGameModeService m_GameModeService;

        public CasualEngine engine =>
            CasualEngine.instance;

        public IViewService viewService {
            get {
                m_ViewSerice = m_ViewSerice ?? engine.GetService<IViewService>();
                return m_ViewSerice;
            }
        }

        public IGameModeService gameModeService {
            get {
                m_GameModeService = m_GameModeService ?? engine.GetService<IGameModeService>();
                return m_GameModeService;
            }
        }
    }
}
