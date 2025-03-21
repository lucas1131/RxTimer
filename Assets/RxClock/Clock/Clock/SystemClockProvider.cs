using System;
using UniRx;
using Zenject;

namespace RxClock.Clock
{
    // In production this could be a NetworkClockProvider and use online synced time
    public class SystemClockProvider : IClock, IInitializable, IDisposable
    {
        private readonly ILogger logger;
        private IDisposable update;

        public SystemClockProvider([Inject] ILogger logger)
        {
            this.logger = logger;
        }

        private ReactiveProperty<DateTime> now { get; } = new(DateTime.Now);
        public IReadOnlyReactiveProperty<DateTime> Now => now;

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