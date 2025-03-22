using RxClock.AudioManager;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class TimerPresenterInstaller : MonoInstaller
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip timerFinishedAlert;
        
        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<AudioManager>()
                .FromNew()
                .AsSingle()
                .WithArguments(audioSource);
            
            Container
                .Bind<AudioClip>()
                .WithId("timer_finishedAlert")
                .FromInstance(timerFinishedAlert)
                .AsSingle();
        }
    }
}