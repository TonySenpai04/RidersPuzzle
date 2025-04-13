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
    public Image loadingSpning;
    public bool isLogin;
    private string path => Path.Combine(Application.persistentDataPath, "LoginData.json");
    private async void Start()
    {
        await Task.Delay(2000);
        LoadLoginState();
    }

    private void FixedUpdate()
    {
        UpdateLoginButtonUI();
    }

    private void UpdateLoginButtonUI()
    {
        bool inputEmpty = string.IsNullOrEmpty(emailLogin.text) || string.IsNullOrEmpty(passwordLogin.text);
        loginBtnAcess.interactable = !inputEmpty;
        loginImage.sprite = inputEmpty ? loginUnactive : loginActive;
    }

    public void SaveLoginState()
    {
        LoginData data = new LoginData
        {
            email = emailLogin.text,
            password = passwordLogin.text
        };
        File.WriteAllText(path, JsonUtility.ToJson(data));
    }

    public void LoadLoginState()
    {
        if (!File.Exists(path))
        {
            ShowLoginUI();
            QuestManager.instance.SyncLocalQuestsToFirebaseIfNotExist();
            QuestManager.instance.LoadQuests();
            AchievementManager.instance.SyncLocalQuestsToFirebaseIfNotExist();
            AchievementManager.instance.LoadQuestData();
            return;
        }

        LoginData data = JsonUtility.FromJson<LoginData>(File.ReadAllText(path));
        if (string.IsNullOrEmpty(data.email) || string.IsNullOrEmpty(data.password))
        {
            return;
        }

        emailLogin.text = data.email;
        passwordLogin.text = data.password;
        emailInAccount.text = data.email;
        passwordInAccount.text = data.password;

        Login();
    }

    private void ShowLoginUI()
    {
        userName.text = "";
        loginBtn.gameObject.SetActive(true);
        registerBtn.gameObject.SetActive(true);
        icon.gameObject.SetActive(true);
        accountBtn.gameObject.SetActive(false);
    }

    public void Logout()
    {
        FirebaseAuth.DefaultInstance.SignOut();
        Debug.Log("🚪 Đã đăng xuất");

        if (File.Exists(path)) File.Delete(path);

        ResetToLoginUI();
    }

    private void ResetToLoginUI()
    {
        SoundManager.instance.PlayMusic("Start Screen");
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
        loadingSpning.gameObject.SetActive(true);

        FirebaseDataManager.Instance.Login(emailLogin.text, passwordLogin.text, OnLoginResult);
    }

    private void OnLoginResult(bool success, string errorMessage)
    {
        if (!success)
        {
            errorTextLogin.text = errorMessage;
            loadingSpning.gameObject.SetActive(false);
            return;
        }

        HandleSuccessfulLogin();
    }

    private void HandleSuccessfulLogin()
    {
        LoadUserDataFromFirebase();

        QuestManager.instance.SyncLocalQuestsToFirebaseIfNotExist();
        QuestManager.instance.LoadQuests();
        AchievementManager.instance.SyncLocalQuestsToFirebaseIfNotExist();
        AchievementManager.instance.LoadQuestData();

        SaveLoginState();
        UpdateUIAfterLogin();
    }

    private void LoadUserDataFromFirebase()
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
    }

    private void UpdateUIAfterLogin()
    {
        login.gameObject.SetActive(false);
        loginBtn.gameObject.SetActive(false);
        registerBtn.gameObject.SetActive(false);
        icon.gameObject.SetActive(false);
        accountBtn.gameObject.SetActive(true);
        emailInAccount.text = emailLogin.text;
        passwordInAccount.text = passwordLogin.text;

        StoryManager.instance.UpdateStoryQuantity();
        StoryManager.instance.isLoaded = true;
        loadingSpning.gameObject.SetActive(false);
    }

    public void Register()
    {
        errorTextRegister.text = "";
        loadingSpning.gameObject.SetActive(true);
        FirebaseDataManager.Instance.Register(emailRegister.text, passwordRegister.text, userName.text, OnRegisterResult);
    }

    private void OnRegisterResult(bool success, string message)
    {
        if (!success)
        {
            errorTextRegister.text = message;
            loadingSpning.gameObject.SetActive(false);
            return;
        }

        errorTextRegister.text = "✅ Đăng ký thành công!";
        emailLogin.text = emailRegister.text;
        passwordLogin.text = passwordRegister.text;
        loadingSpning.gameObject.SetActive(false);
        SaveLoginState();
        regester.gameObject.SetActive(false);
        login.gameObject.SetActive(true);

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
        if (string.IsNullOrEmpty(rename.text) || rename.text.Length >= 14)
            return;

        FirebaseDataManager.Instance.username = rename.text;
        FirebaseDataManager.Instance.SaveData(
            LevelManager.instance.GetAllLevelComplete(),
            GoldManager.instance.GetGold(),
            SaveGameManager.instance.LoadAllProgress(),
            HeroManager.instance.GetUnlockHeroID());

        nameText.text = rename.text;
        renameObj.SetActive(false);
    }

    public void ShowAccount()
    {
        if (string.IsNullOrEmpty(emailLogin.text) || string.IsNullOrEmpty(passwordLogin.text))
        {
            accountNotLog.gameObject.SetActive(true);
            return;
        }

        account.gameObject.SetActive(true);
        LoadUserDataFromFirebase();
    }
}
[Serializable]
public class LoginData
{
    public string email;
    public string password;
}

