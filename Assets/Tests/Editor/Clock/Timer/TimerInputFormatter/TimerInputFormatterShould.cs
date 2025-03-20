using NUnit.Framework;
using RxClock.Clock;
using Zenject;

namespace RxClock.Tests.Editor.Clock
{
    [TestFixture]
    public partial class TimerInputFormatterShould : ZenjectUnitTestFixture
    {
        [Inject] private TimerInputFormatter formatter;
        
        [SetUp]
        public void SetUp()
        {
            Container.BindInterfacesAndSelfTo<TimerInputFormatter>().AsSingle();
            Container.Inject(this);
        }
    }
}