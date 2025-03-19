using System;
using TMPro;
using UnityEngine;

namespace RxClock.Clock
{
    public class ClockPresenter : MonoBehaviour, IClockPresenter
    {
        [SerializeField] private TMP_Text timeText;
        
        public void UpdateTime(DateTime time)
        {
            timeText.text = time.ToString("HH:mm:ss");
        }
    }
}