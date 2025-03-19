using System;
using UniRx;

namespace RxClock.Clock
{
    public class Timer : ITimer, IDisposable
    {
        public ReactiveProperty<TimeSpan> RemainingTimeSeconds { get; } = new ();
        public ReactiveProperty<bool> IsRunning { get; private set; } = new ();

        private IDisposable updateRemainingTimeObservable;
        private readonly TimeSpan interval = TimeSpan.FromSeconds(1); // Granularity here is only 1 second but could also be every frame with EveryUpdate
        
        public void Start(TimeSpan timeSpan)
        {
            if (IsRunning.Value)
            {
                return;
            }
            
            RemainingTimeSeconds.Value = timeSpan;
            
            IsRunning.Value = true;
            updateRemainingTimeObservable = Observable
                .Interval(interval) 
                .Subscribe(_ => UpdateTimer());
        }

        public void Resume() => Start(RemainingTimeSeconds.Value);

        public void Pause()
        {
            updateRemainingTimeObservable?.Dispose();
            updateRemainingTimeObservable = null;
            IsRunning.Value = false;
        }

        public void Stop()
        {
            Pause();
            RemainingTimeSeconds.Value = TimeSpan.Zero;
        }

        public void Reset()
        {
            Stop();
            MessageBroker.Default.Publish(new TimerFinishedMessage(TimerFinishedMessage.Reason.Aborted));
        }

        public void Dispose()
        {
            updateRemainingTimeObservable?.Dispose();
            RemainingTimeSeconds?.Dispose();
        }

        private void UpdateTimer()
        {
            RemainingTimeSeconds.Value -= interval;
            if (RemainingTimeSeconds.Value <= TimeSpan.Zero)
            {
                Stop();
                MessageBroker.Default.Publish(new TimerFinishedMessage(TimerFinishedMessage.Reason.Completed));
            }
        }
    }
}