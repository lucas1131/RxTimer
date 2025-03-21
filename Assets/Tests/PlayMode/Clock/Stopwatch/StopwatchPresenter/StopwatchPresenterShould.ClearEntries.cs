using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine.TestTools;

namespace RxClock.Tests.PlayMode.Clock
{
    public partial class StopwatchPresenterShould
    {
        [UnityTest]
        public IEnumerator ClearEntriesWhenStopButtonIsClicked() => UniTask.ToCoroutine(async () =>
        {
            await GivenStopwatchHasLapped(TimeSpan.FromSeconds(1));
            await GivenStopwatchHasLapped(TimeSpan.FromSeconds(1));
            await GivenStopwatchHasLapped(TimeSpan.FromSeconds(1));
            await GivenStopwatchHasLapped(TimeSpan.FromSeconds(1));

            ThenNumberOfLapEntryInstancesShouldBe(4);

            WhenStopButtonIsClicked();
            await OneFrame();
           
            ThenNumberOfLapEntryInstancesShouldBe(0);
        });

        private async UniTask GivenStopwatchHasLapped(TimeSpan lapTime)
        {
            stopwatchMock.mockedLaps.Add(lapTime);
            await OneFrame();
        }
        
        private async UniTask GivenStopwatchHasLappedNTimes(int laps)
        {
            while(--laps > 0) await GivenStopwatchHasLapped(TimeSpan.FromSeconds(1));
        }

        private void WhenStopButtonIsClicked()
        {
            stopButton.onClick.Invoke();
        }
    }
}