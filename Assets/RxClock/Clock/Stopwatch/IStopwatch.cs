using System;
using UniRx;

namespace RxClock.Clock
{
    public interface IStopwatch
    {
        IReadOnlyReactiveProperty<TimeSpan> TimeCounter { get; }
        IReadOnlyReactiveCollection<TimeSpan> Laps { get; }
        IReadOnlyReactiveProperty<bool> IsRunning { get; }
        void Start();
        void Resume();
        void Pause();
        void Stop();
        void Lap();
    }
}