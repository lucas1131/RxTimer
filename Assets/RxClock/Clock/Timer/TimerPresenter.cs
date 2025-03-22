using System;
using ModestTree;
using RxClock.AudioManager;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace RxClock.Clock
{
    public class TimerPresenter : MonoBehaviour
    {
        [SerializeField, InjectOptional(Id="timer_inputField")] private TMP_InputField inputField;

        [Header("Buttons")] 
        [SerializeField, InjectOptional(Id="timer_startButton")] private Button startButton;
        [SerializeField, InjectOptional(Id="timer_stopButton")] private Button stopButton;

        [SerializeField, Tooltip("Right button")] private Image startButtonIcon;
        [SerializeField, Tooltip("Left button")] private Image stopButtonIcon;

        [Header("Icons")] 
        [SerializeField] private Sprite startIcon;
        [SerializeField] private Sprite stopIcon;
        [SerializeField] private Sprite pauseIcon;
        [SerializeField] private Sprite resetIcon;
        
        private bool isTimerRunning;
        private ILogger logger;
        private TimeSpan originalTimeToCount;
        private ITimer timer;
        private ITimerInputFormatter timerFormatter;
        private IAudioManager audioSource;
        private AudioClip timerFinishedAlert;
        
        // To have a proper Presenter - View layer separation, all these disposables' subscribes should be happening on the View script and this presenter just need to expose the events that triggers them 
        private IDisposable updateTimerObservable;
        private IDisposable onTimerStateChangedObservable;
        private IDisposable onValueChangedObservable;
        private IDisposable onCommitObservable;
        private IDisposable onTimerFinished;

        private void OnDestroy()
        {
            Dispose();
        }

        [Inject]
        public void Initialize(ILogger logger, 
            ITimer timer, 
            ITimerInputFormatter timerFormatter,
            IMessageBroker messageBroker,
            IAudioManager audioSource,
            [Inject(Id="timer_finishedAlert")] AudioClip timerFinishedAlert)
        {
            this.logger = logger;
            this.timer = timer;
            this.timerFormatter = timerFormatter;
            this.audioSource = audioSource;
            this.timerFinishedAlert = timerFinishedAlert;
            
            startButtonIcon ??= startButton.GetComponent<Image>();
            stopButtonIcon ??= stopButton.GetComponent<Image>();

            Dispose();
            updateTimerObservable = timer.RemainingTime
                .Subscribe(UpdateTimer);

            onTimerStateChangedObservable = timer.IsRunning
                .Subscribe(SetState);

            inputField.characterLimit = 8; // HH:mm:ss
            onValueChangedObservable = inputField.onValueChanged
                .AsObservable()
                .Subscribe(OnEditFormat);

            onCommitObservable = inputField.onEndEdit
                .AsObservable()
                .Subscribe(OnCommitFormat);

            onTimerFinished = messageBroker
                .Receive<TimerFinishedMessage>()
                .Subscribe(OnTimerFinished);
        }

        private void Dispose()
        {
            updateTimerObservable?.Dispose();
            onTimerStateChangedObservable?.Dispose();
            onValueChangedObservable?.Dispose();
            onCommitObservable?.Dispose();
            onTimerFinished?.Dispose();
        }

        private void OnEditFormat(string text)
        {
            (string format, int caretOffset) = timerFormatter.EditFormat(text);
            inputField.text = format;
            if (inputField.isFocused)
            {
                inputField.stringPosition += caretOffset;
            }
        }

        private void OnCommitFormat(string text)
        {
            string formatted = timerFormatter.CommitFormat(text);
            inputField.text = formatted;
            originalTimeToCount = text.IsEmpty() ? TimeSpan.Zero : TimeSpan.Parse(inputField.text);
        }

        private void UpdateTimer(TimeSpan remainingTime)
        {
            inputField.text = remainingTime.ToString(@"hh\:mm\:ss");
        }

        private void SetState(bool isRunning)
        {
            if (isRunning)
            {
                startButtonIcon.sprite = pauseIcon;
                stopButtonIcon.sprite = stopIcon;

                // When running, left button is STOP and right button is PAUSE
                ReplaceButtonListener(startButton, timer.Pause);
                ReplaceButtonListener(stopButton, StopTimer);
            }
            else
            {
                startButtonIcon.sprite = startIcon;
                stopButtonIcon.sprite = resetIcon;

                // When NOT running, left button is RESET and right button is START
                ReplaceButtonListener(startButton, StartTimer);
                ReplaceButtonListener(stopButton, ResetTimer);
            }

            inputField.interactable = !isRunning;
        }

        private static void ReplaceButtonListener(Button button, UnityAction action)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
        }

        private void StartTimer()
        {
            logger.Info("Starting timer");
            timer.Start(TimeSpan.Parse(inputField.text));
            audioSource.Stop();
        }

        private void RestartTimer()
        {
            logger.Info("Restarting timer");
            inputField.text = originalTimeToCount.ToString(@"hh\:mm\:ss");
            StartTimer();
        }

        private void StopTimer()
        {
            logger.Info("Stopping timer");
            inputField.text = originalTimeToCount.ToString(@"hh\:mm\:ss");
            timer.Stop();
            audioSource.Stop();
        }

        private void ResetTimer()
        {
            logger.Info("Resetting timer");
            inputField.text = "";
            timer.Reset();
            audioSource.Stop();
        }

        private void OnTimerFinished(TimerFinishedMessage message)
        {
            SetState(false);
            if (message.FinishReason == TimerFinishedMessage.Reason.Completed)
            {
                audioSource.PlayOneShot(timerFinishedAlert);
                logger.Info("Timer finished successfully");
                ReplaceButtonListener(startButton, RestartTimer);
                ReplaceButtonListener(stopButton, ResetTimer);
            }
        }
    }
}