using UnityEngine;
using UnityEngine.InputSystem;

public class FoodPlacer : MonoBehaviour
{
    public FoodPlate foodPlate;

    private PlayerInputActions _input;

    private void Awake()
    {
        _input = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _input.Gameplay.Enable();
        _input.Gameplay.PlaceFood.performed += OnPlaceFood;
    }

    private void OnDisable()
    {
        _input.Gameplay.PlaceFood.performed -= OnPlaceFood;
        _input.Gameplay.Disable();
    }

    private void OnPlaceFood(InputAction.CallbackContext context)
    {
        foodPlate.PlaceFood();
    }
}
