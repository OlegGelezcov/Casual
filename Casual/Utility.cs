using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual {
    public static class Utility {

        public static bool isEditorOrStandalone {
            get {
                return Application.isEditor || 
                    (Application.platform == RuntimePlatform.WindowsPlayer) || 
                    Application.platform == RuntimePlatform.OSXPlayer ||
                    Application.platform == RuntimePlatform.LinuxPlayer;
            }
        }

        public static string RayHitObjectName(Vector2 screenPosition, Camera camera = null) {
            if(camera == null ) {
                camera = Camera.main;
            }
            if(camera == null ) {
                return string.Empty;
            }

            Ray ray = camera.ScreenPointToRay(screenPosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit)) {
                return hit.transform.name;
            }
            return string.Empty;
        }
    }
}
