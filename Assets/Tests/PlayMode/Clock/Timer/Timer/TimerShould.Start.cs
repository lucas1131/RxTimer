using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using FluentAssertions;
using UnityEngine.TestTools;

namespace RxClock.Tests.PlayMode.Clock
{
    public partial class TimerShould
    {
        [UnityTest]
        public IEnumerator CountTimeWhenStartIsCalled() => UniTask.ToCoroutine(async () =>
        {
            TimeSpan initialTime = GivenTimeToCountInSeconds(10);
            
            WhenTimerIsStarted(initialTime);
            
            ThenCounterShouldBe(initialTime);

            TimeSpan deltaTime = await Seconds(2);

            ThenCounterShouldBe(initialTime - deltaTime);
        });
        
        private TimeSpan GivenTimeToCountInSeconds(float seconds) => TimeSpan.FromSeconds(seconds);

        private async UniTask<TimeSpan> Seconds(float seconds)
        {
            TimeSpan deltaTime = TimeSpan.FromSeconds(seconds) + acceptableError;
            await UniTask.Delay(deltaTime);
            return deltaTime;
        }

        private void WhenTimerIsStarted(TimeSpan timeToCount) => timer.Start(timeToCount);

        private void ThenCounterShouldBe(TimeSpan time)
        {
            timer.RemainingTime.Value
                .Should()
                .BeCloseTo(time, acceptableError);
        }
    }
}