using System;
using NUnit.Framework;
using RxClock.Clock;
using Zenject;

namespace RxClock.Tests.PlayMode.Clock
{
    public partial class SystemClockProviderShould : ZenjectIntegrationTestFixture
    {
        // Playmode tests are really inconsistent with time based behaviors
        private readonly TimeSpan acceptableError = TimeSpan.FromMilliseconds(250); 
        
        [Inject] private SystemClockProvider systemClock; 
        
        [SetUp]
        public void SetUp()
        {
            PreInstall();
            
            Container.Bind<ILogger>().FromSubstitute().AsSingle();
            Container.BindInterfacesAndSelfTo<SystemClockProvider>().AsSingle();
            Container.Inject(this);
            
            PostInstall();
        }
    }
}