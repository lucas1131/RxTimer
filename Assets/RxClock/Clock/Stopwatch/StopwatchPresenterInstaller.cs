using RxClock.Clock;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StopwatchPresenterInstaller : MonoInstaller
{
    [SerializeField] private LapEntryPresenter lapEntryPrefab;

    public override void InstallBindings()
    {
        Container
            .BindInterfacesAndSelfTo<LapEntryPresenter>()
            .FromInstance(lapEntryPrefab)
            .AsSingle();
    }
}