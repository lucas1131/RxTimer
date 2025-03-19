using System;
using UniRx;

namespace RxClock.Clock
{
    public interface ITimer
    {
        ReactiveProperty<TimeSpan> RemainingTimeSeconds { get; }
        ReactiveProperty<bool> IsRunning { get; }
        
        void Start(TimeSpan seconds);
        void Resume();
        void Pause();
        
        // Stop and reset are the same thing but Reset generates an event
        void Stop();
        void Reset();
    }
}