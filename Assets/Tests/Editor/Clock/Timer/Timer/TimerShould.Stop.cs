using System;
using FluentAssertions;
using NUnit.Framework;

namespace RxClock.Tests.Editor.Clock
{
    public partial class TimerShould
    {
        [Test]
        public void StopRunningWhenStopIsCalled()
        {
            GivenTimerIsRunning();
            
            WhenStopping();

            ThenTimerShouldNotBeRunning();
        }
        
        [Test]
        public void NotResetCounterWhenStopping()
        {
            TimeSpan timeToCount = GivenTimerIsRunning();
            
            WhenStopping();

            ThenTimerShouldBe(timeToCount);
        }

        private TimeSpan GivenTimerIsRunning()
        {
            TimeSpan timeToCount = TimeSpan.FromMinutes(10);
            timer.Start(timeToCount);
            return timeToCount;
        }

        private void WhenStopping() => timer.Stop();
        
        private void ThenTimerShouldNotBeRunning()
        {
            timer.IsRunning.Value.Should().BeFalse();
        }

        private void ThenTimerShouldBe(TimeSpan timeToCount)
        {
            timer.RemainingTime.Value.Should().Be(timeToCount);
        }
    }
}