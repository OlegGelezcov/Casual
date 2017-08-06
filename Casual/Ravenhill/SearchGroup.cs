using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.Ravenhill {
    public class SearchGroup : GameBehaviour {

        public bool isActiveGroup { get; private set; } = false;

        public BaseSearchableObject[] Activate(int maxObjectCount) {
            if(!isActiveGroup) {
                BaseSearchableObject[] searchObjects = GetComponentsInChildren<BaseSearchableObject>();
                searchObjects.ShuffleArray();

                List<BaseSearchableObject> targetObjects = new List<BaseSearchableObject>();
                int maxCount = Mathf.Min(searchObjects.Length, maxObjectCount);

                for(int i = 0; i < maxCount; i++ ) {
                    targetObjects.Add(searchObjects[i]);
                }

                for(int i = 0; i < searchObjects.Length; i++ ) {
                    bool contains = false;
                    for(int j = 0; j < targetObjects.Count; j++ ) {
                        if(targetObjects[j].id == searchObjects[i].id ) {
                            contains = true;
                            break;
                        }
                    }
                    if(!contains) {
                        searchObjects[i].gameObject.SetActive(false);
                    }
                }
                isActiveGroup = true;
                return targetObjects.ToArray();
            }
            return new BaseSearchableObject[] { };
        }
    }
}
