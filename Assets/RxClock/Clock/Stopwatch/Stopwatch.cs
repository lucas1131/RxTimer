using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace RxClock.Clock
{
    public class Stopwatch : IStopwatch, IDisposable
    {
        public IReadOnlyReactiveProperty<TimeSpan> TimeCounter => timeCounter;
        public IReadOnlyReactiveCollection<TimeSpan> Laps => laps;
        public IReadOnlyReactiveProperty<bool> IsRunning => isRunning;
        
        private readonly ReactiveProperty<TimeSpan> timeCounter= new ();
        private readonly ReactiveCollection<TimeSpan> laps = new ();
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
            
            Dispose();
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
            timeCounter.Value = TimeSpan.Zero;
            currentLapStart.Value = TimeSpan.Zero;
            laps.Clear();
        }

        public void Lap()
        {
            TimeSpan lapTime = TimeCounter.Value - currentLapStart.Value;
            laps.Add(lapTime);
            currentLapStart.Value = TimeCounter.Value;
            logger.Info($@"Stopwatch lap: {lapTime:hh\:mm\:ss}");
        }
        
        private void UpdateCounter()
        {
            timeCounter.Value += TimeSpan.FromSeconds(Time.deltaTime);
        }

        public void Dispose()
        {
            updateTimeCounterObservable?.Dispose();
        }
    }
}