using RxClock.Clock;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ClockInstaller", menuName = "Installers/ClockInstaller")]
public class ClockInstaller : ScriptableObjectInstaller<ClockInstaller>
{
    public bool UseOnlineTime;
    
    public override void InstallBindings()
    {
        if (UseOnlineTime)
        {
            // This is just as a proof of concept, for now online time is not implemented
            Container.BindInterfacesAndSelfTo<NetworkTimeProvider>().AsSingle();
        }
        else
        {
            Container.BindInterfacesAndSelfTo<SystemTimeProvider>().AsSingle();
        }
    }
}