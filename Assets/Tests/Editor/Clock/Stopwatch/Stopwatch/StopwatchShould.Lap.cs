using FluentAssertions;
using NUnit.Framework;

namespace RxClock.Tests.Clock
{
    public partial class StopwatchShould
    {
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public void AddLapToLapsListWhenLapIsCalled(int laps)
        {
            WhenLappingNTimes(laps);

            ThenLapsShouldHaveCount(laps);
        }

        private void WhenLappingNTimes(int laps)
        {
            for (int i = 0; i < laps; i++)
            {
                WhenLapping();
            }
        }
        
        private void WhenLapping() => stopwatch.Lap();

        private void ThenLapsShouldHaveCount(int expected)
        {
            stopwatch.Laps.Should().HaveCount(expected);
        }
    }
}