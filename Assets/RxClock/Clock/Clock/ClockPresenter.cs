using System;
using UniRx;
using TMPro;
using UnityEngine;
using Zenject;

namespace RxClock.Clock
{
    public class ClockPresenter : MonoBehaviour
    {
        private TMP_Text timeText;
        private IDisposable updateUI;

        [Inject]
        public void Initialize(IClock clock, 
            [Inject(Id="timeText")] TMP_Text timeText, 
            [Inject(Id="timeZoneText")] TMP_Text timeZoneText)
        {
            this.timeText = timeText;
            updateUI = clock.Now.Subscribe(UpdateTime);
            
            TimeZoneInfo timeZoneInfo = clock.GetTimeZone();
            DateTime dstAdjustedTime = TimeZoneInfo.ConvertTime(clock.Now.Value, TimeZoneInfo.Utc);
            
            int hoursOffset = (clock.Now.Value - dstAdjustedTime).Hours;
            string offsetText = hoursOffset < 0 
                ? $"{hoursOffset}" 
                : $"+{hoursOffset}";
            
            string timeZoneName = timeZoneInfo.IsDaylightSavingTime(clock.Now.Value) 
                ? timeZoneInfo.DaylightName
                : timeZoneInfo.StandardName;
            
            timeZoneText.text = $"{offsetText}H {timeZoneName}";
        }
        
        public void UpdateTime(DateTime time)
        {
            timeText.text = time.ToString("HH:mm:ss");
        }

        private void OnDestroy()
        {
            updateUI?.Dispose();
        }
    }
}