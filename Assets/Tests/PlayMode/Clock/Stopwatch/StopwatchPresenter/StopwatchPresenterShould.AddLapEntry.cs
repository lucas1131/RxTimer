using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using FluentAssertions;
using UnityEngine.TestTools;

namespace RxClock.Tests.PlayMode.Clock
{
    public partial class StopwatchPresenterShould
    {
        [UnityTest]
        public IEnumerator CreateLapEntryEveryTimeStopwatchLaps()
        {
            return UniTask.ToCoroutine(async () =>
            {
                ThenNumberOfLapEntryInstancesShouldBe(0);

                WhenStopwatchLaps(TimeSpan.FromSeconds(1));
                await OneFrame();

                ThenNumberOfLapEntryInstancesShouldBe(1);

                WhenStopwatchLaps(TimeSpan.FromSeconds(1));
                await OneFrame();
                WhenStopwatchLaps(TimeSpan.FromSeconds(2));
                await OneFrame();
                WhenStopwatchLaps(TimeSpan.FromSeconds(3));
                await OneFrame();

                ThenNumberOfLapEntryInstancesShouldBe(4);
            });
        }

        [UnityTest]
        public IEnumerator MoveScrollViewToBottomAfterCreatingLapEntry()
        {
            return UniTask.ToCoroutine(async () =>
            {
                await GivenStopwatchHasLappedNTimes(10);

                ThenScrollViewPositionShouldBe(0f); // Y+ is up Y- is down in unity, so bottom is 0 and top is 1
            });
        }

        private async UniTask OneFrame()
        {
            await UniTask.NextFrame();
        }

        private void WhenStopwatchLaps(TimeSpan lapTime)
        {
            stopwatchMock.mockedLaps.Add(lapTime);
        }

        private void ThenNumberOfLapEntryInstancesShouldBe(int count)
        {
            StubLapEntryPresenterMock[] presenterInstances =
                scrollViewContentHolder.GetComponentsInChildren<StubLapEntryPresenterMock>();
            presenterInstances.Should().HaveCount(count);
        }

        private void ThenScrollViewPositionShouldBe(float position)
        {
            scrollRect.verticalNormalizedPosition.Should().Be(position);
        }
    }
}