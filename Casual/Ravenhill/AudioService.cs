using UnityEngine;

namespace Casual.Ravenhill {
    public class AudioService : RavenhillGameBehaviour, IAudioService, ISaveable {

        [SerializeField]
        private AudioSource musicSource;

        [SerializeField]
        private AudioSource soundSource;


        private bool isMusicEnabled = true;
        private bool isSoundEnabled = true;

        public void Setup(object data) {

        }

        public override void Start() {
            base.Start();
            engine.GetService<ISaveService>().Register(this);
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.GameModeChanged += OnGameModeChanged;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.GameModeChanged -= OnGameModeChanged;
        }

        private void OnGameModeChanged(GameModeName oldGameMode, GameModeName newGameMode ) {
            if(isLoaded) {
                if(newGameMode == GameModeName.map || newGameMode == GameModeName.hallway || newGameMode == GameModeName.search) {
                    if(IsMusicEnabled ) {
                        PlayMusic(GetCurrentContextMusic());
                    }
                }
            }
        }

        public bool IsMusicEnabled {
            get => isMusicEnabled;
            set {
                bool oldValue = isMusicEnabled;
                isMusicEnabled = value;
                if(oldValue != value) {
                    RavenhillEvents.OnMusicStateChanged(value);
                }
                if(!isMusicEnabled) {
                    StopMusic();
                } else {
                    PlayMusic(GetCurrentContextMusic());
                }
            }
        }
        public bool IsSoundEnabled {
            get => isSoundEnabled;
            set {
                bool oldValue = isSoundEnabled;
                isSoundEnabled = value;
                if(oldValue != value ) {
                    RavenhillEvents.OnSoundStateChanged(value);
                }
            }
        }



        private SoundType GetCurrentContextMusic() {
            if(ravenhillGameModeService.gameModeName == GameModeName.search) {
                if(ravenhillGameModeService.searchSession.searchMode == Data.SearchMode.Day) {
                    return SoundType.search_day;
                } else {
                    return SoundType.search_night;
                }
            }
            return SoundType.hallway;
        }

        public void PlayMusic(SoundType soundType) {
            if (IsMusicEnabled) {
                AudioClip targetClip = resourceService.GetAudioClip(soundType);
                if (musicSource.clip == null || (musicSource.clip.name != targetClip.name)) {
                    musicSource.clip = targetClip;
                    musicSource.loop = true;
                    musicSource.Play();
                } else {
                    if(!musicSource.isPlaying) {
                        musicSource.loop = true;
                        musicSource.Play();
                    }
                }
            }
        }

        public void PlayButton() {
            PlaySound(SoundType.button, true);
        }

        public void PlayViewOpen() {
            PlaySound(SoundType.view_open, true);
        }

        public void PlayViewClose() {
            PlaySound(SoundType.view_close, true);
        }

        public void PlaySound(SoundType soundType, bool force = true) {
            if (IsSoundEnabled) {
                if (force) {
                    PlaySoundImpl(soundType);
                } else {
                    StartCoroutine(CorPlaySoundWhenSourceFree(soundType));
                }
            }
        }

        public void PlaySound(SoundType soundType, AudioSource customSource ) {
            if(customSource == null ) {
                PlaySound(soundType, false);
            } else {
                customSource.PlayOneShot(resourceService.GetAudioClip(soundType));
            }
        }

        private System.Collections.IEnumerator CorPlaySoundWhenSourceFree(SoundType soundType) {
            yield return new WaitUntil(() => soundSource.isPlaying == false);
            PlaySoundImpl(soundType);
        }

        private void PlaySoundImpl(SoundType soundType) {
            soundSource.PlayOneShot(resourceService.GetAudioClip(soundType));
        }

        public void StopMusic() {
            musicSource.Stop();
        }


        #region ISaveable
        public string saveId => "audio_service";

        public bool isLoaded { get; private set; }

        public string GetSave() {
            UXMLWriteElement root = new UXMLWriteElement(saveId);
            root.AddAttribute("music_enabled", isMusicEnabled);
            root.AddAttribute("sound_enabled", isSoundEnabled);
            return root.ToString();
        }

        public bool Load(string saveStr) {
            if(saveStr.IsValid()) {
                UXMLDocument document = UXMLDocument.FromXml(saveStr);
                UXMLElement root = document.Element(saveId);
                isMusicEnabled = root.GetBool("music_enabled", true);
                isSoundEnabled = root.GetBool("sound_enabled", true);
                isLoaded = true;
            } else {
                InitSave();
            }
            return isLoaded;
        }

        public void InitSave() {
            isMusicEnabled = true;
            isSoundEnabled = true;
            isLoaded = true;
        }

        public void OnRegister() {
           
        }

        public void OnLoaded() {
            
        } 
        #endregion
    }
}
