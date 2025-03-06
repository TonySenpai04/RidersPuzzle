using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;


public class GoogleLogin : MonoBehaviour
{
    public TextMeshProUGUI text;
    private static string path = Application.persistentDataPath + "/loginData.json";
    void Start()
    {
        if (LoadLoginState())
        {
            PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        }
    }
    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            Debug.Log("Đăng nhập thành công: " + Social.localUser.userName);
            text.text = "Chào " + Social.localUser.userName;
            SaveLoginState(true);
        }
        else
        {

            Debug.LogError("Đăng nhập thất bại! Lỗi: " );
            text.text = "Đăng nhập thất bại!\nLỗi: " ;
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
        }
    }

    public static void SaveLoginState(bool isLoggedIn)
    {
        LoginData data = new LoginData { isLoggedIn = isLoggedIn };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }

    public static bool LoadLoginState()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            LoginData data = JsonUtility.FromJson<LoginData>(json);
            return data.isLoggedIn;
        }
        return false;
    }
    public void SignIn()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }
}
[System.Serializable]
public class LoginData
{
    public bool isLoggedIn;
}
