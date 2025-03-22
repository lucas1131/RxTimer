using UnityEngine;
using Zenject;

namespace RxClock.AudioManager
{
    // Very simple audio wrapper just to play one clip, this could become a full audio mixing system
    public class AudioManager : IAudioManager
    {
        private readonly AudioSource audioSource;

        [Inject]
        public AudioManager(AudioSource audioSource)
        {
            this.audioSource = audioSource;
        }
        
        public void PlayOneShot(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
		}
        
        public void Stop()
        {
            audioSource.Stop();
        }

        public bool IsPlaying()
        {
            return audioSource.isPlaying;
        }
    }
}