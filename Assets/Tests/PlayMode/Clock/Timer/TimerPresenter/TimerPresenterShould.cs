using System;
using NUnit.Framework;
using RxClock.AudioManager;
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
        [Inject(Id="timer_inputField")] private TMP_InputField inputField;
        [Inject(Id="timer_startButton")] private Button startButton;
        [Inject(Id="timer_stopButton")] private Button stopButton;
        [Inject] private IMessageBroker messageBroker;
        
        [Inject] private IAudioManager audioManagerMock;
        [Inject] private TimerMock timerMock;
        [Inject] private TimerPresenter timerPresenter;

        [SetUp]
        public void SetUp()
        {
            PreInstall();

            Container.BindInterfacesAndSelfTo<TimerMock>().AsSingle();
            Container.BindInterfacesAndSelfTo<TimerInputFormatter>().AsSingle();
            Container.Bind<ILogger>().FromSubstitute().AsSingle();
            Container.Bind<IMessageBroker>().FromInstance(MessageBroker.Default).AsSingle();
            Container.Bind<IAudioManager>().FromSubstitute().AsSingle();

            InstallInputField("timer_inputField");
            InstallButton("timer_startButton");
            InstallButton("timer_stopButton");
            InstallAudioClip("timer_finishedAlert");
            
            Container.Bind<TimerPresenter>().FromNewComponentOnNewGameObject().AsSingle();

            Container.Inject(this);

            PostInstall();

            ResolveDependencies();
        }
        
        private void InstallButton(string id)
        {
            GameObject buttonObject = new();
            buttonObject.AddComponent<Image>(); // Buttons created via editor always have an Image attached
            Button button = buttonObject.AddComponent<Button>();
            GenericInstallWithId(id, button);
        }

        private void InstallInputField(string id)
        {
            GameObject inputFieldObject = new();
            TMP_Text text = inputFieldObject.AddComponent<TextMeshProUGUI>();
            TMP_InputField input = inputFieldObject.AddComponent<TMP_InputField>();
            input.textComponent = text;
            GenericInstallWithId(id, input);
        }
        
        private void InstallAudioClip(string id)
        {
            AudioClip clip = AudioClip.Create(id, 1, 1, 1000, false);
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
                Container.Resolve<IAudioManager>(),
                Container.ResolveId<AudioClip>("timer_finishedAlert"));
        }

        private class TimerMock : ITimer
        {
            public IReadOnlyReactiveProperty<TimeSpan> RemainingTime => mockedRemainingTime;
            public IReadOnlyReactiveProperty<bool> IsRunning => mockedIsRunning;

            public ReactiveProperty<TimeSpan> mockedRemainingTime = new();
            public ReactiveProperty<bool> mockedIsRunning = new();
            
            public void Start(TimeSpan seconds) { }
            public void Resume() { }
            public void Pause() { }
            public void Stop() { }
            public void Reset() { }
        }
    }
}