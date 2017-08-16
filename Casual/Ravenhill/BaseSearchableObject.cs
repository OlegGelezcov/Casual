using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casual.Ravenhill.Data;
using Casual.UI;
using UnityEngine;
using Casual.Ravenhill.UI;

namespace Casual.Ravenhill {
    public abstract class BaseSearchableObject : RavenhillBaseListenerBehaviour, ISearchableObject {

        public abstract string id { get; }

        public abstract bool isActive { get;  set; }

        public abstract bool isCollected { get; set; }

        public abstract void Activate(SearchObjectData data);

        public SearchObjectData data { get; protected set; }

        public virtual void Collect() {
            var transformAnimScale = gameObject.GetOrAdd<TransformAnimScale>();
            var coloredObject = gameObject.GetOrAdd<ColoredObject>();
            var coloredObjectAnim = gameObject.GetOrAdd<ColoredObjectAnim>();
            var transformAnim = gameObject.GetOrAdd<TransformAnimPosition>();

            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -50.0f);

            GameObject particles = Instantiate(engine.GetService<IResourceService>().GetCachedPrefab("search_object_particles"),
               transform);
            particles.transform.localPosition = new Vector3(0, 0, 1);

            transformAnimScale.StartAnim(new MCFloatAnimData {
                duration = 0.8f,
                start = transform.localScale.x,
                end = transform.localScale.x * 1.5f,
                endAction = () => { },
                timedActions = new List<TimedAction> {
                    new TimedAction{
                         timerPercent = 0.4f,
                          action = () => {
                              coloredObjectAnim.StartAnim(new ColorAnimData{
                                   start = Color.white,
                                   end = new Color(1, 1, 1, 0),
                                   duration = 0.8f,
                                   overwriteEasing = new OverwriteEasing{ type = MCEaseType.EaseInOutCubic },
                                   endAction = () => {
                                       //particles.transform.parent = null;
                                       //Destroy(particles, 0.5f);
                                       Destroy(gameObject);
                                       engine.GetService<IEventService>().SendEvent(new SearchObjectCollectedEventArgs(data, this));
                                   }
                              });

                              GameObject movingParticles = Instantiate(engine.GetService<IResourceService>().GetCachedPrefab("found_search_object"), transform.position, transform.rotation);
                              TransformAnimPosition movingParticlesAnim = movingParticles.GetOrAdd<TransformAnimPosition>();
                              movingParticlesAnim.StartAnim(new MCVectorAnimData {
                                   start = transform.position,
                                    end = FindSearchTextPosition(),
                                     duration = 0.8f,
                                      endAction = ()=>{

                                          Destroy(movingParticles, 0.3f);
                                      }
                              });
                              
                          }
                    }
                }
            });
        }

        private Vector3 FindSearchTextPosition() {
            SearchText searchText = FindSearchText();
            if(searchText == null ) {
                return new Vector3(1024, 768, -50);
            }
            return RectTranformToWorldPosition(searchText.GetComponent<RectTransform>());
        }


        private SearchText FindSearchText() {
            return FindObjectsOfType<SearchText>().Where(st => st.searchObjectData.id == data.id).FirstOrDefault();
        }

        private Vector3 RectTranformToWorldPosition(RectTransform rectTransform) {
            Vector2 size = Vector2.Scale(rectTransform.rect.size, rectTransform.lossyScale);
            Rect rect = new Rect(rectTransform.position.x, Screen.height - rectTransform.position.y, size.x, size.y);
            rect.x -= rectTransform.pivot.x * size.x;
            rect.y -= ( 1 - rectTransform.pivot.y) * size.y;
            Vector3 pos =  Camera.main.ScreenToWorldPoint(rect.center);
            pos.y = -pos.y;
            return pos;
        }

       
    }
}
