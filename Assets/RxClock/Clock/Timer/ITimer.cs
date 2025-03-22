using System;
using UniRx;

namespace RxClock.Clock
{
    public interface ITimer
    {
        IReadOnlyReactiveProperty<TimeSpan> RemainingTime { get; }
        IReadOnlyReactiveProperty<bool> IsRunning { get; }

        void Start(TimeSpan seconds);
        void Resume();
        void Pause();
        void Stop();
        void Reset();
    }
}