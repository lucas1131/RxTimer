using System;
using NUnit.Framework;
using RxClock.Clock;
using Zenject;

namespace RxClock.Tests.PlayMode.Clock
{
    public partial class StopwatchShould : ZenjectIntegrationTestFixture
    {
        // Playmode tests are really inconsistent with time based behaviors
        private readonly TimeSpan acceptableError = TimeSpan.FromMilliseconds(100f);
        
        [Inject] private Stopwatch stopwatch; 
        
        [SetUp]
        public void SetUp()
        {
            PreInstall();
            
            Container.Bind<ILogger>().FromSubstitute().AsSingle();
            Container.BindInterfacesAndSelfTo<Stopwatch>().AsSingle();
            Container.Inject(this);
            
            PostInstall();
        }
    }
}