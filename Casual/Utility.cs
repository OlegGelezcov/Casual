using System;
using UnityEngine;

namespace Casual
{
    public static class Utility {

        public static int unixTime {
            get {
                return (int)((System.DateTime.UtcNow - new System.DateTime(1970, 1, 1)).TotalSeconds);
            }
        }

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

        public static bool IsNetworkReachable {
            get {
                return Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork ||
                    Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
            }
        }

        public static string FormatMS(float interval ) {
            TimeSpan timeSpan = TimeSpan.FromSeconds(interval);
            return $"{timeSpan.Minutes.ToString("00")}:{timeSpan.Seconds.ToString("00")}";
        }

        public static string FormatHMS(float interval) {
            TimeSpan timeSpan = TimeSpan.FromSeconds(interval);
            return $"{timeSpan.Hours.ToString("00")}:{timeSpan.Minutes.ToString("00")}:{timeSpan.Seconds.ToString("00")}";
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

        public static Vector2 Range(Vector2 min, Vector2 max) {
            float x = UnityEngine.Random.Range(min.x, max.x);
            float y = UnityEngine.Random.Range(min.y, max.y);
            return new Vector2(x, y);
        }

        public static float Bezier1(float t, float start, float end ) {
            return Mathf.Lerp(start, end, t);
        }

        public static Vector3 Bezier1(float t, Vector3 start, Vector3 end ) {
            return Vector3.Lerp(start, end, t);
        }

        public static Vector2 Bezier1(float t, Vector2 start, Vector2 end ) {
            return Vector2.Lerp(start, end, t);
        }

        public static Vector2 Bezier2(float t, Vector2 start, Vector2 mid, Vector2 end) {
            return (1.0f - t) * (1.0f - t) * start +
                2.0f * t * (1.0f - t) * mid +
                t * t * end;
        }

        public static Vector3 Bezier2(float t, Vector3 start, Vector3 mid, Vector3 end ) {
            return (1.0f - t) * (1.0f - t) * start +
                    2.0f * t * (1.0f - t) * mid +
                    t * t * end;
        }

        public static float Bezier2(float t, float start, float mid, float end ) {
            return (1.0f - t) * (1.0f - t) * start +
                    2.0f * t * (1.0f - t) * mid +
                    t * t * end;
        }

        public static Vector2 Bezier3(float t, Vector2 start, Vector2 mid1, Vector2 mid2, Vector2 end ) {
            float invt = 1.0f - t;
            return invt * invt * invt * start +
                3.0f * t * invt * invt * mid1 +
                3 * t * t * invt * mid2 +
                t * t * t * end;
        }

        public static Vector3 Bezier3(float t, Vector3 start, Vector3 mid1, Vector3 mid2, Vector3 end ) {
            float invt = 1.0f - t;
            return invt * invt * invt * start +
                3.0f * t * invt * invt * mid1 +
                3 * t * t * invt * mid2 +
                t * t * t * end;
        }

        public static float Bezier3(float t, float start, float mid1, float mid2, float end ) {
            float invt = 1.0f - t;
            return invt * invt * invt * start +
                3.0f * t * invt * invt * mid1 +
                3 * t * t * invt * mid2 +
                t * t * t * end;
        }

        public static Color RGBA(int r, int g, int b, int a = 256) {
            return new Color((float)r / 256.0f, (float)g / 256.0f, (float)b / 256.0f, (float)a / 256.0f);
        }
    }
}
