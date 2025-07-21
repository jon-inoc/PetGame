using Zenject;
using UnityEngine;

public class GameServicesInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<SaveLoadService>().AsSingle().NonLazy();
        // TODO: Bind other global services here later
    }
}
