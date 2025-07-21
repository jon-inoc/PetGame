using UnityEngine;

public class PetController : MonoBehaviour
{
    private PetViewModel _viewModel;
    private PetModel _model;

    public void Initialize(PetViewModel viewModel, PetModel model)
    {
        _viewModel = viewModel;
        _model = model;
    }

    public PetViewModel ViewModel => _viewModel;
    public PetModel Model => _model;
}
