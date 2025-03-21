using System;
using TMPro;
using UnityEngine;

namespace RxClock.Clock
{
    public class LapEntryPresenter : MonoBehaviour, ILapEntryPresenter
    {
        [SerializeField] private TMP_Text indexText;
        [SerializeField] private TMP_Text lapTimeText;
        [SerializeField] private TMP_Text elapsedTimeText;

        public virtual void Setup(int index, TimeSpan lapTime, TimeSpan elapsedTime)
        {
            indexText.text = (index + 1).ToString();
            lapTimeText.text = lapTime.ToString(@"hh\:mm\:ss\.fff");
            elapsedTimeText.text = elapsedTime.ToString(@"hh\:mm\:ss\.fff");
        }
    }
}