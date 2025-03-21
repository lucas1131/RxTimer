using System;
using NUnit.Framework;
using RxClock.Clock;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace RxClock.Tests.PlayMode.Clock
{
    public partial class ClockPresenterShould : ZenjectIntegrationTestFixture
    {
        [Inject] private ClockMock clockMock;
        [Inject] private ClockPresenter clockPresenter;
        [Inject(Id = "timeText")] private TMP_Text timeText;
        [Inject(Id = "timeZoneText")] private TMP_Text timeZoneText;

        [SetUp]
        public void SetUp()
        {
            PreInstall();

            Container.BindInterfacesAndSelfTo<ClockMock>().AsSingle();

            InstallText("timeText");
            InstallText("timeZoneText");
            InstallPresenter();

            Container.Inject(this);

            PostInstall();

            ResolveDependencies();
        }

        private void InstallText(string id)
        {
            TMP_Text text = new GameObject().AddComponent<TextMeshProUGUI>();
            Container
                .Bind<TMP_Text>()
                .WithId(id)
                .FromInstance(text)
                .AsTransient();
        }

        private void InstallPresenter()
        {
            ClockPresenter presenterInstance = new GameObject().AddComponent<ClockPresenter>();
            Container
                .Bind<ClockPresenter>()
                .FromInstance(presenterInstance)
                .AsSingle();
        }

        private void ResolveDependencies()
        {
            ResolveTexts();
            ResolvePresenter();
        }

        private void ResolveTexts()
        {
            timeText = Container.ResolveId<TMP_Text>("timeText");
            timeZoneText = Container.ResolveId<TMP_Text>("timeZoneText");
        }

        private void ResolvePresenter()
        {
            clockPresenter = Container.Resolve<ClockPresenter>();
            clockPresenter.Initialize(
                Container.Resolve<ClockMock>(),
                Container.ResolveId<TMP_Text>("timeText"),
                Container.ResolveId<TMP_Text>("timeZoneText"));
        }

        private class ClockMock : IClock
        {
            public ReactiveProperty<DateTime> mockedNow { get; } = new();
            public IReadOnlyReactiveProperty<DateTime> Now => mockedNow;

            public TimeZoneInfo GetTimeZone()
            {
                return TimeZoneInfo.Local;
            }
        }
    }
}