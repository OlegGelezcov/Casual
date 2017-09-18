using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI{

    public partial class QuestDialogView : RavenhillBaseView {

        public override RavenhillViewType viewType => RavenhillViewType.quest_dialog_view;

        public override bool isModal => true;

        public override int siblingIndex => -11;

        private float closeTimer = -1.0f;

        public class Data {
            public QuestData quest { get; set; }
            public bool isStart { get; set; }
        }

        public override void Setup(object data = null) {
            base.Setup(data);

            Data initData = data as Data;
            if(initData == null ) {
                throw new ArgumentException($"invalid QuestDialogView init data, must be typeof {typeof(Data).FullName}");
            }

            Debug.Log("Show quest dialog....");

            QuestOwnerData owner = resourceService.GetQuestOwner(initData.quest.ownerId);

            if(owner != null ) {
                ownerImage.overrideSprite = resourceService.GetSprite(owner, ravenhillGameModeService.roomMode);
            } else {
                ownerImage.overrideSprite = resourceService.transparent;
            }

            if(initData.isStart ) {
                dialogText.text = resourceService.GetString(initData.quest.startTextId);
            } else {
                dialogText.text = resourceService.GetString(initData.quest.endTextId);
            }

            closeTimer = 0.0f;

            closeTrigger.SetEventTriggerClick((bed) => {
                PointerEventData pointerData = bed as PointerEventData;

                if (pointerData != null && pointerData.pointerCurrentRaycast.gameObject != null) {
                    if (pointerData.pointerCurrentRaycast.gameObject.name == closeTrigger.name) {
                        if (IsAllowClose) {
                            viewService.RemoveView(RavenhillViewType.quest_dialog_view);
                            if(initData.isStart ) {
                                Debug.Log($"Call show quest start view after quest dialog view");
                                viewService.ShowViewWithDelay(RavenhillViewType.quest_start_view, 0.5f, initData.quest);
                            } else {
                                viewService.ShowViewWithDelay(RavenhillViewType.quest_end_view, 0.5f, initData.quest);
                            }
                        }
                    }
                }
            }, engine.GetService<IAudioService>());
        }

        

        public override void Update() {
            base.Update();
            if(closeTimer < closeTimeout ) {
                closeTimer += Time.deltaTime;
            }
        }

        private bool IsAllowClose => closeTimer >= closeTimeout;
    }

    public partial class QuestDialogView : RavenhillBaseView {

        [SerializeField]
        private Image ownerImage;

        [SerializeField]
        private Text dialogText;

        [SerializeField]
        private EventTrigger closeTrigger;

        [SerializeField]
        private float closeTimeout = 0.8f;

    }
}
