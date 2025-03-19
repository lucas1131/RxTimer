using System;
using UniRx;
using TMPro;
using UnityEngine;
using Zenject;

namespace RxClock.Clock
{
    public class ClockPresenter : MonoBehaviour, IClockPresenter
    {
        [SerializeField] private TMP_Text timeText;
        private IClock clock;
        private IDisposable updateUI;

        [Inject]
        public void Initialize(IClock clock)
        {
            this.clock = clock;
            updateUI = clock.Now.Subscribe(UpdateTime);
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