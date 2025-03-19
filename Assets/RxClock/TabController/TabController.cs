using System.Collections.Generic;
using UnityEngine;

namespace RxClock.TabController
{
    
    public class TabController : MonoBehaviour
    {
        [SerializeField] private List<TabOption> tabs = new();
        private int activeTab;

        private void Start()
        {
            if (tabs.Count == 0)
            {
                foreach (Transform child in transform)
                {
                    tabs.Add(child.gameObject.GetComponent<TabOption>());
                }
            }


            for (int i = 0; i < tabs.Count; i++)
            {
                int index = i;
                tabs[i].SetButtonAction(() => SelectTab(index));
            }
        }

        private void SelectTab(int index)
        {
            for (int i = 0; i < tabs.Count; i++)
            {
                tabs[i].SetContentActive(i == index);
            }
        }
    }
}
