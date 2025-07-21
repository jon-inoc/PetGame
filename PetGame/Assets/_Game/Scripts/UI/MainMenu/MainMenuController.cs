using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class MainMenuController : IInitializable
{
    private readonly Button _startButton;
    private readonly Button _optionsButton;
    private readonly GameObject _optionsPanel;

    [Inject]
    public MainMenuController(
        [Inject(Id = "StartButton")] Button startButton,
        [Inject(Id = "OptionsButton")] Button optionsButton,
        [Inject(Id = "OptionsPanel")] GameObject optionsPanel)
    {
        _startButton = startButton;
        _optionsButton = optionsButton;
        _optionsPanel = optionsPanel;
    }

    public void Initialize()
    {
        Debug.Log("Menu initialized.");
        _startButton.onClick.AddListener(StartGame);
        _optionsButton.onClick.AddListener(ToggleOptionsPanel);

        _optionsPanel.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Playground");
    }

    public void ToggleOptionsPanel()
    {
        _optionsPanel.SetActive(!_optionsPanel.activeSelf);
    }
}

