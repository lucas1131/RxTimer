using FluentAssertions;
using NSubstitute;
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
        
        [Test]
        public void DoNothingWhenStartIsCalledIfAlreadyRunning()
        {
            GivenStopwatchIsRunning();
                
            WhenStarting();

            ThenStopwatchShouldBeRunning();
            ThenStopwatchShouldLogAboutDoingNothing();
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

        private void ThenStopwatchShouldLogAboutDoingNothing()
        {
            // Asserting for logger calls generally isn't a good test
            loggerMock.Received().Info(Arg.Any<string>()); 
        }
    }
}