using System;

[Serializable]
public class LoginResponse
{
    public bool success;
    public string token;
    public PlayerData playerData;
}
