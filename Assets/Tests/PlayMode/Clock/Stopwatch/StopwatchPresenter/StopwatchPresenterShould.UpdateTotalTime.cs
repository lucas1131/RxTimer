using System;
using FluentAssertions;
using NUnit.Framework;

namespace RxClock.Tests.PlayMode.Clock
{
    public partial class StopwatchPresenterShould
    {
        [Test]
        public void UpdateTimeWhenStopwatchValueChanges()
        {
            TimeSpan newTime = GivenStopwatchCounterValueChanged(TimeSpan.FromMinutes(1));
            
            ThenTimeTextShouldBe(newTime);
            
            newTime = GivenStopwatchCounterValueChanged(TimeSpan.FromMinutes(2));
            
            ThenTimeTextShouldBe(newTime);
        }

        private TimeSpan GivenStopwatchCounterValueChanged(TimeSpan time)
        {
            stopwatchMock.mockedTimeCounter.Value = time;
            return time;
        }

        private void ThenTimeTextShouldBe(TimeSpan expected)
        {
            elapsedTimeText.text.Should().Be(expected.ToString(@"hh\:mm\:ss\.fff"));
        }
    }
}