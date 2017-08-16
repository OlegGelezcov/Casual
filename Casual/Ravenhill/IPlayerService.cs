using Casual.Ravenhill.Data;

namespace Casual.Ravenhill {
    public interface IPlayerService : IService {

        int exp { get; }
        int level { get; }
        string playerName { get; }
        string avatarId { get; }
        int silver { get; }
        int gold { get; }

        void AddExp(int exp);
        void SetExp(int exp);
        void AddCoins(PriceData coins);
        void AddSilver(int count);
        void AddGold(int count);
        void SetSilver(int count);
        void SetGold(int count);
        void SetName(string newName);
        void SetAvatar(AvatarData avatar);
    }
}
