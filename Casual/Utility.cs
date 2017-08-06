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

        public static SystemLanguage gameLanguage {
            get {
                if (Application.systemLanguage == SystemLanguage.Russian) {
                    return SystemLanguage.Russian;
                }

                return SystemLanguage.English;
            }
        }
    }
}
