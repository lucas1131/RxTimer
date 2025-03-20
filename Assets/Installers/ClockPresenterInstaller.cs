using TMPro;
using UnityEngine;
using Zenject;

public class ClockPresenterInstaller : MonoInstaller
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text timeZoneText;

    public override void InstallBindings()
    {
        Container
            .Bind<TMP_Text>()
            .WithId("timeText")
            .FromInstance(timeText)
            .AsTransient();
        
        Container
            .Bind<TMP_Text>()
            .WithId("timeZoneText")
            .FromInstance(timeZoneText)
            .AsTransient();
    }
}