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
        }

        public void Setup(object data) {
            
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
}
