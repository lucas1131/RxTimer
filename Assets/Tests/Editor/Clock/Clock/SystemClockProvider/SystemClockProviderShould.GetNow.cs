using System;
using FluentAssertions;
using NUnit.Framework;

namespace RxClock.Tests.Clock
{
    public partial class SystemClockProviderShould
    {
        [Test]
        public void GetSystemTime()
        {
            DateTime now = WhenGettingTimeNow();
            ShouldBeEqualToSystemTime(now, DateTime.Now);
        }

        private DateTime WhenGettingTimeNow() => systemClock.Now.Value;

        private void ShouldBeEqualToSystemTime(DateTime now, DateTime expected)
        {
            now.Should().BeCloseTo(expected, precision:TimeSpan.FromMilliseconds(10));
        }
    }
}