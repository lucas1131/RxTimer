using NUnit.Framework;

namespace RxClock.Tests.Clock
{
    public partial class TimerShould
    {
        [Test]
        public void StopRunningWhenPauseIsCalled()
        {
            GivenTimerIsRunning();
            
            WhenPausing();

            ThenTimerShouldNotBeRunning();
        }
        
        private void WhenPausing() => timer.Pause();
    }
}