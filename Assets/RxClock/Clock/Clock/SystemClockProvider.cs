using System;
using UniRx;
using Zenject;

namespace RxClock.Clock
{
    // In production this could be a NetworkClockProvider and use online synced time
    public class SystemClockProvider : IClock, IInitializable, IDisposable
    {
        public IReadOnlyReactiveProperty<DateTime> Now => now;
        
        private readonly ReactiveProperty<DateTime> now  = new(DateTime.Now);
        private readonly ILogger logger;
        private IDisposable update;

        public SystemClockProvider([Inject] ILogger logger)
        {
            this.logger = logger;
        }

        public TimeZoneInfo GetTimeZone()
        {
            return TimeZoneInfo.Local;
        }

        public void Dispose()
        {
            update?.Dispose();
        }

        public void Initialize()
        {
            logger.Info("SystemTimeProvider initialized");

            Dispose();
            update = Observable
                .Interval(TimeSpan.FromSeconds(1))
                .Subscribe(
                    _ => now.Value = DateTime.Now
                );
        }
    }
}