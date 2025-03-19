using System;

namespace RxClock.TabController
{
    public interface ITabOption
    {
        void SetButtonAction(Action action);
        void SetContentActive(bool active);
    }
}