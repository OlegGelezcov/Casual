using System.Collections.Generic;

namespace Casual.Ravenhill {
    public abstract class RavenhillBaseListenerBehaviour : RavenhillGameBehaviour, IEventListener<GameEventName> {

        private Dictionary<GameEventName, System.Action<EventArgs<GameEventName>>> handlers { get; } = new Dictionary<GameEventName, System.Action<EventArgs<GameEventName>>>();


        public abstract string listenerName { get; }

        public void OnEvent(EventArgs<GameEventName> args) {
            if(handlers.ContainsKey(args.eventName)) {
                handlers[args.eventName](args);
            }
        }

        protected void AddHandler(GameEventName eventName, System.Action<EventArgs<GameEventName>> handler ) {
            handlers[eventName] = handler;
            engine.GetService<IEventService>().Add(eventName, this);
        }

        protected void RemoveHandler(GameEventName eventName) {
            if(handlers.ContainsKey(eventName)) {
                handlers.Remove(eventName);
                engine?.GetService<IEventService>()?.Remove(eventName, this);
            }
        }

        protected void RemoveAllHandlers() {
            handlers.Clear();
            engine?.GetService<IEventService>()?.RemoveAll(this);
        }

        public override void OnDisable() {
            RemoveAllHandlers();
            base.OnDisable();
        }
    }
}
