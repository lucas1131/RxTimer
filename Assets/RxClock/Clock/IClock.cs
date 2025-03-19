using System;
using UniRx;

namespace RxClock.Clock
{
    public interface IClock
    {
        public ReactiveProperty<DateTime> Now { get; }
    }
}