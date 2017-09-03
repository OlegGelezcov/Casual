using Casual.Ravenhill.Data;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

namespace Casual.Ravenhill.UI {
    public partial class VideoView : RavenhillBaseView  {
        public override RavenhillViewType viewType => RavenhillViewType.video_view;

        public override bool isModal => true;

        public override int siblingIndex => 100;

        public class Data {
            public string id;
            public System.Action completeAction;
        }

        private Data data = null;

        public override void Setup(object inData = null) {
            base.Setup(inData);

            data = inData as Data;
            if(data == null ) {
                throw new ArgumentException($"VideoView must have setup parameter {typeof(Data).FullName}");
            }

            eventTrigger.SetEventTriggerClick(p => {
                PointerEventData pointerData = p as PointerEventData;
                if(pointerData != null ) {
                    if(pointerData.GetPointerObjectName() == eventTrigger.name ) {
                        if (data != null) {
                            data.completeAction?.Invoke();
                            viewService.RemoveView(RavenhillViewType.video_view, 0.2f);
                        }
                    }
                }
            });

            VideoData videoData = resourceService.GetVideoData(data.id);
            if(videoData != null ) {
                VideoClip clip = Resources.Load<VideoClip>(videoData.resourceAsset);
                videoPlayer.clip = clip;
                videoPlayer.Play();
            }
        }

        public override void OnEnable() {
            base.OnEnable();
            videoPlayer.loopPointReached += OnVideoCompleted;
        }

        public override void OnDisable() {
            base.OnDisable();
            videoPlayer.loopPointReached -= OnVideoCompleted;
        }

        private void OnVideoCompleted(VideoPlayer source) {
            if(data != null) {
                data.completeAction?.Invoke();
            }
            viewService.RemoveView(RavenhillViewType.video_view, 1.0f);
        }

        
    }

    public partial class VideoView : RavenhillBaseView {

        [SerializeField]
        private VideoPlayer videoPlayer;

        [SerializeField]
        private EventTrigger eventTrigger;
    }
}
