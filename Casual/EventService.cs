using Casual.Ravenhill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual {

    public class EventService : IEventService {

        private Dictionary<GameEventName, Dictionary<string, IEventListener<GameEventName>>> listeners { get; } = new Dictionary<GameEventName, Dictionary<string, IEventListener<GameEventName>>>();

        public void Add(GameEventName eventName, IEventListener<GameEventName> listener) {
            if (listeners.ContainsKey(eventName)) {
                Dictionary<string, IEventListener<GameEventName>> filteredListeners = listeners[eventName];
                filteredListeners[listener.listenerName] = listener;
            } else {
                Dictionary<string, IEventListener<GameEventName>> newListeners = new Dictionary<string, IEventListener<GameEventName>> {
                    [listener.listenerName] = listener
                };
                listeners.Add(eventName, newListeners);
            }
        }

        public void Remove(GameEventName eventName, IEventListener<GameEventName> listener) {
            if (listeners.ContainsKey(eventName)) {
                Dictionary<string, IEventListener<GameEventName>> filteredListeners = listeners[eventName];
                if (filteredListeners.ContainsKey(listener.listenerName)) {
                    filteredListeners.Remove(listener.listenerName);
                }
            }
        }

        public void RemoveAll(IEventListener<GameEventName> listener) {
            foreach (var kvp in listeners) {
                Dictionary<string, IEventListener<GameEventName>> filteredListeners = kvp.Value;
                if (filteredListeners.ContainsKey(listener.listenerName)) {
                    filteredListeners.Remove(listener.listenerName);
                }
            }
        }

        public void SendEvent(EventArgs<GameEventName> eventArgs) {
            if(listeners.ContainsKey(eventArgs.eventName)) {
                foreach(var kvp in listeners[eventArgs.eventName]) {
                    kvp.Value.OnEvent(eventArgs);
                }
            }
        }

        public void SendEvent(EventArgs args ) {
            SendEvent(args as EventArgs<GameEventName>);
        }

        public virtual void Setup(object data) { }
    }

    

    public interface IEventService : IService {
        void SendEvent(EventArgs eventArgs);
        void Add(GameEventName eventName, IEventListener<GameEventName> listener);
        void Remove(GameEventName eventName, IEventListener<GameEventName> listener);
        void RemoveAll(IEventListener<GameEventName> listener);
    }

    public abstract class EventArgs {

        public T Cast<T>() where T : EventArgs {
            return (this as T);
        }
    }

    public abstract class EventArgs<T> : EventArgs {

        public EventArgs(T name) {
            eventName = name;
        }

        public T eventName { get; }
    }

    public interface IEventListener {
        string listenerName { get; }
    }

    public interface IEventListener<T> : IEventListener {
        void OnEvent(EventArgs<T> args);
    }

    
}
