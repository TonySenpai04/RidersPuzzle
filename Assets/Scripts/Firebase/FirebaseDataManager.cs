using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

[System.Serializable]
public class PlayerData
{
    public string name;
    public int totalLevel;
    public int gold;
    public List<LevelProgressData> levelData;
    public UnlockHeroData unlockHeroData;

    public PlayerData() { }

    public PlayerData(string name, int level, int gold, List<LevelProgressData> data, UnlockHeroData unlockHeroData)
    {
        this.name = name;
        this.totalLevel = level;
        this.gold = gold;
        this.levelData = data;
        this.unlockHeroData = unlockHeroData;
    }
}


public class FirebaseDataManager : MonoBehaviour
{
    public static FirebaseDataManager Instance;

    private FirebaseAuth auth;
    private DatabaseReference dbRef;
    private FirebaseUser currentUser;

    private void Awake()
    {

            Instance = this;

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
               // FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri("https://riders-puzzle-default-rtdb.firebaseio.com/");
                dbRef = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("✅ Firebase ready.");
                Debug.Log(dbRef);

                // Auto test login (bạn có thể xóa dòng này nếu muốn test thủ công)
              // Login("Admin1@gmail.com", "Admin1");
               
            }
            else
            {
                Debug.Log("❌ Firebase init error: " + task.Result);
            }
        });
    }

    // 🔐 Đăng ký tài khoản
    public void Register(string email, string password, System.Action<bool, string> onResult = null)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log("❌ Register failed: " + task.Exception);

                string errorMessage = "Đăng ký thất bại. Vui lòng kiểm tra thông tin.";

                if (task.Exception != null && task.Exception.InnerExceptions.Count > 0)
                {
                    var firebaseEx = task.Exception.InnerExceptions[0] as FirebaseException;
                    if (firebaseEx != null)
                    {
                        switch ((AuthError)firebaseEx.ErrorCode)
                        {
                            case AuthError.EmailAlreadyInUse:
                                errorMessage = "Email đã được sử dụng.";
                                break;
                            case AuthError.InvalidEmail:
                                errorMessage = "Email không hợp lệ.";
                                break;
                            case AuthError.WeakPassword:
                                errorMessage = "Mật khẩu quá yếu. (ít nhất 6 ký tự)";
                                break;
                            default:
                                errorMessage = "Đăng ký thất bại: " + firebaseEx.Message;
                                break;
                        }
                    }
                }

                onResult?.Invoke(false, errorMessage);
                return;
            }

            currentUser = task.Result.User;
            Debug.Log("✅ Registered: " + currentUser.Email);
            onResult?.Invoke(true, null);
        });
    }



    // 🔐 Đăng nhập tài khoản
    public void Login(string email, string password, System.Action<bool, string> onResult = null)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log("❌ Login failed: " + task.Exception);
                string errorMessage = "Đăng nhập thất bại. Vui lòng kiểm tra tài khoản hoặc mật khẩu.";
                if (task.Exception != null && task.Exception.InnerExceptions.Count > 0)
                {
                    var firebaseEx = task.Exception.InnerExceptions[0] as FirebaseException;
                    if (firebaseEx != null)
                    {
                        switch ((AuthError)firebaseEx.ErrorCode)
                        {
                            case AuthError.InvalidEmail:
                                errorMessage = "Email không hợp lệ.";
                                break;
                            case AuthError.WrongPassword:
                                errorMessage = "Sai mật khẩu.";
                                break;
                            case AuthError.UserNotFound:
                                errorMessage = "Tài khoản không tồn tại.";
                                break;
                            default:
                                errorMessage = "Đăng nhập thất bại: " + firebaseEx.Message;
                                break;
                        }
                    }
                }

                onResult?.Invoke(false, errorMessage);
                LoadPlayerData((data) =>
                {
                    if (data != null)
                    {
                        LevelManager.instance.LoadLevelData();
                        GoldManager.instance.LoadCloudData();
                        HeroManager.instance.LoadCloudUnlockHero();
                        Debug.Log("🎮 Dữ liệu người chơi: " + data.gold + " - Level: " + data.totalLevel);
                    }
                    else
                    {
                        Debug.Log("📂 Chưa có dữ liệu. Tạo mới nếu cần.");
                    }
                });
                return;
            }

            currentUser = task.Result.User;
            Debug.Log("✅ Logged in: " + currentUser.Email + " | UID: " + currentUser.UserId);

            // Optional: Load dữ liệu sau login
            LoadPlayerData((data) =>
            {
                if (data != null)
                {
                    LevelManager.instance.LoadLevelData();
                    GoldManager.instance.LoadCloudData();
                    HeroManager.instance.LoadCloudUnlockHero();
                    Debug.Log("🎮 Dữ liệu người chơi: " + data.gold + " - Level: " + data.totalLevel);
                }
                else
                {
                    Example("Tony2", LevelManager.instance.GetAllLevelComplete(), GoldManager.instance.GetGold(),
           SaveGameManager.instance.LoadAllProgress(), HeroManager.instance.GetUnlockHeroID());
                    Debug.Log("📂 Chưa có dữ liệu. Tạo mới nếu cần.");
                }
            });
            onResult?.Invoke(true, null);

        });
    }


    // ✅ Lưu dữ liệu
    public void SavePlayerData(PlayerData data)
    {
        if (currentUser == null)
        {
            Debug.Log("⚠ Chưa đăng nhập!");
            return;
        }

        string json = JsonUtility.ToJson(data);
        dbRef.Child("users").Child(currentUser.UserId).Child("playerData").SetRawJsonValueAsync(json);
        Debug.Log("✅ Đã lưu dữ liệu.");
    }

    // ✅ Tải dữ liệu
    public void LoadPlayerData(System.Action<PlayerData> onDataLoaded)
    {
        if (currentUser == null)
        {
            Debug.Log("⚠ Chưa đăng nhập!");
            PlayerData newData = new PlayerData("Tony",SaveGameManager.instance.LoadAllProgress().Count,
                GoldManager.instance.GetGold(), SaveGameManager.instance.LoadAllProgress(),HeroManager.instance.GetUnlockHeroID());
            onDataLoaded?.Invoke(newData);
            return;
        }

        dbRef.Child("users").Child(currentUser.UserId).Child("playerData").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
            {
                PlayerData data = JsonUtility.FromJson<PlayerData>(task.Result.GetRawJsonValue());
                onDataLoaded?.Invoke(data);
            }
            else
            {
                onDataLoaded?.Invoke(null);
            }
        });
    }

    public FirebaseUser GetCurrentUser()
    {
        return currentUser;
    }
    public void Example(string name,int level, int gold, List<LevelProgressData> data,UnlockHeroData unlockHeroData)
    {
        // Tạo dữ liệu và lưu
        PlayerData newData = new PlayerData(name,level, gold, data,unlockHeroData);
        SavePlayerData(newData);

        // Tải dữ liệu
        LoadPlayerData((loadedData) =>
        {
            if (loadedData != null)
            {
                Debug.Log("Gold: " + loadedData.gold + " | Level: " + loadedData.totalLevel);
            }
        });
    }

}
