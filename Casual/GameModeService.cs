using Casual.Ravenhill;

namespace Casual
{

    public abstract class GameModeService : GameBehaviour, IGameModeService {

        public GameModeName gameModeName {
            get;
            private set;
        } = GameModeName.none;



        public void Setup(object data) {
            
        }

        public void SetGameModeName(GameModeName gameModeName) {
            GameModeName oldGameModeName = this.gameModeName;
            this.gameModeName = gameModeName;
            if(oldGameModeName != this.gameModeName ) {
                RavenhillEvents.OnGameModeChanged(oldGameModeName, this.gameModeName);
            }
        }

        public abstract IRoomManager RoomManager { get; }
    }

    public interface IGameModeService : IService {
        GameModeName gameModeName { get; }
        IRoomManager RoomManager { get; }
    }

    public enum GameModeName {
        none,
        start_menu,
        loading,
        map,
        search,
        hallway
    }

    public enum SearchStatus {
        success,
        fail
    }

    public interface IRoomManager {
        void RollSearchMode();
    }
}
