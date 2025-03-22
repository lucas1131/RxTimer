using System;
using NUnit.Framework;
using RxClock.Clock;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RxClock.Tests.PlayMode.Clock
{
    public partial class StopwatchPresenterShould : ZenjectIntegrationTestFixture
    {
        [Inject(Id = "stopwatch_elapsedTimeText")] private TMP_Text elapsedTimeText;
        [Inject(Id = "stopwatch_scrollRect")] private ScrollRect scrollRect;
        [Inject(Id = "stopwatch_scrollViewContentHolder")] private GameObject scrollViewContentHolder;
        [Inject(Id = "stopwatch_stopButton")] private Button stopButton;
        [Inject] private LapEntryPresenter lapEntryPrefab;
        [Inject] private StopwatchMock stopwatchMock;
        [Inject] private StopwatchPresenter stopwatchPresenter;

        [SetUp]
        public void SetUp()
        {
            PreInstall();

            Container.BindInterfacesAndSelfTo<StopwatchMock>().AsSingle();
            Container.Bind<ILogger>().FromSubstitute().AsSingle();

            InstallText("stopwatch_elapsedTimeText");
            InstallButton("stopwatch_startButton");
            InstallButton("stopwatch_stopButton");
            InstallGameObject("stopwatch_scrollViewContentHolder");
            InstallScrollRect("stopwatch_scrollRect");
            InstallLapEntryPresenter();
            InstallPresenter();

            Container.Inject(this);

            PostInstall();

            ResolveDependencies();
        }

        private void InstallText(string id)
        {
            TMP_Text text = new GameObject().AddComponent<TextMeshProUGUI>();
            GenericInstallWithId(id, text);
        }

        private void InstallButton(string id)
        {
            GameObject buttonObject = new();
            buttonObject.AddComponent<Image>(); // Buttons created via editor always have an Image attached
            Button button = buttonObject.AddComponent<Button>();
            GenericInstallWithId(id, button);
        }

        private void InstallGameObject(string id)
        {
            GenericInstallWithId(id, new GameObject());
        }

        private void InstallScrollRect(string id)
        {
            ScrollRect newScrollRect = new GameObject().AddComponent<ScrollRect>();
            GenericInstallWithId(id, newScrollRect);
        }

        private void GenericInstallWithId<T>(string id, T instance)
        {
            Container
                .Bind<T>()
                .WithId(id)
                .FromInstance(instance)
                .AsTransient();
        }

        private void InstallLapEntryPresenter()
        {
            StubLapEntryPresenterMock testableInstance = new GameObject().AddComponent<StubLapEntryPresenterMock>();
            Container
                .Bind<LapEntryPresenter>()
                .To<StubLapEntryPresenterMock>()
                .FromInstance(testableInstance)
                .AsSingle();
        }

        private void InstallPresenter()
        {
            StopwatchPresenter presenterInstance = new GameObject().AddComponent<StopwatchPresenter>();
            Container
                .Bind<StopwatchPresenter>()
                .FromInstance(presenterInstance)
                .AsSingle();
        }

        private void ResolveDependencies()
        {
            ResolveScrollRect();
            ResolvePresenter();
        }

        private void ResolveScrollRect()
        {
            scrollRect = Container.ResolveId<ScrollRect>("stopwatch_scrollRect");

            GameObject viewport = new();
            RectTransform viewportRect = viewport.AddComponent<RectTransform>();
            viewportRect.SetParent(scrollRect.transform);

            GameObject scrollContent = Container.ResolveId<GameObject>("stopwatch_scrollViewContentHolder");
            RectTransform contentTransform = scrollContent.AddComponent<RectTransform>();
            contentTransform.SetParent(viewportRect.transform);

            scrollRect.viewport = viewportRect;
            scrollRect.content = contentTransform;
            scrollRect.vertical = true;
            scrollRect.horizontal = false;
        }

        private void ResolvePresenter()
        {
            stopwatchPresenter = Container.Resolve<StopwatchPresenter>();
            Container.Inject(stopwatchPresenter);
            stopwatchPresenter.Initialize(
                Container.Resolve<ILogger>(),
                Container.Resolve<IStopwatch>(),
                Container.Resolve<LapEntryPresenter>());
        }

        private class StopwatchMock : IStopwatch
        {
            public ReactiveProperty<TimeSpan> mockedTimeCounter { get; } = new();
            public ReactiveCollection<TimeSpan> mockedLaps { get; } = new();
            public ReactiveProperty<bool> mockedIsRunning { get; } = new();
            public IReadOnlyReactiveProperty<TimeSpan> TimeCounter => mockedTimeCounter;
            public IReadOnlyReactiveCollection<TimeSpan> Laps => mockedLaps;
            public IReadOnlyReactiveProperty<bool> IsRunning => mockedIsRunning;

            public void Start()
            {
            }

            public void Resume()
            {
            }

            public void Pause()
            {
            }

            public void Stop()
            {
            }

            public void Lap()
            {
            }
        }

        public class StubLapEntryPresenterMock : LapEntryPresenter
        {
            public override void Setup(int index, TimeSpan lapTime, TimeSpan elapsedTime)
            {
            }
        }
    }
}