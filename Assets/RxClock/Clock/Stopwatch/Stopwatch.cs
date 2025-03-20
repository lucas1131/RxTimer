using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace RxClock.Clock
{
    public class Stopwatch : IStopwatch, IDisposable
    {
        public ReactiveProperty<TimeSpan> TimeCounter { get; } = new ();
        public ReactiveCollection<TimeSpan> Laps { get; } = new ();
        public IReadOnlyReactiveProperty<bool> IsRunning => isRunning;
        
        private readonly ReactiveProperty<bool> isRunning = new ();
        private readonly ReactiveProperty<TimeSpan> currentLapStart = new();
        private readonly ILogger logger;
        private IDisposable updateTimeCounterObservable;

        [Inject]
        public Stopwatch(ILogger logger)
        {
            this.logger = logger;
        }
        
        public void Start()
        {
            if (isRunning.Value)
            {
                logger.Info("Start called when stopwatch is already running");
                return;
            }
            
            logger.Info("Starting stopwatch");
            
            isRunning.Value = true;
            updateTimeCounterObservable = Observable
                .EveryUpdate() 
                .Subscribe(_ => UpdateCounter());
        }

        public void Resume() => Start();

        public void Pause()
        {
            logger.Info("Pausing stopwatch");
            updateTimeCounterObservable?.Dispose();
            updateTimeCounterObservable = null;
            isRunning.Value = false;
        }

        public void Stop()
        {
            logger.Info("Resetting stopwatch");
            Pause();
            TimeCounter.Value = TimeSpan.Zero;
            currentLapStart.Value = TimeSpan.Zero;
            Laps.Clear();
        }

        public void Lap()
        {
            TimeSpan lapTime = TimeCounter.Value - currentLapStart.Value;
            Laps.Add(lapTime);
            currentLapStart.Value = TimeCounter.Value;
            logger.Info($@"Stopwatch lap: {lapTime:hh\:mm\:ss}");
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