using UnityEngine;

namespace Casual.Ravenhill.Test {
    public class RavenhillTestListener : RavenhillGameBehaviour {

        public override void Start() {
            base.Start();
            
        }

        public override void OnEnable() {
            base.OnEnable();
            //AddHandler(GameEventName.touch, (args) => {
            //    Debug.Log($"touch occured: {(args as TouchEventArgs).position}");
            //});
            //AddHandler(GameEventName.resource_loaded, (args) => {
            //    Debug.Log($"resources loaded event: {engine.GetService<IResourceService>()?.Cast<RavenhillResourceService>()?.stringCount}");
            //});
        }

        public override void OnDisable() {
            base.OnDisable();
        }

        //public override string listenerName => "test_listener";

        //public override void OnEvent(EventArgs<GameEventName> args) {
        //    switch(args.eventName) {
        //        case GameEventName.touch: {
        //                TouchEventArgs touchEventArgs = args.Cast<TouchEventArgs>();
        //                Debug.Log($"touch event args {touchEventArgs.position}");
        //            }
        //            break;
        //        case GameEventName.resource_loaded: {
        //                RavenhillResourceLoadedEventArgs eventArgs = args.Cast<RavenhillResourceLoadedEventArgs>();
        //                Debug.Log($"resource loaded event...");
        //            }
        //            break;
        //    }
        //}
    }
}
