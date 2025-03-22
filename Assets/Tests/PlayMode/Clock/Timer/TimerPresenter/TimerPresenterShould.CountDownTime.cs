using System;
using FluentAssertions;
using NUnit.Framework;

namespace RxClock.Tests.PlayMode.Clock
{
    public partial class TimerPresenterShould
    {
        [Test]
        public void UpdateTimerTextWhenTimerValueChanges()
        {
            TimeSpan newTime = GivenTimerCounterValueChanged(TimeSpan.FromMinutes(1));

            ThenTimeTextShouldBe(newTime);

            newTime = GivenTimerCounterValueChanged(TimeSpan.FromMinutes(2));

            ThenTimeTextShouldBe(newTime);
        }

        private TimeSpan GivenTimerCounterValueChanged(TimeSpan time)
        {
            timerMock.mockedRemainingTimeSeconds.Value = time;
            return time;
        }

        private void ThenTimeTextShouldBe(TimeSpan expected)
        {
            timerText.text.Should().Be(expected.ToString(@"hh\:mm\:ss\.fff"));
        }
    }
}