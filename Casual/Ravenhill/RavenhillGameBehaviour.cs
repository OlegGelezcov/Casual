using Casual.Ravenhill.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill {
    public class RavenhillGameBehaviour : GameBehaviour {

        private CanvasService m_CanvasService;

        private RavenhillResourceService m_ResourceService;

        private RavenhillGameModeService m_GameModeService;

        private RavenhillViewService m_ViewService;

       // private IEventService m_EventService;

        private PlayerService m_PlayerService;

        private NetService m_NetService;


        public NetService netService {
            get {
                if(m_NetService == null ) {
                    m_NetService = engine.GetService<INetService>().Cast<NetService>();
                }
                return m_NetService;
            }
        }

        //public IEventService eventService {
        //    get {
        //        if(m_EventService == null ) {
        //            m_EventService = engine.GetService<IEventService>();
        //        }
        //        return m_EventService;
        //    }
        //}


        public RavenhillViewService ravenhillViewService {
            get {
                if(m_ViewService == null ) {
                    m_ViewService = engine.GetService<IViewService>().Cast<RavenhillViewService>();
                }
                return m_ViewService;
            }
        }

        public RavenhillGameModeService ravenhillGameModeService {
            get {
                if(m_GameModeService == null ) {
                    m_GameModeService = engine.GetService<IGameModeService>().Cast<RavenhillGameModeService>();
                }
                return m_GameModeService;
            }
        }

        public CanvasService canvasService {
            get {
                if (m_CanvasService == null) {
                    m_CanvasService = engine.GetService<ICanvasSerive>().Cast<CanvasService>();
                }
                return m_CanvasService;
            }
        }

        public RavenhillResourceService resourceService {
            get {
                if (m_ResourceService == null) {
                    m_ResourceService = engine.GetService<IResourceService>().Cast<RavenhillResourceService>();
                }
                return m_ResourceService;
            }
        }

        public PlayerService playerService {
            get {
                if(m_PlayerService == null ) {
                    m_PlayerService = engine.GetService<IPlayerService>().Cast<PlayerService>();
                }
                return m_PlayerService;
            }
        }
    }
}
