using System;
using FluentAssertions;
using NUnit.Framework;

namespace RxClock.Tests.Clock
{
    public partial class StopwatchShould
    {
        [Test]
        public void StopRunningWhenPauseIsCalled()
        {
            GivenStopwatchIsRunning();
            
            WhenPausing();

            ThenStopwatchShouldNotBeRunning();
        }
        
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public void NotClearLapsWhenPausing(int laps)
        {
            GivenStopwatchIsRunning();
            GivenStopwatchHasLappedNTimes(laps);
            
            WhenPausing();

            ThenLapsShouldHaveCount(laps);
        }

        private void WhenPausing() => stopwatch.Pause();
    }
}