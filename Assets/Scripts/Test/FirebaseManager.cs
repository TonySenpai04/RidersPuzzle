//using System.Collections;
//using UnityEngine;
//using Firebase;
//using Firebase.Auth;
//using Firebase.Database;
//using Firebase.Extensions;
//[System.Serializable]
//public class PlayerData
//{
//    public string playerId;
//    public string playerName;
//    public int level;
//    public int gold;

//    // Constructor có tham số
//    public PlayerData(string id, string name, int level, int gold)
//    {
//        this.playerId = id;
//        this.playerName = name;
//        this.level = level;
//        this.gold = gold;
//    }

//    // Constructor mặc định (bắt buộc nếu dùng Firebase)
//    public PlayerData() { }
//}


//public class FirebaseManager : MonoBehaviour
//{
//    private DatabaseReference dbRef;
//    private FirebaseAuth auth;
//    private FirebaseUser user;

//    private void Start()
//    {
//        InitializeFirebase();
//    }

//    private void InitializeFirebase()
//    {
//        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
//        {
//            if (task.Result == DependencyStatus.Available)
//            {
//                FirebaseApp app = FirebaseApp.DefaultInstance;

//                // ✅ THÊM DÒNG NÀY để tránh lỗi DatabaseURL
//                app.Options.DatabaseUrl = new System.Uri("https://riders-puzzle-default-rtdb.asia-southeast1.firebasedatabase.app/"); // <== THAY ĐÚNG URL CỦA BẠN Ở ĐÂY

//                auth = FirebaseAuth.DefaultInstance;
//                dbRef = FirebaseDatabase.DefaultInstance.RootReference;

//                Debug.Log("✅ Firebase initialized.");
//            }
//            else
//            {
//                Debug.LogError("❌ Firebase initialization failed: " + task.Result);
//            }
//        });
//    }

//    // Hàm lưu dữ liệu
//    public void SaveGameData(GameData data)
//    {
//        if (FirebaseAuth.DefaultInstance.CurrentUser == null)
//        {
//            Debug.LogWarning("⚠️ No user is logged in.");
//            return;
//        }

//        string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
//        string json = JsonUtility.ToJson(data);

//        dbRef.Child("users").Child(userId).Child("gameData").SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
//        {
//            if (task.IsCompleted)
//                Debug.Log("✅ Game data saved!");
//            else
//                Debug.LogError("❌ Save failed: " + task.Exception);
//        });
//    }

//    // Hàm tải dữ liệu
//    public void LoadGameData(System.Action<GameData> onLoaded)
//    {
//        if (FirebaseAuth.DefaultInstance.CurrentUser == null)
//        {
//            Debug.LogWarning("⚠️ No user is logged in.");
//            onLoaded?.Invoke(null);
//            return;
//        }

//        string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

//        dbRef.Child("users").Child(userId).Child("gameData").GetValueAsync().ContinueWithOnMainThread(task =>
//        {
//            if (task.IsFaulted || task.IsCanceled)
//            {
//                Debug.LogError("❌ Load failed: " + task.Exception);
//                onLoaded?.Invoke(null);
//            }
//            else if (task.Result.Exists)
//            {
//                string json = task.Result.GetRawJsonValue();
//                GameData data = JsonUtility.FromJson<GameData>(json);
//                Debug.Log("✅ Game data loaded!");
//                onLoaded?.Invoke(data);
//            }
//            else
//            {
//                Debug.LogWarning("⚠️ No data found for this user.");
//                onLoaded?.Invoke(null);
//            }
//        });
//    }

//    // Test function
//    public void Test()
//    {
//        GameData newData = new GameData(500, 10, 1);
//        SaveGameData(newData);

//        LoadGameData((loadedData) =>
//        {
//            if (loadedData != null)
//            {
//                Debug.Log("Gold: " + loadedData.gold);
//                Debug.Log("Total Level: " + loadedData.totalLevel);
//                Debug.Log("GPGS: " + loadedData.gpgs);
//            }
//        });
//    }
//}
