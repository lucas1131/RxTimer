using System;
using UniRx;
using Zenject;

namespace RxClock.Clock
{
    // In production this could be a NetworkClockProvider and use online synced time
    public class SystemClockProvider : IClock, IInitializable, IDisposable
    {
        public IReadOnlyReactiveProperty<DateTime> Now => now;
        private ReactiveProperty<DateTime> now { get; } = new (DateTime.Now);
        private IDisposable update;
        private readonly ILogger logger;
        
        public SystemClockProvider([Inject] ILogger logger)
        {
            this.logger = logger;
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
