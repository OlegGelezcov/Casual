using Casual.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class ScreenQuestListItemView : ListItemView<QuestInfo> {

        public override void Setup(QuestInfo data) {
            base.Setup(data);
            iconImage.overrideSprite = resourceService.GetSprite(data.Data);
            UpdateExclamation();
            UpdateTrigger();
        }

        private void UpdateExclamation() {
            if(data.state == QuestState.ready) {
                exclamationImage.ActivateSelf();
            } else {
                exclamationImage.DeactivateSelf();
            }
        }

        private void UpdateTrigger() {
            trigger.SetEventTriggerClick((ped) => {
                iconImage.gameObject.GetOrAdd<RectTransformAnimScale>().StartAnim(new MCFloatAnimData {
                    start = 1,
                    end = 1.1f,
                    duration = 0.15f,
                    overwriteEasing = new OverwriteEasing { type = MCEaseType.EaseInOutCubic },
                    endAction = () => {
                        iconImage.gameObject.GetOrAdd<RectTransformAnimScale>().StartAnim(new MCFloatAnimData {
                            start = 1.1f,
                            end = 1,
                            duration = 0.15f,
                            overwriteEasing = new OverwriteEasing { type = MCEaseType.EaseInOutCubic },
                            endAction = () => {
                                if (data.state == QuestState.ready) {
                                    engine.GetService<IQuestService>().ShowRewardExplicit(data);
                                } else {
                                    viewService.ShowView(RavenhillViewType.journal, data.Data);
                                }
                            }
                        });
                    }
                });
            }, engine.GetService<IAudioService>());
        }
        
    }

    public partial class ScreenQuestListItemView : ListItemView<QuestInfo> {

        [SerializeField]
        private EventTrigger trigger;

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private Image exclamationImage;


    }
}
