using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    [Header("UI References")]
    public InputField usernameInputField;
    public InputField passwordInputField;
    public Button loginButton;
    public GameObject messagePanel;
    public GameObject startGamePanel;
    public Text messageText;

    [Header("Auth Client")]
    public AuthApiClient authApiClient;

    private void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonClicked);
    }

    private void OnLoginButtonClicked()
    {
        string username = usernameInputField.text.Trim();
        string password = passwordInputField.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("Username or password is empty.");
            return;
        }

        StartCoroutine(authApiClient.Login(username, password, (success, response) =>
        {
            if (success && response != null && response.success)
            {
                messagePanel.SetActive(true);
                messageText.text = "Login successful!";
                this.gameObject.SetActive(false);
                startGamePanel.SetActive(true);
                Debug.Log("Token: " + response.token);
                Debug.Log($"Level: {response.playerData.Level}, Coins: {response.playerData.Coins}");

                // TODO: Store token or load game scene here
            }
            else
            {
                messagePanel.SetActive(true);
                messageText.text = "Login failed. Invalid credentials or server error.";
            }
        }));
    }
}
