using UnityEngine;

namespace Casual.Ravenhill.Test {
    public class RavenhillTestListener : RavenhillBaseListenerBehaviour {

        public override void Start() {
            base.Start();
            
        }

        public override void OnEnable() {
            base.OnEnable();
            engine.GetService<IEventService>().Add(GameEventName.touch, this);
            engine.GetService<IEventService>().Add(GameEventName.resource_loaded, this);
        }

        public override void OnDisable() {
            base.OnDisable();

            engine?.GetService<IEventService>().RemoveAll(this);
        }

        public override string listenerName => "test_listener";

        public override void OnEvent(EventArgs<GameEventName> args) {
            switch(args.eventName) {
                case GameEventName.touch: {
                        TouchEventArgs touchEventArgs = args.Cast<TouchEventArgs>();
                        Debug.Log($"touch event args {touchEventArgs.position}");
                    }
                    break;
                case GameEventName.resource_loaded: {
                        RavenhillResourceLoadedEventArgs eventArgs = args.Cast<RavenhillResourceLoadedEventArgs>();
                        Debug.Log($"resource loaded event...");
                    }
                    break;
            }
        }
    }
}
