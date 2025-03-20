using System;
using UniRx;
using UnityEngine;

namespace RxClock.Clock
{
    public class Stopwatch : IStopwatch, IDisposable
    {
        public ReactiveProperty<TimeSpan> TimeCounter { get; } = new ();
        public ReactiveCollection<TimeSpan> Laps { get; } = new ();
        public IReadOnlyReactiveProperty<bool> IsRunning => isRunning;
        
        private readonly ReactiveProperty<bool> isRunning = new ();
        private readonly ReactiveProperty<TimeSpan> currentLapStart = new();
        private IDisposable updateTimeCounterObservable;
        
        public void Start()
        {
            if (isRunning.Value)
            {
                return;
            }
            
            isRunning.Value = true;
            updateTimeCounterObservable = Observable
                .EveryUpdate() 
                .Subscribe(_ => UpdateCounter());
        }

        public void Resume() => Start();

        public void Pause()
        {
            updateTimeCounterObservable?.Dispose();
            updateTimeCounterObservable = null;
            isRunning.Value = false;
        }

        public void Stop()
        {
            Pause();
            TimeCounter.Value = TimeSpan.Zero;
            currentLapStart.Value = TimeSpan.Zero;
            Laps.Clear();
        }

        public void Lap()
        {
            Laps.Add(TimeCounter.Value - currentLapStart.Value);
            currentLapStart.Value = TimeCounter.Value;
        }
        
        private void UpdateCounter()
        {
            TimeCounter.Value += TimeSpan.FromSeconds(Time.deltaTime);
        }

        public void Dispose()
        {
            updateTimeCounterObservable?.Dispose();
        }
    }
}