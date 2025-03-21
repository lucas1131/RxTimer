using System;
using UniRx;
using Zenject;

namespace RxClock.Clock
{
    public class Timer : ITimer, IDisposable
    {
        public IReadOnlyReactiveProperty<TimeSpan> RemainingTimeSeconds => remainingTimeSeconds;
        public IReadOnlyReactiveProperty<bool> IsRunning => isRunning;
        
        private readonly ReactiveProperty<TimeSpan> remainingTimeSeconds = new();
        private readonly ReactiveProperty<bool> isRunning = new();
        
        // Granularity here is only 1 second but could also be every frame with EveryUpdate
        private readonly TimeSpan interval = TimeSpan.FromSeconds(1); 
        private readonly ILogger logger;
        private readonly IMessageBroker messageBroker;

        private IDisposable updateRemainingTimeObservable;

        [Inject]
        public Timer(ILogger logger, IMessageBroker messageBroker)
        {
            this.logger = logger;
            this.messageBroker = messageBroker;
        }

        public void Dispose()
        {
            updateRemainingTimeObservable?.Dispose();
            remainingTimeSeconds?.Dispose();
        }

        public void Start(TimeSpan timeSpan)
        {
            if (isRunning.Value)
            {
                logger.Info("Start called when timer is already running");
                return;
            }

            if (timeSpan == TimeSpan.Zero)
            {
                logger.Info("Trying to start timer with 0 seconds, ignoring");
                return;
            }

            logger.Info("Starting timer");

            remainingTimeSeconds.Value = timeSpan;

            isRunning.Value = true;
            
            Dispose();
            updateRemainingTimeObservable = Observable
                .Interval(interval)
                .Subscribe(_ => UpdateTimer());
        }

        public void Resume()
        {
            Start(remainingTimeSeconds.Value);
        }

        public void Pause()
        {
            logger.Info($@"Stopping timer at {remainingTimeSeconds.Value:hh\:mm\:ss}");
            updateRemainingTimeObservable?.Dispose();
            updateRemainingTimeObservable = null;
            isRunning.Value = false;
        }

        public void Stop()
        {
            Pause();
        }

        public void Reset()
        {
            logger.Info("Resetting timer");
            Stop();
            remainingTimeSeconds.Value = TimeSpan.Zero;
        }

        private void UpdateTimer()
        {
            remainingTimeSeconds.Value -= interval;
            if (RemainingTimeSeconds.Value <= TimeSpan.Zero)
            {
                Pause();
                messageBroker.Publish(new TimerFinishedMessage(TimerFinishedMessage.Reason.Completed));
            }
        }
    }
}