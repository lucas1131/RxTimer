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
    public partial class TimerPresenterShould : ZenjectIntegrationTestFixture
    {
        [Inject(Id = "timer_elapsedTimeText")]
        private TMP_Text elapsedTimeText;

        [Inject] private LapEntryPresenter lapEntryPrefab;
        [Inject(Id = "timer_scrollRect")] private ScrollRect scrollRect;

        [Inject(Id = "timer_scrollViewContentHolder")]
        private GameObject scrollViewContentHolder;

        [Inject(Id = "timer_stopButton")] private Button stopButton;
        [Inject] private TimerMock timerMock;
        [Inject] private TimerPresenter timerPresenter;

        [SetUp]
        public void SetUp()
        {
            PreInstall();

            Container.BindInterfacesAndSelfTo<TimerMock>().AsSingle();
            Container.Bind<ILogger>().FromSubstitute().AsSingle();
            Container.Bind<AudioSource>().FromNewComponentOnNewGameObject().AsSingle();

            InstallText("timer_elapsedTimeText");
            InstallButton("timer_startButton");
            InstallButton("timer_stopButton");
            InstallAudioClip("timer_finishedAlert");
            
            Container.Bind<TimerPresenter>().FromNewComponentOnNewGameObject().AsSingle();

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

        private void InstallAudioClip(string id)
        {
            AudioClip clip = AudioClip.Create(id, 10, 10, 10, false);
            GenericInstallWithId(id, clip);
        }

        private void GenericInstallWithId<T>(string id, T instance)
        {
            Container
                .Bind<T>()
                .WithId(id)
                .FromInstance(instance)
                .AsTransient();
        }

        private void ResolveDependencies()
        {
            ResolvePresenter();
        }

        private void ResolvePresenter()
        {
            timerPresenter = Container.Resolve<TimerPresenter>();
            Container.Inject(timerPresenter);
            timerPresenter.Initialize(
                Container.Resolve<ILogger>(),
                Container.Resolve<ITimer>(),
                Container.Resolve<ITimerInputFormatter>(),
                Container.Resolve<IMessageBroker>(),
                Container.Resolve<AudioSource>(),
                Container.Resolve<AudioClip>());
        }

        private class TimerMock : ITimer
        {
            public IReadOnlyReactiveProperty<TimeSpan> RemainingTimeSeconds => mockedRemainingTimeSeconds;
            public IReadOnlyReactiveProperty<bool> IsRunning => mockedIsRunning;

            public ReactiveProperty<TimeSpan> mockedRemainingTimeSeconds = new();
            public ReactiveProperty<bool> mockedIsRunning = new();
            
            public void Start(TimeSpan seconds) { }
            public void Resume() { }
            public void Pause() { }
            public void Stop() { }
            public void Reset() { }
        }
    }
}