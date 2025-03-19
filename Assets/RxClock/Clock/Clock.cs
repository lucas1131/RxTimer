using System;
using UniRx;
using Zenject;

namespace RxClock.Clock
{
    public class Clock : IInitializable, IDisposable
    {
        private IDisposable updateSecond;
        private readonly IClockPresenter clockPresenter;
        private readonly ILogger logger;

        public Clock([Inject] IClockPresenter clockPresenter)
        {
            this.clockPresenter = clockPresenter;
        }

        public void Initialize()
        {
            updateSecond = Observable
                .Interval(TimeSpan.FromSeconds(1))
                .Subscribe(
                    _ => clockPresenter.UpdateTime(DateTime.Now)
                );
        }

        public void Dispose()
        {
            updateSecond?.Dispose();
        }
    }
}
