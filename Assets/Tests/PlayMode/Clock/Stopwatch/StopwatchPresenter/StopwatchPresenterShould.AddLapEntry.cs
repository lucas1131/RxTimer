using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FluentAssertions;
using UnityEngine;
using UnityEngine.TestTools;

namespace RxClock.Tests.PlayMode.Clock
{
    public partial class StopwatchPresenterShould
    {
        [UnityTest]
        public IEnumerator CreateLapEntryEveryTimeStopwatchLaps() => UniTask.ToCoroutine(async () =>
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

        // [UnityTest]
        // public IEnumerator MoveScrollViewToBottomAfterCreatingLapEntry()
        // {
        //     yield return null;
        // }
        
        private async UniTask OneFrame() => await UniTask.NextFrame();

        private void WhenStopwatchLaps(TimeSpan lapTime) =>
            stopwatchMock.mockedLaps.Add(lapTime);

        private void ThenNumberOfLapEntryInstancesShouldBe(int count)
        {
            StubLapEntryPresenterMock[] presenterInstances = scrollViewContentHolder.GetComponentsInChildren<StubLapEntryPresenterMock>();
            presenterInstances.Should().HaveCount(count);
        }
    }
}