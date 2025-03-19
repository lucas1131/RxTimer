using System;
using System.Linq;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace RxClock.Clock
{
    public class TimerPresenter : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        private ITimeInputFormatter timeFormatter;
        private IDisposable onValueChangedObservable;

        [Inject]
        public void Initialize(ITimeInputFormatter timeFormatter)
        {
            this.timeFormatter = timeFormatter;
            inputField.characterLimit = 8; // HH:mm:ss
            onValueChangedObservable = inputField.onValueChanged
                .AsObservable()
                .Subscribe(Format);
        }

        private void Format(string text)
        {
            string digits = text.Where(char.IsDigit).ToString(); 
            string formatted = timeFormatter.Format(digits);
            inputField.text = formatted;
        }
    }
}
