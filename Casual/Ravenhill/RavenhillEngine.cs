namespace Casual.Ravenhill {

    public class RavenhillEngine : CasualEngine {

        private bool m_IsServicesRegistered = false;

        public override void Awake() {
            base.Awake();
            RegisterServices();
            SetupServices();
        }

        public override T GetService<T>() {
            RegisterServices();
            return base.GetService<T>();
        }

        private void RegisterServices() {
            if(!m_IsServicesRegistered) {
                ServiceRegistration();
                m_IsServicesRegistered = true;
            }
        }

        protected virtual void ServiceRegistration() {
            Register<IEventService, RavenhillEventService>(new RavenhillEventService());
            Register<IResourceService, RavenhillResourceService>(new RavenhillResourceService());
            Register<IViewService, RavenhillViewService>(new RavenhillViewService());
            Register<ICanvasSerive, CanvasService>(FindObjectOfType<CanvasService>());
            Register<IGameModeService, RavenhillGameModeService>(FindObjectOfType<RavenhillGameModeService>());
        }

        protected virtual void SetupServices() {
            GetService<IEventService>().Setup(null);
            GetService<IResourceService>().Setup(null);
            GetService<IViewService>().Setup(null);
            GetService<ICanvasSerive>().Setup(null);
            GetService<IGameModeService>().Setup(null);
        }
    }
}
