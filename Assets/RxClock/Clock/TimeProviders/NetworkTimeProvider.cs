using System;
using UniRx;
using Zenject;

namespace RxClock.Clock
{
    // Online time is not implemented. This is just a proof of concept for injecting online or system time.
    // For now this is just a copy of SystemTimeProvider
    public class NetworkTimeProvider : IClock, IInitializable, IDisposable
    {
        public ReactiveProperty<DateTime> Now { get; } = new ReactiveProperty<DateTime>(DateTime.Now);
        private IDisposable update;
        private readonly ILogger logger;

        public NetworkTimeProvider([Inject] ILogger logger)
        {
            this.logger = logger;
        }

        public void Initialize()
        {
            logger.Info("NetworkTimeProvider initialized (fake online)");
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
