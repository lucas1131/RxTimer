using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "LoggerInstaller", menuName = "Installers/LoggerInstaller")]
public class LoggerInstaller : ScriptableObjectInstaller<LoggerInstaller>
{
    public override void InstallBindings()
    {
    }
}