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
            ShouldBeEqualToSystemTimeZone(timeZoneInfo, TimeZoneInfo.Local);
        }

        private TimeZoneInfo WhenGettingTimeZoneInfo() => systemClock.GetTimeZone();

        private void ShouldBeEqualToSystemTimeZone(TimeZoneInfo timeZoneInfo, TimeZoneInfo expected)
        {
            timeZoneInfo.Should().Be(expected);
        }
    }
}