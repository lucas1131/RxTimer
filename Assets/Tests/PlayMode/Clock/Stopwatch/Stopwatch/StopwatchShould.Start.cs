using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using FluentAssertions;
using UnityEngine;
using UnityEngine.TestTools;

namespace RxClock.Tests.PlayMode.Clock
{
    public partial class StopwatchShould
    {
        [UnityTest]
        public IEnumerator CountTimeWhenStartIsCalled()
        {
            return UniTask.ToCoroutine(async () =>
            {
                WhenStopwatchIsStarted();

                float deltaTime = await GivenNFramesHavePassed(10);

                ThenCounterShouldBe(deltaTime);
            });
        }

        private async UniTask<float> GivenNFramesHavePassed(int frames)
        {
            float deltaTime = 0;
            for (int i = 0; i < frames; i++)
            {
                await UniTask.Yield();
                deltaTime += Time.deltaTime;
            }

            return deltaTime;
        }

        private void WhenStopwatchIsStarted()
        {
            stopwatch.Start();
        }

        private void ThenCounterShouldBe(float deltaTime)
        {
            stopwatch.TimeCounter.Value
                .Should()
                .BeCloseTo(TimeSpan.FromSeconds(deltaTime), acceptableError);
        }
    }
}