using System;
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
        [SerializeField] private TMP_InputField inputField;
        
        [Header("Buttons")]
        [SerializeField] private Button startButton;
        [SerializeField] private Button stopButton;
        [SerializeField, Tooltip("Right button")] private Image startButtonIcon;
        [SerializeField, Tooltip("Left button")] private Image stopButtonIcon;
        
        [Header("Icons")]
        [SerializeField] private Sprite startIcon;
        [SerializeField] private Sprite stopIcon;
        [SerializeField] private Sprite pauseIcon;
        [SerializeField] private Sprite resetIcon;
        
        private ILogger logger;
        private ITimer timer;
        private ITimeInputFormatter timeFormatter;
        private TimeSpan originalTimeToCount;
        private bool isTimerRunning;
        
        // To have a proper Presenter - View layer separation, all these disposables' subscribes should be happening on the View script and this presenter just need to expose the events that triggers them 
        private IDisposable updateTimerObservable;
        private IDisposable onTimerStateChangedObservable;
        private IDisposable onValueChangedObservable;
        private IDisposable onCommitObservable;

        [Inject]
        public void Initialize(ILogger logger, ITimer timer, ITimeInputFormatter timeFormatter)
        {
            this.logger = logger;
            this.timer = timer;
            this.timeFormatter = timeFormatter;
            
            updateTimerObservable = timer.RemainingTimeSeconds
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

            MessageBroker.Default
                .Receive<TimerFinishedMessage>()
                .Subscribe(OnTimerFinished);
        }

        private void OnDestroy()
        {
            updateTimerObservable?.Dispose();
            onTimerStateChangedObservable?.Dispose();
            onValueChangedObservable?.Dispose();
            onCommitObservable?.Dispose();
        }

        private void OnEditFormat(string text)
        {
            (string format, int caretOffset) = timeFormatter.EditFormat(text);
            inputField.text = format;
            if (inputField.isFocused)
            {
                inputField.stringPosition += caretOffset;
            }
        }

        private void OnCommitFormat(string text)
        {
            string formatted = timeFormatter.CommitFormat(text);
            inputField.text = formatted;
            originalTimeToCount = TimeSpan.Parse(inputField.text);
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
        }
        
        private void ResetTimer()
        {
            logger.Info("Resetting timer");
            inputField.text = "";
            timer.Reset();
        }

        private void OnTimerFinished(TimerFinishedMessage message)
        {
            SetState(false);
            if (message.FinishReason == TimerFinishedMessage.Reason.Completed)
            {
                // TODO play alarm audio
                logger.Info("Timer finished successfully");
                ReplaceButtonListener(startButton, RestartTimer);
                ReplaceButtonListener(stopButton, ResetTimer);
            }
        }
    }
}
