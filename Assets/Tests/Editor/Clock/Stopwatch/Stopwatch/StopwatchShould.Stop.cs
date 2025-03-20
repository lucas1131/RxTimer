using System;
using FluentAssertions;
using NUnit.Framework;

namespace RxClock.Tests.Editor.Clock
{
    public partial class StopwatchShould
    {
        [Test]
        public void StopRunningWhenStopIsCalled()
        {
            GivenStopwatchIsRunning();
            
            WhenStopping();

            ThenStopwatchShouldNotBeRunning();
        }
        
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public void ClearLapsWhenStopping(int laps)
        {
            GivenStopwatchIsRunning();
            GivenStopwatchHasLappedNTimes(laps);
            
            WhenStopping();

            ThenLapsShouldHaveCount(0);
        }
        
        [Test]
        public void ResetCounterWhenStopping()
        {
            GivenStopwatchIsRunning();
            
            WhenStopping();

            ThenTimerShouldBeZero();
        }

        private void GivenStopwatchIsRunning() => stopwatch.Start();
        private void GivenStopwatchHasLappedNTimes(int laps)
        {
            for (int i = 0; i < laps; i++)
            {
                GivenStopwatchHasLapped();
            }
        }
        
        private void GivenStopwatchHasLapped() => stopwatch.Lap();
        
        private void WhenStopping() => stopwatch.Stop();

        private void ThenStopwatchShouldNotBeRunning()
        {
            stopwatch.IsRunning.Value.Should().BeFalse();
        }
        
        private void ThenLapsShouldBeCleared()
        {
            stopwatch.Laps.Should().HaveCount(0);
        }

        private void ThenTimerShouldBeZero()
        {
            stopwatch.TimeCounter.Value.Should().Be(TimeSpan.Zero);
        }
    }
}