namespace Casual
{
    public interface ISaveService : IService {
        void Register(ISaveable saveable);
        void Unregister(ISaveable saveable);
        void Save();
        void Load();
        void Restart();
    }
}
