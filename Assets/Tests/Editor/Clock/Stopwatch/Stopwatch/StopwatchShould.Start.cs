using FluentAssertions;
using NUnit.Framework;

namespace RxClock.Tests.Clock
{
    public partial class StopwatchShould
    {
        [Test]
        public void BeRunningWhenStartIsCalled()
        {
            GivenStopwatchIsStopped();
                
            WhenStarting();

            ThenStopwatchShouldBeRunning();
        }

        private void GivenStopwatchIsStopped()
        {
            stopwatch.Stop();
        }
        
        private void WhenStarting() => stopwatch.Start();

        private void ThenStopwatchShouldBeRunning()
        {
            stopwatch.IsRunning.Value.Should().BeTrue();
        }
    }
}