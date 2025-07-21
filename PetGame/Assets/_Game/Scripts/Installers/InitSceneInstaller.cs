using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class InitSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        // Example: Bind a scene-specific service, or leave empty if none
        // You can bind scene load commands here if needed
        SceneManager.LoadScene("MainMenu");
    }

    public override void Start()
    {
        // Example: Load MainMenu after InitLoading finishes
    }
}
