using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using FluentAssertions;
using UnityEngine.TestTools;

namespace RxClock.Tests.PlayMode.Clock
{
    public partial class StopwatchShould
    {
        [UnityTest]
        public IEnumerator ResetCounterWhenStopIsCalled() => UniTask.ToCoroutine(async () =>
        {
            await GivenStopwatchHasStarted();
            await GivenNFramesHavePassed(1);

            WhenStopping();

            ThenCounterShouldBeZero();
        });

        private async UniTask GivenStopwatchHasStarted()
        {
            stopwatch.Start();
            await UniTask.NextFrame(); // Wait 1 frame so observable start listening to Update events
            // this isn't good practice, the test shouldn't be adjusted to pass the implemented code.
            // A possible solution would be to explicitly expose a signal from stopwatch to tell when
            // the observable has actually started counting, the downside is that this creates more
            // behaviours to tests.
        }

        private void WhenStopping() => stopwatch.Stop();

        private void ThenCounterShouldBeZero()
        {
            stopwatch.TimeCounter.Value.Should().Be(TimeSpan.Zero);
        }
    }
}