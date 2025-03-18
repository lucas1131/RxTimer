using System;
using TMPro;
using UnityEngine;

namespace RxClock.Clock
{
    public class Clock : MonoBehaviour
    {
        [SerializeField] private TMP_Text timeText;
    
        void Start()
        {
        }

        void Update()
        {
            DateTime timeNow = DateTime.Now;
            timeText.text = timeNow.ToString("HH:mm:ss");
        }
    }
}
