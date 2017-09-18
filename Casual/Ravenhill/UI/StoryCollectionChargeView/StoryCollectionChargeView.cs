using Casual.Ravenhill.Data;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {

    public partial class StoryCollectionChargeView : RavenhillCloseableView {
        public override RavenhillViewType viewType => RavenhillViewType.story_collection_charge_view;

        public override bool isModal => true;

        public override int siblingIndex => 3;

        private StoryCollectionData cachedData = null;

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.InventoryChanged += OnInventoryChanged;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.InventoryChanged -= OnInventoryChanged;
        }

        private void OnInventoryChanged(InventoryItemType type, string itemId, int count ) {
            if(type == InventoryItemType.StoryCharger || type == InventoryItemType.StoryCollectable || type == InventoryItemType.StoryCollection ) {
                if(cachedData != null ) {
                    Setup(cachedData);
                    Debug.Log("OnInventoryAdded resetup StoryCollectionChargerView");
                }
            }
        }

        public override void Setup(object data = null) {
            base.Setup(data);

            StoryCollectionData storyCollectionData = data as StoryCollectionData;
            cachedData = storyCollectionData;

            if(storyCollectionData == null ) {
                throw new ArgumentException($"{GetType().Name}: must have setup argument {typeof(StoryCollectionData).Name}");
            }

            for(int i = 0; i < storyCollectableViews.Length; i++ ) {
                if(i < storyCollectionData.collectables.Count ) {
                    StoryCollectableData collectableData = resourceService.GetStoryCollectable(storyCollectionData.collectables[i]);
                    storyCollectableViews[i].Setup(collectableData);
                }
            }

            StoryChargerData storyChargerData = resourceService.GetStoryCharger(storyCollectionData.chargerId);
            chargerView.Setup(storyChargerData);

            SetupChargeButton(storyCollectionData);
            SetupCollectionIcon(storyCollectionData);
            closeBigButton.SetListener(() => Close(), engine.GetService<IAudioService>());
            buyChargerView.Setup(storyCollectionData);
        }

        private void SetupChargeButton(StoryCollectionData storyCollectionData) {
            int playerCollectionCount = playerService.GetItemCount(storyCollectionData);
            if(playerCollectionCount > 0 ) {
                chargeButton.DeactivateSelf();
            } else {
                chargeButton.ActivateSelf();
                if(ravenhillGameModeService.IsStoryCollectionReadyToCharge(storyCollectionData)) {
                    chargeButton.interactable = true;
                    chargeButton.SetListener(() => {
                        engine.GetService<IVideoService>().PlayVideo(storyCollectionData.videoId, () => {
                            ravenhillGameModeService.ChargeStoryCollection(storyCollectionData);
                        });
                    }, engine.GetService<IAudioService>());
                } else {
                    chargeButton.interactable = false;
                }
            }
        }


        private void SetupCollectionIcon(StoryCollectionData storyCollectionData) {
            storyCollectionChargeBackground.overrideSprite = resourceService.GetSprite(storyCollectionData.chargeBackImageId, storyCollectionData.chargeBackImagePath);

            int playerCount = playerService.GetItemCount(storyCollectionData);
            if(playerCount > 0 ) {
                Sprite sprite = resourceService.GetSprite(storyCollectionData.chargeColorImageId, storyCollectionData.chargeColorImagePath);
                SetStoryCollectionIconSprite(sprite);
            } else {
                Sprite sprite = resourceService.GetSprite(storyCollectionData.chargeGrayImageId, storyCollectionData.chargeGrayImagePath);
                SetStoryCollectionIconSprite(sprite);
            }
        }

        private void SetStoryCollectionIconSprite(Sprite sprite) {
            storyCollectionIconImage.rectTransform.sizeDelta = new Vector2(sprite.rect.width, sprite.rect.height);
            storyCollectionIconImage.overrideSprite = sprite;
        }
    }

    public partial class StoryCollectionChargeView : RavenhillCloseableView {

        [SerializeField]
        private StoryCollectableView[] storyCollectableViews;

        [SerializeField]
        private StoryChargerView chargerView;

        [SerializeField]
        private StoryBuyChargerView buyChargerView;

        [SerializeField]
        private Button chargeButton;

        [SerializeField]
        private Image storyCollectionChargeBackground;

        [SerializeField]
        private Image storyCollectionIconImage;

        [SerializeField]
        private Button closeBigButton;


    }
}
