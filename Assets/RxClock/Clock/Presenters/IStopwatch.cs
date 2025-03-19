using System;
using UniRx;

namespace RxClock.Clock
{
    public interface IStopwatch
    {
        ReactiveProperty<TimeSpan> TimeCounter { get; }
        ReactiveProperty<TimeSpan> CurrentLapStart { get; }
        ReactiveCollection<TimeSpan> Laps { get; }
        ReactiveProperty<bool> IsRunning { get; }
        void Start();
        void Resume();
        void Pause();
        void Stop();
        void Lap();
        void Dispose();
    }
}