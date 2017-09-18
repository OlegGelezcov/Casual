using System.Collections.Generic;

namespace Casual.Ravenhill.UI {
    using Casual.UI;
    using UnityEngine;
    using UnityEngine.UI;

    public partial class ScreenText : RavenhillUIBehaviour {

        public class Data {
            public string text;
            public Vector2 screenPosition;
            public Color color = Color.white;
        }

        public void Setup(Data data) {
            GetComponent<RectTransform>().anchoredPosition = canvasService.TouchPositionToCanvasPosition(data.screenPosition);
            text.text = data.text;
            text.color = data.color;

            canvasService.AddToFirstGroup(transform);

            gameObject.GetOrAdd<RectTransformAnimPosition>().StartAnim(new MCVectorAnimData {
                start = GetComponent<RectTransform>().anchoredPosition,
                end = GetComponent<RectTransform>().anchoredPosition + moving,
                duration = duration,
                timedActions = new List<TimedAction> {
                       new TimedAction {
                            timerPercent = 0.5f,
                             action = () => {
                                 gameObject.GetOrAdd<GraphicAnimAlpha>().StartAnim(new MCFloatAnimData {
                                      duration = duration * 0.5f,
                                      start = 1,
                                      end = 0,
                                      overwriteEasing = new OverwriteEasing{ type = MCEaseType.EaseInOutCubic },

                                 });
                             }
                       }
                  },
                endAction = () => {
                    Destroy(gameObject, 0.5f);
                   }
                
            });


        }

        public static GameObject Create(string text, Vector2 screenPosition, Color color ) {
            GameObject prefab = CasualEngine.Get<RavenhillEngine>().GetService<IResourceService>().GetCachedPrefab("screen_text");
            GameObject instance = Instantiate<GameObject>(prefab);
            instance.GetComponent<ScreenText>().Setup(new Data { text = text, color = color, screenPosition = screenPosition });
            return instance;
        }
    }

    public partial class ScreenText : RavenhillUIBehaviour {
        [SerializeField]
        private Text text;

        [SerializeField]
        private float duration;

        [SerializeField]
        private Vector2 moving = new Vector2(0, 300);
    }
}
