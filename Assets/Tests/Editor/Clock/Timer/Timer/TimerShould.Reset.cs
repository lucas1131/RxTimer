using System;
using FluentAssertions;
using NUnit.Framework;

namespace RxClock.Tests.Editor.Clock
{
    public partial class TimerShould
    {
        [Test]
        public void SetRemainingTimeToZeroWhenResetIsCalled()
        {
            GivenTimerHasStarted(TimeSpan.FromMinutes(10));
            
            WhenResetting();

            ThenRemainingTimeShouldBe(TimeSpan.Zero);
        }
        
        [Test]
        public void StopTimerWhenResetIsCalled()
        {
            GivenTimerHasStarted(TimeSpan.FromMinutes(10));
            
            WhenResetting();

            ThenTimerShouldNotBeRunning();
        }
        
        private void GivenTimerHasStarted(TimeSpan timeToCount) => timer.Start(timeToCount);

        private void WhenResetting() => timer.Reset();

        private void ThenRemainingTimeShouldBe(TimeSpan expected)
        {
            timer.RemainingTime.Value.Should().Be(expected);
        }
    }
}