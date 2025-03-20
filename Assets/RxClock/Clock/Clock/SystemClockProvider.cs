using System;
using UniRx;
using Zenject;

namespace RxClock.Clock
{
    // In production this could be a NetworkClockProvider and use online synced time
    public class SystemClockProvider : IClock, IInitializable, IDisposable
    {
        public ReactiveProperty<DateTime> Now { get; } = new ReactiveProperty<DateTime>(DateTime.Now);
        private IDisposable update;
        private readonly ILogger logger;
        
        public SystemClockProvider([Inject] ILogger logger)
        {
            this.logger = logger;
        }

        public void Initialize()
        {
            logger.Info("SystemTimeProvider initialized");
            update = Observable
                .Interval(TimeSpan.FromSeconds(1))
                .Subscribe(
                    _ => Now.Value = DateTime.Now
                );
        }

        public void Dispose()
        {
            update?.Dispose();
        }
        
        public TimeZoneInfo GetTimeZone()
        {
            return TimeZoneInfo.Local;
        }
    }
}
