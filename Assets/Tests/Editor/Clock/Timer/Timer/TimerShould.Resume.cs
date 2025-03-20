using System;
using FluentAssertions;
using NUnit.Framework;

namespace RxClock.Tests.Clock
{
    public partial class TimerShould
    {
        [Test]
        public void StartTimerWithSameRemainingTimeWhenResumeIsCalled()
        {
            GivenTimerHasStarted(TimeSpan.FromMinutes(10));
            GivenTimerHasPaused();
            
            WhenResuming();

            ThenRemainingTimeShouldBe(TimeSpan.FromMinutes(10));
        }

        private void GivenTimerHasPaused() => timer.Pause();
        
        private void WhenResuming() => timer.Resume();
    }
}