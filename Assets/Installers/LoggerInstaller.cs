using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "LoggerInstaller", menuName = "Installers/LoggerInstaller")]
public class LoggerInstaller : ScriptableObjectInstaller<LoggerInstaller>
{
    public override void InstallBindings()
    {
        //Could make a #if not shipping then install DebugLogger, else install actual production logger, like firebase
        Container.BindInterfacesAndSelfTo<DebugLogger>().AsSingle();
    }
}