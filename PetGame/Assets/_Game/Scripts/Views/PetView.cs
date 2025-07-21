using UnityEngine;
using UnityEngine.UI;

public class PetView : MonoBehaviour
{
    [SerializeField] private Slider hungerSlider;
    [SerializeField] private Slider happinessSlider;

    private PetViewModel _viewModel;

    public void Bind(PetViewModel viewModel)
    {
        _viewModel = viewModel;

        _viewModel.OnHungerChanged += UpdateHungerUI;
        _viewModel.OnHappinessChanged += UpdateHappinessUI;

        // Init values
        UpdateHungerUI(_viewModel.Hunger);
        UpdateHappinessUI(_viewModel.Happiness);
    }

    private void UpdateHungerUI(float value) => hungerSlider.value = value / 100f;
    private void UpdateHappinessUI(float value) => happinessSlider.value = value / 100f;
}
