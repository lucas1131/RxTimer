using System;

namespace RxClock.Clock
{
    public interface IClockPresenter
    {
        void UpdateTime(DateTime time);
    }
}