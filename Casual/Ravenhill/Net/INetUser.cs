namespace Casual.Ravenhill.Net {
    public interface INetUser {
        string id { get; }
        string name { get; }
        string avatarId { get; }
        int level { get; }
        bool isValid { get; }
    }
}
