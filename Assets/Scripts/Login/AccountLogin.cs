using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class AccountLogin : MonoBehaviour
{
    public InputField emailLogin;
    public InputField passwordLogin;
    public InputField emailRegister;
    public InputField passwordRegister;
    public InputField userName;
    public InputField emailInput;
    public TextMeshProUGUI errorTextLogin;
    public TextMeshProUGUI errorTextRegister;
    public GameObject login, regester, panelLogin,renameObj,account,accountNotLog,selectzone,playzone,main;
    public Button loginBtn, registerBtn, accountBtn, loginBtnAcess;
    public Sprite loginActive, loginUnactive;
    public Image loginImage, icon;
    public TextMeshProUGUI forgotPasswordMessageText;
    public InputField emailInAccount;
    public InputField passwordInAccount;
    public InputField rename;
    public TextMeshProUGUI nameText;
    
    private string path => Path.Combine(Application.persistentDataPath, "LoginData.json");
    private async void Start()
    {
        await Task.Delay(2000);
        LoadLoginState();
    }
    public void SaveLoginState()
    {
        LoginData data = new LoginData { email = emailLogin.text, password = passwordLogin.text };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }
    private void FixedUpdate()
    {
        if (emailLogin.text == "" || passwordLogin.text == "")
        {
            loginBtnAcess.interactable = false;
            loginImage.sprite = loginUnactive;
        }
        else
        {
            loginBtnAcess.interactable = true;
            loginImage.sprite = loginActive;
        }
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
            emailInAccount.text = data.email;
            passwordInAccount.text = data.password;

            Login();

        }
        else
        {
            userName.text = "";
            loginBtn.gameObject.SetActive(true);
            registerBtn.gameObject.SetActive(true);
            icon.gameObject.SetActive(true);
            accountBtn.gameObject.SetActive(false);
        }

    }
    public void Logout()
    {
        Firebase.Auth.FirebaseAuth.DefaultInstance.SignOut();
        Debug.Log("🚪 Đã đăng xuất");

        string path = Path.Combine(Application.persistentDataPath, "LoginData.json");
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        main.SetActive(true);
        LevelManager.instance.LoadLocal();
        GoldManager.instance.LoadLocal();
        HeroManager.instance.LoadUnlockHero();
        login.SetActive(true);
        regester.SetActive(false);
        loginBtn.gameObject.SetActive(true);
        registerBtn.gameObject.SetActive(true);
        icon.gameObject.SetActive(true);
        accountBtn.gameObject.SetActive(false);
        passwordLogin.text = "";
        emailLogin.text = "";
        selectzone.SetActive(false);
        playzone.SetActive(false);
    }


    public void Login()
    {
        errorTextLogin.text = "";
        FirebaseDataManager.Instance.Login(emailLogin.text, passwordLogin.text, OnLoginResult);

    }
    public void ShowAccount()
    {
        if (emailLogin.text == "" || passwordLogin.text == "") {
            accountNotLog.gameObject.SetActive(true);
            return;
        }

        account.gameObject.SetActive(true);
        FirebaseUser currentUser = FirebaseDataManager.Instance.GetCurrentUser();
        string uid = currentUser.UserId;
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(uid).Child("playerData")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    PlayerData data = JsonUtility.FromJson<PlayerData>(task.Result.GetRawJsonValue());
                    nameText.text = data.name;
                    emailInAccount.text = emailLogin.text;
                    passwordInAccount.text = passwordLogin.text;
                }
            });

    }
    private void OnLoginResult(bool success, string errorMessage)
    {
        if (!success)
        {
            errorTextLogin.text = errorMessage;
        }
        else
        {
            FirebaseUser currentUser = FirebaseDataManager.Instance.GetCurrentUser();
            string uid = currentUser.UserId;
            FirebaseDatabase.DefaultInstance.GetReference("users").Child(uid).Child("playerData")
                .GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompleted && task.Result.Exists)
                    {
                        PlayerData data = JsonUtility.FromJson<PlayerData>(task.Result.GetRawJsonValue());
                        nameText.text = data.name;
                        emailInAccount.text = emailLogin.text;
                        passwordInAccount.text = passwordLogin.text;
                    }
                });

            SaveLoginState();
            login.gameObject.SetActive(false);
            loginBtn.gameObject.SetActive(false);
            registerBtn.gameObject.SetActive(false);
            icon.gameObject.SetActive(false);
            accountBtn.gameObject.SetActive(true);
            emailInAccount.text = emailLogin.text;
            passwordInAccount.text = passwordLogin.text;
           int a = LevelManager.instance.GetAllLevelComplete();
            StoryManager.instance.count = a / 30;
            errorTextRegister.text = "✅ Đăng nhập thành công!";
        }
    }
    public void Register()
    {
        errorTextRegister.text = "";
        FirebaseDataManager.Instance.Register(emailRegister.text, passwordRegister.text, userName.text, OnRegisterResult);
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
    public void OnClickForgotPassword()
    {
        FirebaseDataManager.Instance.ForgotPassword(emailInput.text, (success, message) =>
        {
            forgotPasswordMessageText.text = message;
        });
    }
    public void Rename()
    {
        if (rename.text == "" || rename.text.Length >= 14)
            return;
        FirebaseDataManager.Instance.username = rename.text;
        FirebaseDataManager.Instance.SaveData(
                LevelManager.instance.GetAllLevelComplete(), GoldManager.instance.GetGold(),
                      SaveGameManager.instance.LoadAllProgress(), HeroManager.instance.GetUnlockHeroID());
        nameText.text = rename.text;
        renameObj.SetActive(false);

    }
}
[Serializable]
public class LoginData
{
    public string email;
    public string password;
}

