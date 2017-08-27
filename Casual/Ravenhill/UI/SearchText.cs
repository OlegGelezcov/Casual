using Casual.Ravenhill.Data;
using Casual.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {

    public class SearchText : GameBehaviour {

#pragma warning disable 0649
        [SerializeField]
        private Text m_Text;

        [SerializeField]
        private Image m_StrokeImage;

        [SerializeField]
        private float m_StokeSpeed = 1.0f;

        [SerializeField]
        private MCEaseType m_EaseType = MCEaseType.Linear;

        [SerializeField]
        private Image m_HightlightImage;

        [SerializeField]
        private GraphicAnimAlpha m_GraphicAnimAlpha;
#pragma warning restore 0649

        private GraphicAnimAlpha graphicAnimAlpha => m_GraphicAnimAlpha;
        private Image highlightImage => m_HightlightImage;
        private Text text => m_Text;
        private Image strokeImage => m_StrokeImage;
        private float strokeSpeed => m_StokeSpeed;
        private MCEaseType easeType => m_EaseType;
        private FloatValueUpdater strokeUpdater { get; } = new FloatValueUpdater();
        public SearchObjectData searchObjectData { get; private set; }

        public void Clear() {
            if(text != null) {
                text.text = string.Empty;
            }
            if(strokeImage != null ) {
                strokeImage.fillAmount = 0.0f;
            }
            if(highlightImage != null ) {
                highlightImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            }
        }

        public void Setup(SearchObjectData data) {
            this.searchObjectData = data;
            Clear();
            var resourceService = engine.GetService<IResourceService>();
            text.text = resourceService.GetString(data.textId);
        }

        public void Stroke() {
            strokeUpdater.Start(0.0f, 1.0f, strokeSpeed, easeType);
            graphicAnimAlpha?.StartAnim(new MCFloatAnimData {
                duration = 1.0f,
                end = 1.0f,
                start = 0.0f,
                endAction = () => { },
                overwriteEasing = new OverwriteEasing { type = MCEaseType.EaseInOutCubic }
            });
        }

        public override void Update() {
            base.Update();
            if(strokeUpdater.isStarted) {
                Tuple<float, bool> result = strokeUpdater.Update();
                if(strokeImage != null ) {
                    strokeImage.fillAmount = result.Item1;
                }
                bool completed = !result.Item2;
                if(completed) {
                    RavenhillEvents.OnSearchTextStroked(this, searchObjectData);
                }
            }
        }
    }
}
