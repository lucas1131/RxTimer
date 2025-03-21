using System;
using UniRx;
using Zenject;

namespace RxClock.Clock
{
    // Online time is not implemented. This is just a proof of concept for injecting online or system time.
    // For now this is just a copy of SystemTimeProvider
    public class NetworkClockProvider : IClock, IInitializable, IDisposable
    {
        public IReadOnlyReactiveProperty<DateTime> Now => now;
        
        private readonly ReactiveProperty<DateTime> now = new(DateTime.Now);
        private readonly ILogger logger;
        
        private IDisposable update;

        public NetworkClockProvider([Inject] ILogger logger)
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
            logger.Info("NetworkTimeProvider initialized (fake online)");

            Dispose();
            update = Observable
                .Interval(TimeSpan.FromSeconds(1))
                .Subscribe(
                    _ => now.Value = DateTime.Now
                );
        }
    }
}