using Casual.Ravenhill.Data;
using Casual.UI;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class StoryCollectionView : RavenhillUIBehaviour {

        public void Setup() {
            var collection = resourceService.GetStoryCollection(collectionId);

            if(collection == null ) {
                throw new NullReferenceException("storyCollection");
            }

            int playerCount = playerService.GetItemCount(collection);

            if(playerCount > 0 ) {
                presentImage.ActivateSelf();
                nonPresentImage.DeactivateSelf();
            } else {
                presentImage.DeactivateSelf();
                nonPresentImage.ActivateSelf();
            }

            presentTrigger.SetEventTriggerClick(p => {
                PointerEventData pointerData = p as PointerEventData;
                if(pointerData.GetPointerObjectName() == presentTrigger.name) {
                    engine.GetService<IVideoService>().PlayVideo(collection.videoId, ()=>Debug.Log("hey"));
                }
                presentImage.gameObject.GetOrAdd<RectTransformAnimScale>().StartAnim(new MCFloatAnimData {
                    duration = 0.15f,
                    start = 1,
                    end = 1.1f,
                    overwriteEasing = new OverwriteEasing { type = MCEaseType.EaseInOutCubic },
                    endAction = () => {
                        presentImage.gameObject.GetOrAdd<RectTransformAnimScale>().StartAnim(new MCFloatAnimData {
                            duration = 0.15f,
                            start = 1.1f,
                            end = 1,
                            overwriteEasing = new OverwriteEasing { type = MCEaseType.EaseInOutCubic }
                        });
                    }
                });
            });

            nonPresentTrigger.SetEventTriggerClick(p => {
                PointerEventData pointerData = p as PointerEventData;

                if(pointerData.GetPointerObjectName() == nonPresentTrigger.name ) {
                    viewService.ShowView(RavenhillViewType.story_collection_charge_view, collection);
                }

                nonPresentImage.gameObject.GetOrAdd<RectTransformAnimScale>().StartAnim(new MCFloatAnimData {
                    duration = 0.15f,
                    start = 1,
                    end = 1.1f,
                    overwriteEasing = new OverwriteEasing { type = MCEaseType.EaseInOutCubic },
                    endAction = () => {
                        nonPresentImage.gameObject.GetOrAdd<RectTransformAnimScale>().StartAnim(new MCFloatAnimData {
                            duration = 0.15f,
                            start = 1.1f,
                            end = 1,
                            overwriteEasing = new OverwriteEasing { type = MCEaseType.EaseInOutCubic }
                        });
                    }
                });
            });
        }

        private void OnTriggerClick(StoryCollectionData data) {
            if(Application.isMobilePlatform ) {
                Handheld.PlayFullScreenMovie(data.id, Color.black, FullScreenMovieControlMode.CancelOnInput);
            } else {
                
            }
        }
    }

    public partial class StoryCollectionView : RavenhillUIBehaviour {

        [SerializeField]
        private string collectionId;

        [SerializeField]
        private Image nonPresentImage;

        [SerializeField]
        private Image presentImage;

        [SerializeField]
        private EventTrigger nonPresentTrigger;

        [SerializeField]
        private EventTrigger presentTrigger;

    }
}
