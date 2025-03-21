using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using FluentAssertions;
using UnityEngine.TestTools;

namespace RxClock.Tests.PlayMode.Clock
{
    public partial class SystemClockProviderShould
    {
        [UnityTest]
        public IEnumerator CountSecondsAutomaticallyFromCreation()
        {
            return UniTask.ToCoroutine(async () =>
            {
                TimeSpan oneSecond = GivenSeconds(1);

                DateTime time1 = WhenGettingTimeNow();
                await GivenSecondsPassed(oneSecond);
                DateTime time2 = WhenGettingTimeNow();

                ThenTimeSpanDifferenceShouldBe(time1, time2, oneSecond);
            });
        }

        private TimeSpan GivenSeconds(int seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }

        private async UniTask GivenSecondsPassed(TimeSpan seconds)
        {
            await UniTask.Delay(seconds);
        }

        private DateTime WhenGettingTimeNow()
        {
            return systemClock.Now.Value;
        }

        private void ThenTimeSpanDifferenceShouldBe(DateTime time1, DateTime time2, TimeSpan interval)
        {
            TimeSpan difference = time2 - time1;
            difference.Should().BeCloseTo(interval, acceptableError);
        }
    }
}