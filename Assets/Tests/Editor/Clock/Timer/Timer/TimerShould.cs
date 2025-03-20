using NUnit.Framework;
using RxClock.Clock;
using UniRx;
using Zenject;

namespace RxClock.Tests.Editor.Clock
{
    [TestFixture]
    public partial class TimerShould : ZenjectUnitTestFixture
    {
        [Inject] private Timer timer;
        [Inject] private ILogger loggerMock;
        
        [SetUp]
        public void SetUp()
        {
            Container.Bind<ILogger>().FromSubstitute().AsSingle();
            Container.Bind<IMessageBroker>().FromSubstitute().AsSingle();
            Container.BindInterfacesAndSelfTo<Timer>().AsSingle();
            Container.Inject(this);
        }
    }
}