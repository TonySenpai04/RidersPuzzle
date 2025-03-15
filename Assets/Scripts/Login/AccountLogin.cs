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
    public InputField emailLogin;
    public InputField passwordLogin;
    public InputField emailRegister;
    public InputField passwordRegister;
    public InputField userName;
    public TextMeshProUGUI errorTextLogin;
    public TextMeshProUGUI errorTextRegister;
    public GameObject login, regester,panelLogin;
    public TextMeshProUGUI nameTxt;
    private string path => Path.Combine(Application.persistentDataPath, "LoginData.json");
    private async void Start()
    {
        await Task.Delay(2000);
        LoadLoginState();
    }
    public void SaveLoginState()
    {
        LoginData data = new LoginData { email= emailLogin.text,password=passwordLogin.text, userName = FirebaseDataManager.Instance.username };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }

    public void LoadLoginState()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            LoginData data = JsonUtility.FromJson<LoginData>(json);
            if (data.email == "" || data.password == "")
                return;
            this.emailLogin.text = data.email;
            this.passwordLogin.text = data.password;
            FirebaseDataManager.Instance.username = data.userName;
            Login();

        }
        else
        {
            userName.text = "";
        }
 
    }
    public void Login()
    {
        errorTextLogin.text = ""; 
        FirebaseDataManager.Instance.Login(emailLogin.text, passwordLogin.text, OnLoginResult);
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
            nameTxt.text = "Hello " + FirebaseDataManager.Instance.username;
            errorTextRegister.text = "✅ Đăng nhập thành công!";
        }
    }
    public void Register()
    {
        errorTextRegister.text = "";
        FirebaseDataManager.Instance.Register(emailRegister.text, passwordRegister.text, userName.text,OnRegisterResult);
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
            emailLogin.text = emailRegister.text;
            passwordLogin.text = passwordRegister.text;
            SaveLoginState();
            regester.gameObject.SetActive(false);
        }
    }
}
[Serializable]
public class LoginData {
    public string email;
    public string password;
    public string userName;
}

