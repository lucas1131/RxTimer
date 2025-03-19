using System;
using UnityEngine;
using UnityEngine.UI;

namespace RxClock.TabController
{
    public class TabOption : MonoBehaviour, ITabOption
    {
        [SerializeField] private GameObject content;
        [SerializeField] private Button button;

        public void SetButtonAction(Action action)
        {
            button.onClick.AddListener(() => action?.Invoke());
        }

        public void SetContentActive(bool active)
        {
            content.SetActive(active);
            button.interactable = !active;
        }

        private void OnDestroy()
        {
            button.onClick.RemoveAllListeners();
        }
    }
}