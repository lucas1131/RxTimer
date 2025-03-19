using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace RxClock.Clock
{
    public class StopwatchPresenter : MonoBehaviour
    {
        [SerializeField] private TMP_Text elapsedTimeText;
        [SerializeField] private LapEntryPresenter lapEntryPrefab;
        [SerializeField] private GameObject scrollViewContent;
        [SerializeField] private ScrollRect scrollRect;
        
        [Header("Buttons")]
        [SerializeField] private Button startButton;
        [SerializeField] private Button stopButton;
        [SerializeField, Tooltip("Right button")] private Image startButtonIcon;
        [SerializeField, Tooltip("Left button")] private Image stopButtonIcon;
        
        [Header("Icons")]
        [SerializeField] private Sprite startIcon;
        [SerializeField] private Sprite stopIcon;
        [SerializeField] private Sprite pauseIcon;
        [SerializeField] private Sprite lapIcon;
        
        private ILogger logger;
        private IStopwatch stopwatch;
        private ITimeInputFormatter timeFormatter;
        private TimeSpan elapsedTime;
        private bool isStopwatchRunning;
        
        // To have a proper Presenter - View layer separation, all these disposables' subscribes should be happening on the View script and this presenter just need to expose the events that triggers them 
        private IDisposable updateStopwatchObservable;
        private IDisposable onStopwatchStateChangedObservable;
        private IDisposable onLapObservable;

        [Inject]
        public void Initialize(ILogger logger, IStopwatch stopwatch)
        {
            this.logger = logger;
            this.stopwatch = stopwatch;
            
            updateStopwatchObservable = stopwatch.TimeCounter
                .Subscribe(UpdateTotalTime);

            onStopwatchStateChangedObservable = stopwatch.IsRunning
                .Subscribe(SetState);

            onLapObservable = stopwatch.Laps
                .ObserveAdd()
                .Subscribe(OnStopwatchLap);
        }
        
        private void OnDestroy()
        {
            updateStopwatchObservable?.Dispose();
            onStopwatchStateChangedObservable?.Dispose();
            onLapObservable?.Dispose();
        }

        private void UpdateTotalTime(TimeSpan totalElapsedTime)
        {
            elapsedTime = totalElapsedTime;
            elapsedTimeText.text = totalElapsedTime.ToString(@"hh\:mm\:ss\.fff");
        }

        private void SetState(bool isRunning)
        {
            if (isRunning)
            {
                startButtonIcon.sprite = pauseIcon;
                stopButtonIcon.sprite = lapIcon;
                
                // When running, left button is STOP and right button is PAUSE
                ReplaceButtonListener(startButton, stopwatch.Pause);
                ReplaceButtonListener(stopButton, stopwatch.Lap);
            }
            else
            {
                startButtonIcon.sprite = startIcon;
                stopButtonIcon.sprite = stopIcon;
                
                // When NOT running, left button is RESET and right button is START
                ReplaceButtonListener(startButton, StartStopwatch);
                ReplaceButtonListener(stopButton, StopStopwatch);
            }
        }

        private static void ReplaceButtonListener(Button button, UnityAction action)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
        }

        private void StartStopwatch()
        {
            logger.Info("Starting stopwatch");
            stopwatch.Start();
        }

        private void StopStopwatch()
        {
            logger.Info("Stopping stopwatch");
            elapsedTimeText.text = TimeSpan.Zero.ToString(@"hh\:mm\:ss\.fff");
            stopwatch.Stop();
            ClearScrollViewContent();
        }

        private void OnStopwatchLap(CollectionAddEvent<TimeSpan> newItem)
        {
            ILapEntryPresenter newLap = Instantiate(lapEntryPrefab, scrollViewContent.transform);
            newLap.Setup(newItem.Index, newItem.Value, elapsedTime);
            logger.Info($@"New lap: (#{newItem.Index}: {newItem.Value:hh\:mm\:ss\.fff})");

            SetScrollRectToBottomAfterLayoutUpdate().Forget();
        }

        private async UniTask SetScrollRectToBottomAfterLayoutUpdate()
        {
            // This needs to happen after one frame so the scroll rect layout is properly updated after entry instantiation
            await UniTask.Yield();
            scrollRect.verticalNormalizedPosition = 0f; 
        }

        private void ClearScrollViewContent()
        {
            // If we expect to have a lot of laps, this should be changed to have an object pool and not re-create and destroy all objects everytime
            foreach (Transform child in scrollViewContent.transform)
            {
                Destroy(child.gameObject);
            }
            
            scrollRect.verticalNormalizedPosition = 1f;
        }
    }
}
