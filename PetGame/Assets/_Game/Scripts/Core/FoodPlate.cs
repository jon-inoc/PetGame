using UnityEngine;

public class FoodPlate : MonoBehaviour
{
    [SerializeField] private bool _hasFood = false;

    public bool HasFood => _hasFood;

    public void PlaceFood()
    {
        _hasFood = true;
        Debug.Log("??? Food placed on the plate.");
    }

    public void ConsumeFood()
    {
        if (_hasFood)
        {
            _hasFood = false;
            Debug.Log("?? Pet consumed the food.");
        }
    }
}