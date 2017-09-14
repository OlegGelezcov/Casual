namespace Casual.Ravenhill.UI {
    using Casual.UI;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public partial class BuffView : ListItemView<BuffInfo> {

        private BuffInfo buff;
        private readonly UpdateTimer updateTimer = new UpdateTimer();

        public override void Setup(BuffInfo buff ) {
            base.Setup(buff);

            this.buff = buff;
            this.name = buff.Id;

            iconImage.overrideSprite = resourceService.GetSprite(buff.Data);
            updateTimer.Setup(1.0f, (delay) => {
                UpdateTimer();
            });
            trigger.SetEventTriggerClick(p => {
                PointerEventData ped = p as PointerEventData;
                if(ped.pointerCurrentRaycast.isValid) {
                    if(ped.GetPointerObjectName() == this.name ) {
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
                                        viewService.ShowView(RavenhillViewType.hint_view, new HintView.ItemData {
                                            data = buff.Data,
                                            offsetType = HintView.OffsetType.Left,
                                            screenPosition = engine.Input.lastPointerPosition
                                        });
                                    }
                                });
                            }
                        });
                    }
                }
            });
        }

        private void UpdateTimer() {
            if(buff != null ) {
                int remainTime = Mathf.RoundToInt(buff.RemainTime);
                if(remainTime <= 0 ) {
                    timerText.text = string.Empty;
                } else {
                    timerText.text = Utility.FormatHMS(remainTime);
                }
            }
        }

        public override void Update() {
            base.Update();
            updateTimer.Update();
        }
    }

    public partial class BuffView : ListItemView<BuffInfo> {

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private Text timerText;

        [SerializeField]
        private EventTrigger trigger;
    }
}
