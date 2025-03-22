using RxClock.Clock;
using UnityEngine;
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