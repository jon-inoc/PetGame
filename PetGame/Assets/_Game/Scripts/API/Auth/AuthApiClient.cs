using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class AuthApiClient : MonoBehaviour
{
    private string baseUrl = "http://localhost:5041/api/Auth";

    public IEnumerator Register(string username, string password, System.Action<bool, string> callback)
    {
        var requestData = new RegisterRequest { username = username, password = password };
        string json = JsonUtility.ToJson(requestData);

        using (UnityWebRequest www = new UnityWebRequest(baseUrl + "/register", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
                callback(true, www.downloadHandler.text);
            else
                callback(false, www.error);
        }
    }

    public IEnumerator Login(string username, string password, System.Action<bool, LoginResponse> callback)
    {
        var requestData = new LoginRequest { username = username, password = password };
        string json = JsonUtility.ToJson(requestData);

        using (UnityWebRequest www = new UnityWebRequest(baseUrl + "/login", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                var response = JsonUtility.FromJson<LoginResponse>(www.downloadHandler.text);
                callback(true, response);
            }
            else
            {
                callback(false, null);
            }
        }
    }
}
