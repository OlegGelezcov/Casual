using Casual.Ravenhill.Data;
using Casual.Ravenhill.UI;
using UnityEngine;

namespace Casual.Ravenhill {
    public class VideoService : RavenhillGameBehaviour, IVideoService {


        public void PlayVideo(string id) {
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

                viewService.ShowView(RavenhillViewType.video_view, new VideoView.Data { id = id, completeAction = () => Debug.Log($"vide {id} completed") });

            } else if( Application.isMobilePlatform ){
                Handheld.PlayFullScreenMovie(videoData.streamingAsset);
            }

        }

        public void Setup(object data) {
            
        }
    }
}
