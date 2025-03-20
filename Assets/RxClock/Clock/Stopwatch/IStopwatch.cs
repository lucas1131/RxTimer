using System;
using UniRx;

namespace RxClock.Clock
{
    public interface IStopwatch
    {
        ReactiveProperty<TimeSpan> TimeCounter { get; }
        IReadOnlyReactiveProperty<TimeSpan> CurrentLapStart { get; }
        ReactiveCollection<TimeSpan> Laps { get; }
        IReadOnlyReactiveProperty<bool> IsRunning { get; }
        void Start();
        void Resume();
        void Pause();
        void Stop();
        void Lap();
    }
}