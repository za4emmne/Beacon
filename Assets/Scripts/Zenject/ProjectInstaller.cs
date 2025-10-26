using System.ComponentModel;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private SoundManager soundManagerPrefab;

    public override void InstallBindings()
    {
        // Контейнер создаёт и удерживает экземпляр SoundManager
        Container.Bind<ISoundManager>()
            .To<SoundManager>()
            .FromComponentInNewPrefab(soundManagerPrefab)
            .AsSingle()
            .NonLazy();
    }
}
