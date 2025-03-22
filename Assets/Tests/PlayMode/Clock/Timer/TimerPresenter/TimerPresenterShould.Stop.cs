using System;
using FluentAssertions;
using NUnit.Framework;

namespace RxClock.Tests.PlayMode.Clock
{
    public partial class TimerPresenterShould
    {
        [Test]
        public void ClearTextWhenStopIsClickedGivenTimerIsStopped()
        {
            GivenTimerText("12:34:56");
            GivenTimerIsStopped();
            
            ThenTextShouldBe("12:34:56");

            WhenStopButtonIsClicked();

            ThenTextShouldBeEmpty();
        }

        [Test]
        public void ResetTimerToInitialTimeWhenStopIsClickedGivenTimerIsRunning()
        {
            GivenUserInputTimer("10:00:56");
            GivenTimerIsRunning();
            
            ThenTextShouldBe("10:00:56");

            GivenSecondsPassed(6);

            ThenTextShouldBe("10:00:50");
            
            WhenStopButtonIsClicked();

            ThenTextShouldBe("10:00:56");
        }

        private void GivenTimerText(string text) => inputField.text = text;

        private void GivenUserInputTimer(string timeText)
        {
            inputField.text = timeText;
            inputField.onEndEdit.Invoke(timeText);
            TimeSpan remainingTime = TimeSpan.Parse(timeText);
            timerMock.mockedRemainingTime.Value = remainingTime;
        }
        
        private void GivenTimerIsStopped() => timerMock.mockedIsRunning.Value = false;
        
        private void GivenTimerIsRunning() => timerMock.mockedIsRunning.Value = true;
        
        private void GivenSecondsPassed(float seconds) => 
            timerMock.mockedRemainingTime.Value -= TimeSpan.FromSeconds(seconds);

        private void WhenStopButtonIsClicked() => stopButton.onClick.Invoke();

        private void ThenTextShouldBe(string expected)
        {
            inputField.text.Should().Be(expected);
        }
        
        private void ThenTextShouldBeEmpty()
        {
            inputField.text.Should().BeEmpty();
        }
    }
}