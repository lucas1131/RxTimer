using System;
using FluentAssertions;
using NUnit.Framework;

namespace RxClock.Tests.Clock
{
    public partial class SystemClockProviderShould
    {
        [Test]
        public void GetSystemTimeZoneInfo()
        {
            TimeZoneInfo timeZoneInfo = WhenGettingTimeZoneInfo();
            ThenBeEqualToSystemTimeZone(timeZoneInfo, TimeZoneInfo.Local);
        }

        private TimeZoneInfo WhenGettingTimeZoneInfo() => systemClock.GetTimeZone();

        private void ThenBeEqualToSystemTimeZone(TimeZoneInfo timeZoneInfo, TimeZoneInfo expected)
        {
            timeZoneInfo.Should().Be(expected);
        }
    }
}