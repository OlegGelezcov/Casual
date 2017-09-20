using UnityEngine;

namespace Casual.Ravenhill {
    public interface IAudioService : IService, IButtonSoundProvider {
        bool IsMusicEnabled { get; set; }
        bool IsSoundEnabled { get; set; }
        void PlaySound(SoundType soundType, bool force = true);
        void PlayMusic(SoundType soundType);
        void PlaySound(SoundType soundType, AudioSource customSource);
        void StopMusic();
        //void PlayButton();
        void PlayViewOpen();
        void PlayViewClose();
    }


}
