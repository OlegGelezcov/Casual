using Casual.Ravenhill.Data;
using Casual.Ravenhill.UI;
using System;
using UnityEngine;

namespace Casual.Ravenhill {
    public class VideoService : RavenhillGameBehaviour, IVideoService {


        public void PlayVideo(string id, Action completeAction) {
            VideoData videoData = resourceService.GetVideoData(id);
            if(videoData == null ) {
                Debug.LogWarning($"video data {id} not founded");
                return;
            }

            if(Application.platform == RuntimePlatform.WindowsEditor ||
                Application.platform == RuntimePlatform.OSXEditor ||
                Application.platform == RuntimePlatform.LinuxEditor ||
                Application.platform == RuntimePlatform.WindowsPlayer ||
                Application.platform == RuntimePlatform.OSXPlayer ||
                Application.platform == RuntimePlatform.LinuxPlayer ) {

                viewService.ShowView(RavenhillViewType.video_view, 
                    new VideoView.Data {
                        id = id,
                        completeAction = completeAction
                    );

            } else if( Application.isMobilePlatform ){
                Handheld.PlayFullScreenMovie(videoData.streamingAsset, Color.black, FullScreenMovieControlMode.CancelOnInput);
                engine.Cast<RavenhillEngine>().Run(completeAction, 1.0f);
            }

        }


        public void Setup(object data) {
            
        }
    }
}
