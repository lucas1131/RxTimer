using FluentAssertions;
using NUnit.Framework;

namespace RxClock.Tests.Editor.Clock
{
    public partial class StopwatchShould
    {
        [Test]
        public void StartStopwatchWhenResumeIsCalled()
        {
            GivenStopwatchIsStopped();
            
            WhenResuming();

            ThenStopwatchShouldBeRunning();
        }
        
        private void WhenResuming() => stopwatch.Resume();
    }
}