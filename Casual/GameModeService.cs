using Casual.Ravenhill;
using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual {

    public class GameModeService : GameBehaviour, IGameModeService {

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
                engine.GetService<IEventService>().SendEvent(new GameModeChangedEventArgs(oldGameModeName, this.gameModeName));
            }
        }
    }

    public interface IGameModeService : IService {
        GameModeName gameModeName { get; }
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


}
