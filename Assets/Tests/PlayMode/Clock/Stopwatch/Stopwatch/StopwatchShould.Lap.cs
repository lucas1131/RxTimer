using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using FluentAssertions;
using UnityEngine.TestTools;

namespace RxClock.Tests.PlayMode.Clock
{
    public partial class StopwatchShould
    {
        [UnityTest]
        public IEnumerator SaveEachLapWhenLapping()
        {
            return UniTask.ToCoroutine(async () =>
            {
                await GivenStopwatchHasStarted();
                float firstLap = await GivenNFramesHavePassed(60);

                WhenLapping();
                ThenLapsShouldHaveSize(1);
                ThenLapsTimeShouldBe(TimeSpan.FromSeconds(firstLap));

                // Again
                float secondLap = await GivenNFramesHavePassed(30);

                WhenLapping();

                ThenLapsShouldHaveSize(2);
                ThenLapsTimeShouldBe(TimeSpan.FromSeconds(firstLap), TimeSpan.FromSeconds(secondLap));
                ThenCounterShouldBe(firstLap + secondLap);
            });
        }

        private void WhenLapping()
        {
            stopwatch.Lap();
        }

        private void ThenLapsShouldHaveSize(int count)
        {
            stopwatch.Laps.Should().HaveCount(count);
        }

        private void ThenLapsTimeShouldBe(params TimeSpan[] expectedTimes)
        {
            IEnumerable<Action<TimeSpan>> mapLapTimesWithExpectedTimes = expectedTimes.Select(IndividualLapAssert);
            stopwatch.Laps.Should().SatisfyRespectively(mapLapTimesWithExpectedTimes);
            return;

            Action<TimeSpan> IndividualLapAssert(TimeSpan expectedTime)
            {
                return lapTime => ThenLapShouldBe(lapTime, expectedTime);
            }

            void ThenLapShouldBe(TimeSpan lap, TimeSpan expected)
            {
                lap.Should().BeCloseTo(expected, acceptableError);
            }
        }
    }
}