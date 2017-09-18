using UnityEngine;

namespace Casual
{
    public class GameBehaviour : MonoBehaviour, IGameElement {

        private IViewService m_ViewSerice;
        private IGameModeService m_GameModeService;

        public CasualEngine engine =>
            CasualEngine.instance;

        public virtual IViewService viewService {
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

        public virtual void Awake() { }
        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void OnEnable() { }
        public virtual void OnDisable() { }
    }
}
