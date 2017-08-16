using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.Ravenhill.Data {
    public class LevelExpTable : Dictionary<int, int> {

        public void Load(string path) {
            UXMLDocument document = new UXMLDocument();
            document.Load(path);

            Clear();

            document.Element("level_exp_table").Elements("entry").ForEach((entryElement) => {
                int level = entryElement.GetInt("level");
                int exp = entryElement.GetInt("exp");
                if (!ContainsKey(level)) {
                    Add(level, exp);
                }
            });
        }

        private int m_MaxLevel = -1;


        public int maxLevel {
            get {
                if(m_MaxLevel < 0 ) {
                    List<int> allLevels = new List<int>(Keys);
                    m_MaxLevel = allLevels.Max();
                }
                return m_MaxLevel;
            }
        }

        public int GetExp(int level ) {
            level = Mathf.Clamp(level, 1, maxLevel);
            int exp = 0;
            if(TryGetValue(level, out exp)) {
                return exp;
            }
            return 0;
        }

        public int GetLevelForExp(int exp) {

            for(int i = 2; i <= maxLevel; i++ ) {
                if(this[i - 1] <= exp && this[i] > exp ) {
                    return (i - 1);
                }
            }

            if(exp >= this[maxLevel]) {
                return maxLevel;
            }
            return 1;
        }

        public int GetExpFromLastLevel(int currentExp ) {
            int currentLevel = GetLevelForExp(currentExp);
            return (currentExp - this[currentLevel]);
        }

        public float GetProgress(int currentExp ) {
            int currentLevel = GetLevelForExp(currentExp);

            int baseExp = GetExp(currentLevel);
            int nextExp = GetExp(currentLevel + 1);
            if(baseExp == nextExp ) {
                return 0.0f;
            } else {
                return Mathf.Clamp01((float)(currentExp - baseExp) / (float)(nextExp - baseExp));
            }
        }
    }
}
