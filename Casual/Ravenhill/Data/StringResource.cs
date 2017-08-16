using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.Ravenhill.Data {
    public class StringResource {

        private Dictionary<uint, string> stringCache { get; } = new Dictionary<uint, string>();

        private static uint JenkinsHash(string str) {
            uint hash = 0;
            for(int i = 0; i < str.Length; ++i) {
                hash += (uint)str[i];
                hash += (hash << 10);
                hash ^= (hash >> 6);
            }
            hash += (hash << 3);
            hash ^= (hash >> 11);
            hash += (hash << 15);
            return hash;
        }

        public string GetString(string key) {

            uint hash = JenkinsHash(key);
            if(stringCache.ContainsKey(hash)) {
                return stringCache[hash];
            }
            return string.Empty;
        }

        public int count => stringCache.Count;

        public List<StringHashConflict> Load(List<string> assetFiles, SystemLanguage lang) {
            stringCache.Clear();
            List<StringHashConflict> conflicts = new List<StringHashConflict>();

            foreach(string assetFile in assetFiles) {
                conflicts.AddRange(AppenFile(assetFile, stringCache, lang));
            }

            return conflicts;
        }

        private List<StringHashConflict> AppenFile(string asset, Dictionary<uint, string> targetCache, SystemLanguage lang) {
            List<StringHashConflict> conflicts = new List<StringHashConflict>();
            string xml = UnityEngine.Resources.Load<TextAsset>(asset).text;
            UXMLDocument document = new UXMLDocument();
            document.Parse(xml);

            foreach(UXMLElement element in document.Element("strings").Elements("string")) {
                string key = element.GetString("name");
                string content = string.Empty;
                if(lang == SystemLanguage.Russian) {
                    content = element.Element("ru").value.Trim();
                } else {
                    content = element.Element("en").value.Trim();
                }
                uint hash = JenkinsHash(key);
                if(targetCache.ContainsKey(hash)) {
                    conflicts.Add(new StringHashConflict(key, content, hash, asset));
                } else {
                    targetCache.Add(hash, content);
                }
            }
            return conflicts;
        }
    }

    public class StringHashConflict {
        public string key { get; private set; }
        public string text { get; private set; }
        public uint hash { get; private set; }
        public string assetName { get; private set; }

        public StringHashConflict(string key, string text, uint hash, string assetName) {
            this.key = key;
            this.text = text;
            this.hash = hash;
            this.assetName = assetName;
        }

        public override string ToString() {
            return $"Conflict {assetName}: {key}({hash}) - {text}";
        }
    }
}
