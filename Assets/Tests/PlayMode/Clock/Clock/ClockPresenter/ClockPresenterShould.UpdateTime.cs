using System;
using FluentAssertions;
using NUnit.Framework;

namespace RxClock.Tests.PlayMode.Clock
{
    public partial class ClockPresenterShould
    {
        [Test]
        public void UpdateTimeWhenClockValueChanges()
        {
            DateTime newTime = GivenClockNowValueChanged(DateTime.Now.AddMinutes(1));
            
            ThenTimeTextShouldBe(newTime);
            
            newTime = GivenClockNowValueChanged(DateTime.Now.AddMinutes(2));
            
            ThenTimeTextShouldBe(newTime);
        }

        private DateTime GivenClockNowValueChanged(DateTime time)
        {
            clockMock.mockedNow.Value = time;
            return time;
        }

        private void ThenTimeTextShouldBe(DateTime expected)
        {
            timeText.text.Should().Be(expected.ToString(@"HH\:mm\:ss"));
        }
    }
}