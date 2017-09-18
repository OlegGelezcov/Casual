using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.Ravenhill {
    public class AudioService : RavenhillGameBehaviour, IAudioService {

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
        }

        public override void OnEnable() {
            base.OnEnable();
        }

        public override void OnDisable() {
            base.OnDisable();
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
            PlaySound(SoundType.button, false);
        }

        public void PlayView() {
            PlaySound(SoundType.view, false);
        }

        public void PlaySound(SoundType soundType, bool force = false) {
            if (IsSoundEnabled) {
                if (force) {
                    PlaySoundImpl(soundType);
                } else {
                    StartCoroutine(CorPlaySoundWhenSourceFree(soundType));
                }
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
    }
}
