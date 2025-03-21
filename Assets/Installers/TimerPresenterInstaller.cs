using System;
using RxClock.Clock;
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
            // It would be better to have an audio mixer that could be globally injected instead of audio source, this
            // will cause a bunch of injection conflicts when multiple scripts need to play audio
            Container.Bind<AudioSource>().FromInstance(audioSource).AsSingle(); 
            Container
                .Bind<AudioClip>()
                .WithId("timer_finishedAlert")
                .FromInstance(timerFinishedAlert)
                .AsSingle();
        }
    }
}