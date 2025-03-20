using NUnit.Framework;
using RxClock.Clock;
using Zenject;

namespace RxClock.Tests.Clock
{
    [TestFixture]
    public partial class SystemClockProviderShould : ZenjectUnitTestFixture
    {
        [Inject] private SystemClockProvider systemClock;
        
        [SetUp]
        public void SetUp()
        {
            Container.Bind<ILogger>().FromSubstitute().AsSingle();
            Container.BindInterfacesAndSelfTo<SystemClockProvider>().AsSingle();
            Container.Inject(this);
        }
    }
}