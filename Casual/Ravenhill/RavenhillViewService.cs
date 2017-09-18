using Casual.Ravenhill.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.Ravenhill {
    public class RavenhillViewService : ViewSerive {

        private Dictionary<RavenhillViewType, GameObject> cachedPrefabs { get; } = new Dictionary<RavenhillViewType, GameObject>();
        private Dictionary<RavenhillViewType, GameObject> openedViews { get; } = new Dictionary<RavenhillViewType, GameObject>();
        private Dictionary<RavenhillViewType, string> prefabPath { get; } = new Dictionary<RavenhillViewType, string>();
        private float lastViewRemovedTime = 0.0f;

        public override float LastViewRemovedTime => lastViewRemovedTime;

        public override void Setup(object data) {
            if(data != null ) {
                Load((string)data);
            }
        }

        public override GameObject GetView(RavenhillViewType viewType) {
            return openedViews.GetOrDefault(viewType);
        }

        public override bool hasModals => (modalCount > 0);

        //public bool noModals => (!hasModals);

        private int modalCount {
            get {
                int counter = 0;
                foreach(var kvp in openedViews) {
                    if(kvp.Value ) {
                        var view = kvp.Value.GetComponentInChildren<RavenhillBaseView>();
                        if(view.isModal) {
                            counter++;
                        }
                    }
                }
                return counter;
            }
        }

        public void Load(string xml) {
            prefabPath.Clear();
            UXMLDocument document = new UXMLDocument();
            document.Parse(xml);
            var pairs = (from viewElement in document.Element("views").Elements("view")
                        select new {
                            type = viewElement.GetEnum<RavenhillViewType>("type"),
                            path = viewElement.GetString("path")
                        }).ToList();
            pairs.ForEach(pair => prefabPath.Add(pair.type, pair.path));
        }

        public string GetPath(RavenhillViewType viewType) {
            return prefabPath.ContainsKey(viewType) ? prefabPath[viewType] : string.Empty;
        }


        private GameObject GetPrefab(RavenhillViewType viewType ) {
            if(cachedPrefabs.ContainsKey(viewType)) {
                return cachedPrefabs[viewType];
            } else {
                string path = GetPath(viewType);
                if(!string.IsNullOrEmpty(path)) {
                    GameObject prefab = Resources.Load<GameObject>(path);
                    if(prefab != null ) {
                        cachedPrefabs[viewType] = prefab;
                    } else {
                        throw new ResourceFailedLoadException(path);
                    }
                    return prefab;
                } else {
                    throw new UnityException($"Not founded path for view type: {viewService}");
                }
            }
        }

        private GameObject CreatePrefabAndAddToScreen(RavenhillViewType viewType, object data = null ) {
            GameObject prefab = GetPrefab(viewType);
            GameObject instance = GameObject.Instantiate<GameObject>(prefab);
            RavenhillBaseView baseView = instance.GetComponentInChildren<RavenhillBaseView>();
            baseView?.Setup(data);
            ICanvasSerive canvasService = engine.GetService<ICanvasSerive>();
            canvasService?.Add(instance.transform);
            SetSibling(baseView);
            return instance;
        }

        private void SetSibling(RavenhillBaseView baseView) {
            if(baseView == null ) { return; }

            List<RavenhillBaseView> currentViews = (from kvp in openedViews
                                                    where kvp.Value
                                                    let view = kvp.Value.GetComponentInChildren<RavenhillBaseView>()
                                                    where view
                                                    select view).ToList();
            currentViews.Add(baseView);
            currentViews.Sort((a, b) => a.siblingIndex.CompareTo(b.siblingIndex));
            for(int i = 0; i < currentViews.Count; i++ ) {
                currentViews[i].transform.SetSiblingIndex(i);
            }
            ICanvasSerive canvasService = engine.GetService<ICanvasSerive>();
            canvasService?.RestoreSiblings();

        }

        public override void ShowViewWithDelay(RavenhillViewType viewType, float delay, object data = null) {
            Debug.Log($"SHow view {viewType} with delay {delay}");
            engine.StartCoroutine(CorShowViewWithDelay(viewType, delay, data));
        }

        private System.Collections.IEnumerator CorShowViewWithDelay(RavenhillViewType type, float delay, object data) {
            yield return new WaitForSeconds(delay);
            ShowView(type, data);
        }

        public override void ShowViewWithCondition(RavenhillViewType viewType, Func<bool> predicate, object data = null) {
            engine.StartCoroutine(CorShowViewWithCondition(viewType, predicate, data));
        }

        private System.Collections.IEnumerator CorShowViewWithCondition(RavenhillViewType viewType, Func<bool> predicate, object data = null) {
            yield return new WaitUntil(predicate);
            ShowView(viewType, data);
        }

        public override  GameObject ShowView(RavenhillViewType type, object data = null ) {
            GameObject resultObject = null;

            if(openedViews.ContainsKey(type)) {
                GameObject existingViewObj = openedViews[type];
                if(existingViewObj) {
                    existingViewObj.GetComponentInChildren<RavenhillBaseView>()?.Setup(data);
                    resultObject = existingViewObj;
                } else {
                    openedViews.Remove(type);
                    resultObject = CreatePrefabAndAddToScreen(type, data);
                    openedViews.Add(type, resultObject);
                }
            } else {
                resultObject = CreatePrefabAndAddToScreen(type, data);
                openedViews.Add(type, resultObject);
            }

            RavenhillEvents.OnViewAdded(type);
            return resultObject;
        }

        public override void RemoveView(RavenhillViewType viewType ) {
            if(openedViews.ContainsKey(viewType)) {
                GameObject instance = openedViews[viewType];
                openedViews.Remove(viewType);

                if(instance ) {
                    RavenhillBaseView baseView = instance.GetComponentInChildren<RavenhillBaseView>();
                    baseView?.OnViewWillBeClosed();

                    if(baseView ) {
                        baseView.FadeOut();
                    } else {
                        GameObject.Destroy(instance);
                    }
                    lastViewRemovedTime = Time.time;
                    RavenhillEvents.OnViewRemoved(viewType);
                }
            }
        }

        public override void RemoveView(RavenhillViewType viewType, float delay) {
            engine.StartCoroutine(CorRemoveView(viewType, delay));
        }

        private System.Collections.IEnumerator CorRemoveView(RavenhillViewType viewType, float delay) {
            yield return new WaitForSeconds(delay);
            RemoveView(viewType);
        }


        public override void RemoveAll() {
            List<RavenhillViewType> viewTypes = new List<RavenhillViewType>(openedViews.Keys);
            foreach(RavenhillViewType viewType in viewTypes ) {
                RemoveView(viewType);
            }
        }

        public override bool ExistView(RavenhillViewType viewType) {
            return openedViews.ContainsKey(viewType);
        }

        //public override GameObject ShowView(object viewType, object data = null) {
        //    return this.ShowView((RavenhillViewType)viewType, data);
        //}

        //public override void RemoveView(object viewType) {
        //    this.RemoveView((RavenhillViewType)viewType);
        //}

        public override void CreateScreenText(string text, Vector2 screenPosition, Color color) {
            ScreenText.Create(text, screenPosition, color);
        }
    }



    public class ResourceFailedLoadException : UnityException {

        public string path { get; }

        public ResourceFailedLoadException(string path) {
            this.path = path;
        }

        public override string Message => string.Format($"Failed load resource at path {path}");
    }
}
