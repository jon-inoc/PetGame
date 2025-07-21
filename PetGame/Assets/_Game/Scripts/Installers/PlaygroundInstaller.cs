using UnityEngine;
using Zenject;

public class PlaygroundInstaller : MonoInstaller
{
    [Header("Scene References")]
    [SerializeField] private GameObject petPrefab;
    [SerializeField] private PetStateConfig petStateConfig;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject playerControllerPrefab;

    public override void InstallBindings()
    {
        Container.Bind<PlayerModel>().AsSingle();
        Container.Bind<PetStateConfig>().FromInstance(petStateConfig).AsSingle();
        Container.BindInstance(petPrefab).WithId("PetPrefab");

        Container.BindFactory<Vector3, PetController, PetFactory>()
            .FromFactory<CustomPetFactory>();

        Container.Bind<Camera>().WithId("MainCamera").FromInstance(mainCamera);

        Container.BindInterfacesAndSelfTo<PlayerController>()
            .FromComponentInNewPrefab(playerControllerPrefab)
            .AsSingle()
            .NonLazy();
    }
}
