using System;
using UniRx;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace RxClock.Clock
{
    public class ClockPresenter : MonoBehaviour, IClockPresenter
    {
        [SerializeField] private TMP_Text timeText;
        [FormerlySerializedAs("timezoneText")] [SerializeField] private TMP_Text timeZoneText;
        private IDisposable updateUI;

        [Inject]
        public void Initialize(IClock clock)
        {
            updateUI = clock.Now.Subscribe(UpdateTime);
            
            TimeZoneInfo timeZoneInfo = clock.GetTimeZone();
            // Comparing local time to UTC makes the offset opposite, so we need to negate it again to get offset from UTC 
            DateTime dstAdjustedTime = TimeZoneInfo.ConvertTime(clock.Now.Value, TimeZoneInfo.Utc); 
            string offsetText = -dstAdjustedTime.Hour >= 0 
                ? $"+{-dstAdjustedTime.Hour}" 
                : $"{-dstAdjustedTime.Hour}";
            
            string timeZoneName = timeZoneInfo.IsDaylightSavingTime(clock.Now.Value) 
                ? timeZoneInfo.DaylightName
                : timeZoneInfo.StandardName;
            
            timeZoneText.text = $"{offsetText}H {timeZoneName}";
            timeZoneText.text = $"{offsetText}H {timeZoneName}";
        }
        
        public void UpdateTime(DateTime time)
        {
            timeText.text = time.ToString("HH:mm:ss");
        }

        private void OnDestroy()
        {
            updateUI.Dispose();
        }
    }
}