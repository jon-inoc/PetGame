using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuInstaller : MonoInstaller
{
    [SerializeField] private Button btnStartGame;
    [SerializeField] private Button btnOptions;
    [SerializeField] private GameObject optionsPanel;

    public override void InstallBindings()
    {
        Container.Bind<Button>().WithId("StartButton").FromInstance(btnStartGame);
        Container.Bind<Button>().WithId("OptionsButton").FromInstance(btnOptions);
        Container.Bind<GameObject>().WithId("OptionsPanel").FromInstance(optionsPanel);

        Container.BindInterfacesAndSelfTo<MainMenuController>().AsSingle();
    }
}