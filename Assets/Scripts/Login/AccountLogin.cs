using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountLogin : MonoBehaviour
{
    public InputField userNameLogin;
    public InputField passwordLogin;
    public InputField userNameRegister;
    public InputField passwordRegister;
    public TextMeshProUGUI errorTextLogin;
    public TextMeshProUGUI errorTextRegister;
    public GameObject login, regester,panelLogin;
    private string path => Path.Combine(Application.persistentDataPath, "LoginData.json");
    private async void Start()
    {
        await Task.Delay(2000);
        LoadLoginState();
    }
    public void SaveLoginState()
    {
        LoginData data = new LoginData { userName=userNameLogin.text,password=passwordLogin.text };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }

    public void LoadLoginState()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            LoginData data = JsonUtility.FromJson<LoginData>(json);
            this.userNameLogin.text = data.userName;
            this.passwordLogin.text = data.password;
            Login();

        }
 
    }
    public void Login()
    {
        errorTextLogin.text = ""; 
        FirebaseDataManager.Instance.Login(userNameLogin.text, passwordLogin.text, OnLoginResult);
        SaveLoginState();
        panelLogin.gameObject.SetActive(false);

    }

    private void OnLoginResult(bool success, string errorMessage)
    {
        if (!success)
        {
            errorTextLogin.text = errorMessage;
        }
        else
        {
            errorTextRegister.text = "✅ Đăng nhập thành công!";
        }
    }
    public void Register()
    {
        errorTextRegister.text = "";
        FirebaseDataManager.Instance.Register(userNameRegister.text, passwordRegister.text, OnRegisterResult);
    }

    private void OnRegisterResult(bool success, string message)
    {
        if (!success)
        {
            errorTextRegister.text = message;
        }
        else
        {
            errorTextRegister.text = "✅ Đăng ký thành công!";
            login.gameObject.SetActive(true);
            userNameLogin.text = userNameRegister.text;
            passwordLogin.text = passwordRegister.text;
            regester.gameObject.SetActive(false);
        }
    }
}
[Serializable]
public class LoginData {
    public string userName;
    public string password;
}

