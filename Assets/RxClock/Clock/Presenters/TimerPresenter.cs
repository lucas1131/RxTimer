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
        private IDisposable onCommitObservable;

        [Inject]
        public void Initialize(ITimeInputFormatter timeFormatter)
        {
            this.timeFormatter = timeFormatter;
            inputField.characterLimit = 8; // HH:mm:ss
            onValueChangedObservable = inputField.onValueChanged
                .AsObservable()
                .Subscribe(OnEditFormat);

            onCommitObservable = inputField.onEndEdit
                .AsObservable()
                .Subscribe(OnCommitFormat);
        }

        private void OnDestroy()
        {
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
    }
}
