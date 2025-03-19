using System;
using UniRx;
using UnityEngine;

namespace RxClock.Clock
{
    public class Stopwatch : IStopwatch, IDisposable
    {
        public ReactiveProperty<TimeSpan> TimeCounter { get; } = new ();
        public ReactiveProperty<TimeSpan> CurrentLapStart { get; private set; } = new ();
        public ReactiveCollection<TimeSpan> Laps { get; } = new ();
        public ReactiveProperty<bool> IsRunning { get; private set; } = new ();

        private IDisposable updateTimeCounterObservable;
        private readonly TimeSpan interval = TimeSpan.FromSeconds(1); // Granularity here is only 1 second but could also be every frame with EveryUpdate
        
        public void Start()
        {
            if (IsRunning.Value)
            {
                return;
            }
            
            IsRunning.Value = true;
            updateTimeCounterObservable = Observable
                .EveryUpdate() 
                .Subscribe(_ => UpdateCounter());
        }

        public void Resume() => Start();

        public void Pause()
        {
            updateTimeCounterObservable?.Dispose();
            updateTimeCounterObservable = null;
            IsRunning.Value = false;
        }

        public void Stop()
        {
            Pause();
            TimeCounter.Value = TimeSpan.Zero;
            CurrentLapStart.Value = TimeSpan.Zero;
            Laps.Clear();
        }

        public void Lap()
        {
            Laps.Add(TimeCounter.Value - CurrentLapStart.Value);
            CurrentLapStart.Value = TimeCounter.Value;
        }
        
        private void UpdateCounter()
        {
            TimeCounter.Value += TimeSpan.FromSeconds(Time.deltaTime);
        }

        public void Dispose()
        {
        }
    }
}