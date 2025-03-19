using System;
using TMPro;
using UniRx;
using UnityEngine;
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
        private IDisposable updateTimerObservable;
        private IDisposable onTimerStateChangedObservable;
        private IDisposable onValueChangedObservable;
        private IDisposable onCommitObservable;
        private bool isTimerRunning;

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
            string formatted = timeFormatter.EditFormat(text);
            inputField.text = formatted;
        }

        private void OnCommitFormat(string text)
        {
            string formatted = timeFormatter.CommitFormat(text);
            inputField.text = formatted;
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
                startButton.onClick.RemoveAllListeners();
                startButton.onClick.AddListener(timer.Pause);
                
                stopButton.onClick.RemoveAllListeners();
                stopButton.onClick.AddListener(timer.Stop);
            }
            else
            {
                startButtonIcon.sprite = startIcon;
                stopButtonIcon.sprite = resetIcon;
                
                // When NOT running, left button is RESET and right button is START
                startButton.onClick.RemoveAllListeners();
                startButton.onClick.AddListener(StartTimer);
                
                stopButton.onClick.RemoveAllListeners();
                stopButton.onClick.AddListener(timer.Reset);
            }
            
            inputField.interactable = !isRunning;
        }

        private void StartTimer()
        {
            TimeSpan timeToCount = TimeSpan.Parse(inputField.text);
            timer.Start(timeToCount);
        }

        private void OnTimerFinished(TimerFinishedMessage message)
        {
            if (message.FinishReason == TimerFinishedMessage.Reason.Completed)
            {
                // TODO play alarm audio
                logger.Info("Timer finished successfully");
            }
        }
    }
}
