using System;
using FluentAssertions;
using NUnit.Framework;

namespace RxClock.Tests.PlayMode.Clock
{
    public partial class ClockPresenterShould
    {
        // Format should be (+|-)<Offset>H <TimeZoneDisplayName>
        private const string TimeZoneTextFormat = @"(\+|-)?[0-9]H\s*\w+";

        [Test]
        public void DisplayTimeZoneInfoOnInitialization()
        {
            ThenTimeZoneTextShouldContainTimeZoneOffsetAndName();
        }

        private void ThenTimeZoneTextShouldContainTimeZoneOffsetAndName()
        {
            timeZoneText.text.Should().MatchRegex(TimeZoneTextFormat);
        }
    }
}