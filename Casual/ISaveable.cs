namespace Casual {
    public interface ISaveable {
        string saveId { get; }
        string GetSave();
        bool Load(string saveStr);
        bool isLoaded { get; }
        void InitSave();
        void OnRegister();
        void OnLoaded();
    }
}
