using UnityEngine;
using Zenject;

public class PetSpawner : MonoBehaviour
{
    [Inject] private PetFactory _factory;

    [SerializeField] private Transform[] spawnPositions;

    void Start()
    {
        foreach (var pos in spawnPositions)
        {
            _factory.Create(pos.position);
        }
    }
}
