using System;
using UniRx;

namespace RxClock.Clock
{
    public interface IClock
    {
        IReadOnlyReactiveProperty<DateTime> Now { get; }
        TimeZoneInfo GetTimeZone();
    }
}