using System;
using System.Collections.Generic;
using UnityEngine;

public class PetTriggerSensor : MonoBehaviour
{
    public event Action<GameObject> OnInteractableEnter;
    public event Action<GameObject> OnInteractableExit;

    public event Action<GameObject> OnPetEnter;
    public event Action<GameObject> OnPetExit;

    public event Action<GameObject> OnFoodPlateEnter;
    public event Action<GameObject> OnFoodPlateExit;

    public event Action<GameObject> OnWallEnter;
    public event Action<GameObject> OnWallExit;

    [SerializeField] private string _interactableTag = "Interactable";
    [SerializeField] private string _petTag = "Pet";
    [SerializeField] private string _foodPlateTag = "FoodPlate";
    [SerializeField] private string _wallTag = "Wall";

    private readonly HashSet<GameObject> _interactablesInRange = new();
    private readonly HashSet<GameObject> _petsInRange = new();
    private readonly HashSet<GameObject> _foodPlatesInRange = new();
    private readonly HashSet<GameObject> _wallsInRange = new();

    public IReadOnlyCollection<GameObject> Interactables => _interactablesInRange;
    public IReadOnlyCollection<GameObject> Pets => _petsInRange;
    public IReadOnlyCollection<GameObject> FoodPlates => _foodPlatesInRange;
    public IReadOnlyCollection<GameObject> Walls => _wallsInRange;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_interactableTag))
        {
            D("Interactable detected");
            _interactablesInRange.Add(other.gameObject);
            OnInteractableEnter?.Invoke(other.gameObject);
        }
        else if (other.CompareTag(_petTag))
        {
            _petsInRange.Add(other.gameObject);
            OnPetEnter?.Invoke(other.gameObject);
        }
        else if (other.CompareTag(_foodPlateTag))
        {
            _foodPlatesInRange.Add(other.gameObject);
            OnFoodPlateEnter?.Invoke(other.gameObject);
            D("Food plate detected");
        }
        else if (other.CompareTag(_wallTag))
        {
            _wallsInRange.Add(other.gameObject);
            OnWallEnter?.Invoke(other.gameObject);
            D("Wall detected");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_interactableTag))
        {
            _interactablesInRange.Remove(other.gameObject);
            OnInteractableExit?.Invoke(other.gameObject);
        }
        else if (other.CompareTag(_petTag))
        {
            _petsInRange.Remove(other.gameObject);
            OnPetExit?.Invoke(other.gameObject);
        }
        else if (other.CompareTag(_foodPlateTag))
        {
            _foodPlatesInRange.Remove(other.gameObject);
            OnFoodPlateExit?.Invoke(other.gameObject);
            D("Food plate exited");
        }
        else if (other.CompareTag(_wallTag))
        {
            _wallsInRange.Remove(other.gameObject);
            OnWallExit?.Invoke(other.gameObject);
            D("Wall exited");
        }
    }

    private void D(string message, bool isError = false)
    {
        if (isError)
            Debug.LogError($"<<{gameObject.name}>> <<PetTriggerSensor>> {message}");
        else
            Debug.Log($"<<{gameObject.name}>> <<PetTriggerSensor>> {message}");
    }
}
