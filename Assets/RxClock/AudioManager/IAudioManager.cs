using UnityEngine;

namespace RxClock.AudioManager
{
    public interface IAudioManager
    {
        void PlayOneShot(AudioClip clip);
        void Stop();
        bool IsPlaying();
    }
}