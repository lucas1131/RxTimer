using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using RxClock.Clock;
using UnityEngine.TestTools;

namespace RxClock.Tests.PlayMode.Clock
{
    public partial class TimerShould
    {
        [UnityTest]
        public IEnumerator PublishEventWhenRemainingTimeReachesZero() => UniTask.ToCoroutine(async () =>
        {
            TimeSpan initialTime = GivenTimeToCountInSeconds(1);
            
            WhenTimerIsStarted(initialTime);
            
            await Seconds(2);

            ThenMessageShouldHaveBeenPublishedWithSuccess();
            ThenTimerShouldBePaused();
        });

        private void ThenMessageShouldHaveBeenPublishedWithSuccess()
        {
            TimerFinishedMessage successMessage = new (TimerFinishedMessage.Reason.Completed);
            messageBrokerMock
                .Received(1)
                .Publish(successMessage);
        }

        private void ThenTimerShouldBePaused()
        {
            timer.IsRunning.Value.Should().BeFalse();
        }
    }
}