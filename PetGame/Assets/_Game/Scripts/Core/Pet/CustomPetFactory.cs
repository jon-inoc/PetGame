using UnityEngine;
using Zenject;

public class CustomPetFactory : IFactory<Vector3, PetController>
{
    private readonly DiContainer _container;
    private readonly GameObject _petPrefab;
    private readonly PetStateConfig _petConfig;

    public CustomPetFactory(
        DiContainer container,
        [Inject(Id = "PetPrefab")] GameObject petPrefab,
        PetStateConfig petConfig)
    {
        _container = container;
        _petPrefab = petPrefab;
        _petConfig = petConfig;
    }

    public PetController Create(Vector3 position)
    {
        PetModel model = new PetModel();
        PetViewModel viewModel = new PetViewModel(model);

        GameObject petObj = _container.InstantiatePrefab(_petPrefab, position, Quaternion.identity, null);
        PetAIController aiController = petObj.GetComponent<PetAIController>();

        aiController.Construct(viewModel, _petConfig);

        return petObj.GetComponent<PetController>();
    }
}
