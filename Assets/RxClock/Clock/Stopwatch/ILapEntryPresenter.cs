using System;

namespace RxClock.Clock
{
    public interface ILapEntryPresenter
    {
        void Setup(int index, TimeSpan lapTime, TimeSpan elapsedTime);
    }
}