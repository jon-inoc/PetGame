using UnityEngine;

public class SaveLoadService
{
    private const string SaveKey = "player";

    public PlayerModel Load()
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            string json = PlayerPrefs.GetString(SaveKey);
            return JsonUtility.FromJson<PlayerModel>(json);
        }

        return new PlayerModel(); // default new player
    }

    public void Save(PlayerModel data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }
}
