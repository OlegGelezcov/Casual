using Casual.Ravenhill.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Casual.Ravenhill {
    public class SearchMissChecker : RavenhillGameElement {

        private int counter = 0;

        public void Check(Vector2 screenPosition, Camera camera = null) {
            bool isMiss = true;

            if(!EventSystem.current.IsPointerOverGameObject()) {
                camera = (camera == null) ? Camera.main : camera;
                Ray ray = camera.ScreenPointToRay(screenPosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit)) {
                    var so = hit.transform.GetComponent<BaseSearchableObject>();
                    if(so.isActive && !so.isCollected) {
                        isMiss = false;
                    }
                }
                RaycastHit2D hit2d = Physics2D.GetRayIntersection(ray);
                if(hit2d.transform) {
                    var so = hit2d.transform.GetComponent<BaseSearchableObject>();
                    if(so.isActive && !so.isCollected) {
                        isMiss = false;
                    }
                }
            }

            if(isMiss) {
                IncrementCounter(screenPosition);
                engine.GetService<IAudioService>().PlaySound(SoundType.object_miss, true);
            }
        }

        private void IncrementCounter(Vector2 screenPosition) {
            counter++;
            if(counter >= 5 ) {
                counter = 0;
                var searchTimer = GameObject.FindObjectOfType<SearchTimerView>();
                if(searchTimer != null ) {
                    if(searchTimer.IsAllowMissPenalty) {
                        searchTimer.RemoveFromTimer(10);
                        Debug.Log("remove 10 seconds...");
                        engine.GetService<IViewService>().CreateScreenText("-10", screenPosition, Color.red);
                    }
                }
            }
        }
    }
}
