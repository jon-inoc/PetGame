using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameBootstrapper : IInitializable
{
    private readonly DiContainer _container;

    public GameBootstrapper(DiContainer container)
    {
        _container = container;
    }

    public void Initialize()
    {
        Debug.Log("Initializing game...");

        GameObject servicesPrefab = Resources.Load<GameObject>("GameServicesInstaller");
        if (servicesPrefab == null)
        {
            Debug.LogError("GameServicesInstaller prefab not found in Resources!");
            return;
        }

        GameObject instance = Object.Instantiate(servicesPrefab);
        Object.DontDestroyOnLoad(instance);
        _container.InjectGameObject(instance);

        SceneManager.LoadScene("MainMenu");
    }
}
