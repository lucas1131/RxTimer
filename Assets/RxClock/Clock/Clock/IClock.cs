using System;
using UniRx;

namespace RxClock.Clock
{
    public interface IClock
    {
        ReactiveProperty<DateTime> Now { get; }
        TimeZoneInfo GetTimeZone();
    }
}