namespace Casual.Ravenhill {
    public interface IAudioService : IService, IButtonSoundProvider {
        bool IsMusicEnabled { get; set; }
        bool IsSoundEnabled { get; set; }
        void PlaySound(SoundType soundType, bool force = false);
        void PlayMusic(SoundType soundType);
        void StopMusic();
        //void PlayButton();
        void PlayView();
    }


}
