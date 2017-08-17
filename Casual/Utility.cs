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
            if (camera == null) {
                camera = Camera.main;
            }
            if (camera == null) {
                return string.Empty;
            }

            Ray ray = camera.ScreenPointToRay(screenPosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                return hit.transform.name;
            }
            return string.Empty;
        }

        public static string RayHitObjectName2D(Vector2 screenPosition ) {
            var ray = Camera.main.ScreenPointToRay(new Vector3(screenPosition.x, screenPosition.y, 0.0f));
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            return hit.collider?.name ?? string.Empty;
        }


        public static SystemLanguage gameLanguage {
            get {
                if (Application.systemLanguage == SystemLanguage.Russian) {
                    return SystemLanguage.Russian;
                }

                return SystemLanguage.English;
            }

        }

        public static string FormatMS(float interval ) {
            TimeSpan timeSpan = TimeSpan.FromSeconds(interval);
            return $"{timeSpan.Minutes.ToString("00")}:{timeSpan.Seconds.ToString("00")}";
        }

        public static int TryParseInt(string text, int defaultValue = 0) {
            int ret;
            if(int.TryParse(text, out ret)) {
                return ret;
            }
            return defaultValue;
        }

        public static int GetIntOrDefault(string source, int defaultValue = 0) {
            int val;
            if (int.TryParse(source, out val)) {
                return val;
            }
            return defaultValue;
        }

        public static float GetFloatOrDefault(string source, float defaultValue = 0.0f) {
            float val;
            if (float.TryParse(source, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out val)) {
                return val;
            }
            return defaultValue;
        }

        public static bool GetBoolOrDefault(string source, bool defaultValue = false) {
            bool val;
            if (bool.TryParse(source, out val)) {
                return val;
            }
            return defaultValue;
        }
    }
}
