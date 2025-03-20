using NUnit.Framework;
using RxClock.Clock;
using Zenject;

namespace RxClock.Tests.Editor.Clock
{
    [TestFixture]
    public partial class StopwatchShould : ZenjectUnitTestFixture
    {
        [Inject] private Stopwatch stopwatch;
        [Inject] private ILogger loggerMock;
        
        [SetUp]
        public void SetUp()
        {
            Container.Bind<ILogger>().FromSubstitute().AsSingle();
            Container.BindInterfacesAndSelfTo<Stopwatch>().AsSingle();
            Container.Inject(this);
        }
    }
}