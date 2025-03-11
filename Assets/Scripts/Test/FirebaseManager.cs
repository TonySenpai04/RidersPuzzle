using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

[System.Serializable]
public class GameData
{
    public int gold;
    public int level;
    public int gpgs; // thêm biến khác nếu muốn

    public GameData(int gold, int level, int gpgs)
    {
        this.gold = gold;
        this.level = level;
        this.gpgs = gpgs;
    }
}

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;

    private FirebaseAuth auth;
    private DatabaseReference dbReference;
    private FirebaseUser user;

    public bool isFirebaseReady = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeFirebase();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        GameData newData = new GameData(200, 5, 1);
       SaveData(newData);

        LoadData(OnDataLoaded);
    }
    void OnDataLoaded(GameData data)
    {
        if (data != null)
        {
            Debug.Log("Gold: " + data.gold + ", Level: " + data.level + ", GPGS: " + data.gpgs);
        }
    }
    void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var status = task.Result;
            if (status == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                SignInAnonymously();
            }
            else
            {
                Debug.LogError("Firebase dependency error: " + status);
            }
        });
    }

    void SignInAnonymously()
    {
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCompleted && task.Exception == null)
            {
                user = auth.CurrentUser;
                dbReference = FirebaseDatabase.DefaultInstance.RootReference;
                isFirebaseReady = true;
                Debug.Log("Signed in anonymously: " + user.UserId);
            }
            else
            {
                Debug.LogError("Anonymous sign-in failed: " + task.Exception);
            }
        });
    }

    // Gọi để lưu dữ liệu
    public void SaveData(GameData data)
    {
        if (!isFirebaseReady) return;

        string userId = user.UserId;
        string jsonData = JsonUtility.ToJson(data);

        dbReference.Child("users").Child(userId).SetRawJsonValueAsync(jsonData).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Dữ liệu đã lưu.");
            }
            else
            {
                Debug.LogError("Lưu thất bại: " + task.Exception);
            }
        });
    }

    // Gọi để load dữ liệu
    public void LoadData(System.Action<GameData> onDataLoaded)
    {
        if (!isFirebaseReady) return;

        string userId = user.UserId;

        dbReference.Child("users").Child(userId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
            {
                string json = task.Result.GetRawJsonValue();
                GameData data = JsonUtility.FromJson<GameData>(json);
                Debug.Log("Dữ liệu đã load: Gold=" + data.gold + ", Level=" + data.level);
                onDataLoaded?.Invoke(data);
            }
            else
            {
                Debug.LogWarning("Không tìm thấy dữ liệu hoặc lỗi: " + task.Exception);
                onDataLoaded?.Invoke(null);
            }
        });
    }
}
