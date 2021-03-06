﻿using Casual.Ravenhill.Data;
using Casual.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public class InventoryItemShortView : ListItemView<InventoryItem> {

#pragma warning disable 0649
        [SerializeField]
        private Image m_IconImage;

        [SerializeField]
        private Text m_NameText;
#pragma warning restore 0649

        private Image iconImage => m_IconImage;
        private Text nameText => m_NameText;

        public override void Setup(InventoryItem data) {
            base.Setup(data);
            iconImage.overrideSprite = resourceService.GetSprite(data);
            nameText.text = resourceService.GetString(data.data.nameId);
        }
    }
}
