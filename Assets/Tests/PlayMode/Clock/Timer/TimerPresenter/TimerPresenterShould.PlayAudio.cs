using NSubstitute;
using NUnit.Framework;
using RxClock.Clock;
using UnityEngine;

namespace RxClock.Tests.PlayMode.Clock
{
    public partial class TimerPresenterShould
    {
        [Test]
        public void PlayAlarmAudioWhenTimerFinishedSuccessfully()
        {
            WhenTimerFinishedWithReason(TimerFinishedMessage.Reason.Completed);

            ThenAudioManagerShouldHavePlayedClip();
        }
        
        [Test]
        public void NotPlayAudioWhenTimerFinishedUnsuccessfully()
        {
            WhenTimerFinishedWithReason(TimerFinishedMessage.Reason.Aborted);
            WhenTimerFinishedWithReason(TimerFinishedMessage.Reason.Unknown);

            ThenAudioManagerShouldNotHavePlayedClip();
        }

        private void WhenTimerFinishedWithReason(TimerFinishedMessage.Reason reason) =>
            messageBroker.Publish(new TimerFinishedMessage(reason));

        private void ThenAudioManagerShouldHavePlayedClip()
        {
            audioManagerMock.Received(1).PlayOneShot(Arg.Any<AudioClip>());
        }

        private void ThenAudioManagerShouldNotHavePlayedClip()
        {
            audioManagerMock.Received(0).PlayOneShot(Arg.Any<AudioClip>());
        }
    }
}