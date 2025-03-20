using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace RxClock.Tests.Editor.Clock
{
    public partial class TimerShould
    {
        [Test]
        public void BeRunningWhenStartIsCalled()
        {
            GivenTimerIsStopped();
                
            WhenStarting(TimeSpan.FromMinutes(10));

            ThenTimerShouldBeRunning();
        }
        
        [Test]
        public void DoNothingWhenStartIsCalledIfAlreadyRunning()
        {
            GivenTimerIsRunning();
                
            WhenStarting(TimeSpan.FromMinutes(10));

            ThenTimerShouldBeRunning();
            ThenTimerShouldLogAboutDoingNothing();
        }
        
        [Test]
        public void DoNothingIfStartIsCalledWithZeroTime()
        {
            GivenTimerIsStopped();
                
            WhenStarting(TimeSpan.Zero);

            ThenTimerShouldNotBeRunning();
            ThenTimerShouldLogAboutDoingNothing();
        }

        private void GivenTimerIsStopped()
        {
            timer.Stop();
        }
        
        private void WhenStarting(TimeSpan timeToCount) => timer.Start(timeToCount);

        private void ThenTimerShouldBeRunning()
        {
            timer.IsRunning.Value.Should().BeTrue();
        }

        private void ThenTimerShouldLogAboutDoingNothing()
        {
            // Asserting for logger calls generally isn't a good test
            loggerMock.Received().Info(Arg.Any<string>()); 
        }
    }
}