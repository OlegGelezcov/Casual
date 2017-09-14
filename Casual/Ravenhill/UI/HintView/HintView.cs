using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.UI {
    using Casual.Ravenhill.Data;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public partial class HintView : RavenhillBaseView {

        public override RavenhillViewType viewType => RavenhillViewType.hint_view;

        public override bool isModal => false;

        public override int siblingIndex => 55;

        public enum OffsetType : byte { Up, Down, Left, Right }

        public class BaseData {
            public Vector2 screenPosition;
            public OffsetType offsetType = OffsetType.Up;
        }

        public class ItemData : BaseData {
            public InventoryItemData data;
        }

        public class TextData : BaseData {
            public string title = string.Empty;
            public string text = string.Empty;
        }

        private bool isHideAllowed = false;
        private float hideTimer = 0.8f;

        public override void Setup(object objdata = null) {
            base.Setup(objdata);

            BaseData baseData = objdata as BaseData;

            if(objdata is ItemData ) {
                ItemData data = objdata as ItemData;
                nameText.text = resourceService.GetString(data.data.nameId);
                if(data.data.descriptionId.IsValid() ) {
                    descriptionText.text = resourceService.GetString(data.data.descriptionId);
                } else {
                    descriptionText.text = string.Empty;
                }
            } else if(objdata is TextData ) {
                TextData data = objdata as TextData;
                nameText.text = data.title;
                descriptionText.text = data.text;
            } else {
                throw new ArgumentException($"Allowed arguments of type {typeof(ItemData).Name} or {typeof(TextData).Name}");
            }

            Vector2 offset = Vector2.zero;
            switch(baseData.offsetType) {
                case OffsetType.Down: {
                        offset = new Vector2(0, -background.sizeDelta.y * 0.5f);
                    }
                    break;
                case OffsetType.Up: {
                        offset = new Vector2(0, background.sizeDelta.y * 0.5f);
                    }
                    break;
                case OffsetType.Left: {
                        offset = new Vector2(-background.sizeDelta.x * 0.5f, 0.0f);
                    }
                    break;
                case OffsetType.Right: {
                        offset = new Vector2(background.sizeDelta.x * 0.5f, 0.0f);
                    }
                    break;
            }

            background.anchoredPosition = engine.GetService<ICanvasSerive>().TouchPositionToCanvasPosition(baseData.screenPosition) + offset;
            eventTrigger.SetEventTriggerClick(OnEventTrigger);
            if(secondEventTrigger != null ) {
                secondEventTrigger.SetEventTriggerClick(OnEventTrigger);
            }
        }

        private void OnEventTrigger(BaseEventData p) {
            PointerEventData ped = p as PointerEventData;
            if(ped.pointerCurrentRaycast.isValid ) {
                if(ped.GetPointerObjectName() == eventTrigger.name ) {
                    if(isHideAllowed) {
                        viewService.RemoveView(RavenhillViewType.hint_view);
                    }
                } else {
                    if(secondEventTrigger != null ) {
                        if(ped.GetPointerObjectName() == secondEventTrigger.name) {
                            if(isHideAllowed) {
                                viewService.RemoveView(RavenhillViewType.hint_view);
                            }
                        }
                    }
                }
            }
        }

        public override void Update() {
            base.Update();
            if(!isHideAllowed) {
                hideTimer -= Time.deltaTime;
                if(hideTimer <= 0.0f ) {
                    isHideAllowed = true;
                }
            }
        }
    }

    public partial class HintView : RavenhillBaseView {
        [SerializeField]
        private Text nameText;

        [SerializeField]
        private Text descriptionText;

        [SerializeField]
        private EventTrigger eventTrigger;

        [SerializeField]
        private RectTransform background;

        [SerializeField]
        private EventTrigger secondEventTrigger;
    }
}
