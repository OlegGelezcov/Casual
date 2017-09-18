namespace Casual {

    public enum ServiceType : int {
        event_serice
    }

    public interface IService {
        void Setup(object data);
    }

    public interface IServiceLocator {
        T GetService<T>() where T : IService;
        T Register<I, T>(T service) 
            where I : IService 
            where T : I;
    }

    public interface IEngine : IServiceLocator {
        
    }

    public interface IGameElement {
        CasualEngine engine { get; }
        IViewService viewService { get; }
        IGameModeService gameModeService { get; }
    }

    public interface IButtonSoundProvider {
        void PlayButton();
    }
}
