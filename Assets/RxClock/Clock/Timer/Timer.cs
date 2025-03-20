using System;
using UniRx;
using Zenject;

namespace RxClock.Clock
{
    public class Timer : ITimer, IDisposable
    {
        public ReactiveProperty<TimeSpan> RemainingTimeSeconds { get; } = new ();
        public ReactiveProperty<bool> IsRunning { get; private set; } = new ();

        private IDisposable updateRemainingTimeObservable;
        private readonly TimeSpan interval = TimeSpan.FromSeconds(1); // Granularity here is only 1 second but could also be every frame with EveryUpdate
        private readonly ILogger logger;

        [Inject]
        public Timer(ILogger logger)
        {
            this.logger = logger;
        }
        
        public void Start(TimeSpan timeSpan)
        {
            if (IsRunning.Value)
            {
                logger.Info("Start called when timer is already running");
                return;
            }
            
            logger.Info("Starting timer");
            
            RemainingTimeSeconds.Value = timeSpan;
            
            IsRunning.Value = true;
            updateRemainingTimeObservable = Observable
                .Interval(interval) 
                .Subscribe(_ => UpdateTimer());
        }

        public void Resume() => Start(RemainingTimeSeconds.Value);

        public void Pause()
        {
            logger.Info($@"Stopping timer at {RemainingTimeSeconds.Value:hh\:mm\:ss}");
            updateRemainingTimeObservable?.Dispose();
            updateRemainingTimeObservable = null;
            IsRunning.Value = false;
        }

        public void Stop() => Pause();

        public void Reset()
        {
            logger.Info("Resetting timer");
            Stop();
            RemainingTimeSeconds.Value = TimeSpan.Zero;
        }

        private void UpdateTimer()
        {
            RemainingTimeSeconds.Value -= interval;
            if (RemainingTimeSeconds.Value <= TimeSpan.Zero)
            {
                Pause();
                MessageBroker.Default.Publish(new TimerFinishedMessage(TimerFinishedMessage.Reason.Completed));
            }
        }
        
        public void Dispose()
        {
            updateRemainingTimeObservable?.Dispose();
            RemainingTimeSeconds?.Dispose();
        }
    }
}