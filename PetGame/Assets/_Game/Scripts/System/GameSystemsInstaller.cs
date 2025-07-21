using UnityEngine;
using Zenject;

public class GameSystemsInstaller : MonoInstaller
{
    [SerializeField] private GameObject gameManagerPrefab;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameManager>()
            .FromComponentInNewPrefab(gameManagerPrefab)
            .AsSingle()
            .NonLazy();
    }
}
