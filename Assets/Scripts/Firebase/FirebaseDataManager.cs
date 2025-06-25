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
    public int avatarIndex;

    public PlayerData() { }

    public PlayerData(string name, int level, int gold, List<LevelProgressData> data, UnlockHeroData unlockHeroData, int avatarIndex)
    {
        this.name = name;
        this.totalLevel = level;
        this.gold = gold;
        this.levelData = data;
        this.unlockHeroData = unlockHeroData;
        this.avatarIndex = avatarIndex;
    }
}


public class FirebaseDataManager : MonoBehaviour
{
    public static FirebaseDataManager Instance;

    private FirebaseAuth auth;
    private DatabaseReference dbRef;
    public FirebaseUser currentUser;
    public string username;


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
    public void Register(string email, string password, string username, System.Action<bool, string> onResult = null)
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
            this.username= username;
            Debug.Log("✅ Registered: " + currentUser.Email);
            onResult?.Invoke(true, LocalizationManager.instance.GetLocalizedText("login_popup_register_success"));
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
                string errorMessage = "Wrong username or password";
                if (task.Exception != null && task.Exception.InnerExceptions.Count > 0)
                {
                    var firebaseEx = task.Exception.InnerExceptions[0] as FirebaseException;
                    if (firebaseEx != null)
                    {
                        switch ((AuthError)firebaseEx.ErrorCode)
                        {
                            case AuthError.InvalidEmail:
                                errorMessage = LocalizationManager.instance.GetLocalizedText("login_popup_wrong_password");
                                break;
                            case AuthError.WrongPassword:
                                errorMessage = LocalizationManager.instance.GetLocalizedText("login_popup_wrong_password"); ;
                                break;
                            case AuthError.UserNotFound:
                                errorMessage = LocalizationManager.instance.GetLocalizedText("login_popup_wrong_password"); ;
                                break;
                            default:
                                errorMessage = LocalizationManager.instance.GetLocalizedText("login_popup_wrong_password"); ;
                                break;
                        }
                    }
                }

                onResult?.Invoke(false, errorMessage);
                //LoadPlayerData((data) =>
                //{
                //    if (data != null)
                //    {
                //        this.username = data.name;
                //        LevelManager.instance.LoadLevelData();
                //        GoldManager.instance.LoadCloudData();
                //        HeroManager.instance.LoadCloudUnlockHero();
                //        AvatarManager.instance.SelectAvatar(data.avatarIndex);
                //        Debug.Log("🎮 Dữ liệu người chơi: " + data.gold + " - Level: " + data.totalLevel);
                //    }
                //    else
                //    {
                //        Debug.Log("📂 Chưa có dữ liệu. Tạo mới nếu cần.");
                //    }
                //});
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
                    this.username = data.name;
                    AvatarManager.instance.selectedAvatarIndex=data.avatarIndex;
                    AvatarManager.instance.SelectAvatar(data.avatarIndex);
                    Debug.Log("🎮 Dữ liệu người chơi: " + data.gold + " - Level: " + data.totalLevel+"-Name" +data.name);
                }
                else
                {
                    SaveData(LevelManager.instance.GetAllLevelComplete(), GoldManager.instance.GetGold(),
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
            PlayerData newData = new PlayerData(username,SaveGameManager.instance.LoadAllProgress().Count,
                GoldManager.instance.GetGold(), SaveGameManager.instance.LoadAllProgress(),HeroManager.instance.GetUnlockHeroID(),0);
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
    public void ForgotPassword(string email, System.Action<bool, string> onResult = null)
    {
        if (string.IsNullOrEmpty(email))
        {
            onResult?.Invoke(false, "Username is invalid");
            return;
        }

        FirebaseAuth.DefaultInstance.SendPasswordResetEmailAsync(email).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("❌ Gửi email khôi phục thất bại: " + task.Exception);
                onResult?.Invoke(false, "Username is invalid");
            }
            else
            {
                Debug.Log("✅ Đã gửi email khôi phục thành công.");
                onResult?.Invoke(true, "New password is sent.");
            }
        });
    }

    public void SaveData(int level, int gold, List<LevelProgressData> newData, UnlockHeroData unlockHeroData)
    {
        // Tải dữ liệu hiện tại để không mất dữ liệu cũ
        LoadPlayerData((loadedData) =>
        {
            if (loadedData != null)
            {
                List<LevelProgressData> updatedLevelData = loadedData.levelData ?? new List<LevelProgressData>();

                // Cập nhật hoặc thêm mới từng level trong danh sách
                foreach (var newLevel in newData)
                {
                    bool exists = false;
                    for (int i = 0; i < updatedLevelData.Count; i++)
                    {
                        if (updatedLevelData[i].levelIndex == newLevel.levelIndex)
                        {
                            updatedLevelData[i] = newLevel; // Ghi đè dữ liệu mới vào level cũ
                            exists = true;
                            break;
                        }
                    }
                    if (!exists)
                    {
                        updatedLevelData.Add(newLevel); // Thêm level mới nếu chưa tồn tại
                    }
                }

                // Tạo dữ liệu mới để lưu
                PlayerData newPlayerData = new PlayerData(username, level, gold, updatedLevelData, unlockHeroData, AvatarManager.instance.selectedAvatarIndex);
                SavePlayerData(newPlayerData);
            }
            else
            {
                // Nếu không có dữ liệu cũ, tạo dữ liệu mới
                PlayerData newPlayerData = new PlayerData(username, level, gold, newData, unlockHeroData, AvatarManager.instance.selectedAvatarIndex);
                SavePlayerData(newPlayerData);
            }
        });
    }


}
