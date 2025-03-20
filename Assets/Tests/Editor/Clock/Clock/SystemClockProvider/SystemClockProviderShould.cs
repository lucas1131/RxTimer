using System;
using NUnit.Framework;
using RxClock.Clock;
using Zenject;

namespace RxClock.Tests.Editor.Clock
{
    [TestFixture]
    public partial class SystemClockProviderShould : ZenjectUnitTestFixture
    {
        private readonly TimeSpan acceptableError = TimeSpan.FromMilliseconds(10);
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