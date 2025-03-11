using System.Collections;
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
    public int gpgs;

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

    private void Awake()
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
        // Delay nhẹ để tránh xung đột khi Play Mode khởi động
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(1f); // Delay nhẹ tránh treo
        StartCoroutine(WaitForFirebaseReadyAndStart());
    }

    IEnumerator WaitForFirebaseReadyAndStart()
    {
        float timeout = 10f;
        float elapsed = 0f;

        while (!isFirebaseReady && elapsed < timeout)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (!isFirebaseReady)
        {
            Debug.LogWarning("⏳ Firebase chưa sẵn sàng sau thời gian chờ.");
            yield break;
        }

        Debug.Log("✅ Firebase đã sẵn sàng → Bắt đầu xử lý dữ liệu");

        // Dữ liệu mẫu để test
        GameData sampleData = new GameData(200, 5, 1);

        // Lưu
        SaveData(sampleData);

        // Load lại
        LoadData(OnDataLoaded);
    }

    void OnDataLoaded(GameData data)
    {
        if (data != null)
        {
            Debug.Log($"📥 Dữ liệu đã load: Gold={data.gold}, Level={data.level}, GPGS={data.gpgs}");
        }
        else
        {
            Debug.LogWarning("❌ Không có dữ liệu hoặc load thất bại.");
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
                Debug.LogError("❌ Firebase dependency error: " + status);
                isFirebaseReady = true; // tránh treo nếu lỗi dependency
            }
        });
    }

    void SignInAnonymously()
    {
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Exception == null)
                {
                    user = auth.CurrentUser;
                    dbReference = FirebaseDatabase.DefaultInstance.RootReference;
                    Debug.Log("🔐 Đăng nhập ẩn danh thành công - UserID: " + user.UserId);
                }
                else
                {
                    Debug.LogError("❌ Đăng nhập ẩn danh thất bại: " + task.Exception);
                }

                // Dù thành công hay lỗi cũng không nên treo app
                isFirebaseReady = true;
            }
        });
    }

    public void SaveData(GameData data)
    {
        if (!isFirebaseReady || user == null)
        {
            Debug.LogWarning("⚠️ Firebase chưa sẵn sàng hoặc chưa có user - không thể lưu.");
            return;
        }

        string userId = user.UserId;
        string jsonData = JsonUtility.ToJson(data);

        dbReference.Child("users").Child(userId).SetRawJsonValueAsync(jsonData).ContinueWith(task =>
        {
            if (task.IsCompleted && task.Exception == null)
            {
                Debug.Log("✅ Dữ liệu đã lưu thành công.");
            }
            else
            {
                Debug.LogError("❌ Lỗi khi lưu dữ liệu: " + task.Exception);
            }
        });
    }

    public void LoadData(System.Action<GameData> onDataLoaded)
    {
        if (!isFirebaseReady || user == null)
        {
            Debug.LogWarning("⚠️ Firebase chưa sẵn sàng hoặc chưa có user - không thể load.");
            return;
        }

        string userId = user.UserId;

        dbReference.Child("users").Child(userId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted && task.Exception == null && task.Result.Exists)
            {
                string json = task.Result.GetRawJsonValue();
                GameData data = JsonUtility.FromJson<GameData>(json);
                Debug.Log("📥 Dữ liệu đã load thành công từ Firebase.");
                onDataLoaded?.Invoke(data);
            }
            else
            {
                Debug.LogWarning("❌ Load dữ liệu thất bại hoặc không tồn tại.");
                onDataLoaded?.Invoke(null);
            }
        });
    }
}
