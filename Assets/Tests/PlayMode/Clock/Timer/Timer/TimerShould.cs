using System;
using NUnit.Framework;
using RxClock.Clock;
using UniRx;
using Zenject;

namespace RxClock.Tests.PlayMode.Clock
{
    public partial class TimerShould : ZenjectIntegrationTestFixture
    {
        // Playmode tests are really inconsistent with time based behaviors
        private readonly TimeSpan acceptableError = TimeSpan.FromMilliseconds(100f);

        [Inject] private Timer timer;
        [Inject] private IMessageBroker messageBrokerMock;

        [SetUp]
        public void SetUp()
        {
            PreInstall();

            Container.Bind<ILogger>().FromSubstitute().AsSingle();
            Container.Bind<IMessageBroker>().FromSubstitute().AsSingle();
            Container.BindInterfacesAndSelfTo<Timer>().AsSingle();
            Container.Inject(this);

            PostInstall();
        }
    }
}