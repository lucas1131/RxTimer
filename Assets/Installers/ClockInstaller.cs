using RxClock.Clock;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ClockInstaller", menuName = "Installers/ClockInstaller")]
public class ClockInstaller : ScriptableObjectInstaller<ClockInstaller>
{
    // ReSharper disable once UnassignedField.Global ScriptableObject serialized
    public bool UseOnlineTime;
    
    public override void InstallBindings()
    {
        if (UseOnlineTime)
        {
            // This is just as a proof of concept, for now online time is not implemented
            Container.BindInterfacesAndSelfTo<NetworkClockProvider>().AsSingle();
        }
        else
        {
            Container.BindInterfacesAndSelfTo<SystemClockProvider>().AsSingle();
        }

        Container.BindInterfacesAndSelfTo<TimerInputFormatter>().AsSingle(); // If we had multiple formats this could use .WithId and later select which formatter to use
        Container.BindInterfacesAndSelfTo<Timer>().AsSingle();
        Container.BindInterfacesAndSelfTo<Stopwatch>().AsSingle();
    }
}