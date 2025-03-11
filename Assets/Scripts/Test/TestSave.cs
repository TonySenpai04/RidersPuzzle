//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Text;
//using UnityEngine;
//using GooglePlayGames;
//using GooglePlayGames.BasicApi.SavedGame;
//using UnityEngine.SocialPlatforms;
//using GooglePlayGames.BasicApi;
//using TMPro;

//[System.Serializable]
//public class GameData
//{
//    public int gold;
//    public int level;
//    public string selectedCharacter;
//    public List<string> itemsOwned = new List<string>();
//}

//public class TestSave : MonoBehaviour
//{
//    private const string saveFileName = "game_save";
//    private const string localFileName = "saveTest.json";
//    public TextMeshProUGUI text;
//   public static TestSave Instance;
//     public TextMeshProUGUI text2;

//    private void Awake()
//    {
//        Instance = this;
//    }

//    // Gọi hàm này để lưu dữ liệu
//    public void SaveGame(GameData data)
//    {
//        string json = JsonUtility.ToJson(data);
//        SaveToLocal(json);

//        if (Social.localUser.authenticated)
//        {
//            SaveToCloud(json);
//            text.text = "success";
//        }
//        else
//        {
//            text.text = "faild";
//        }
//    }

//    // Gọi hàm này để load dữ liệu
//    public void LoadGame(Action<GameData> onLoaded)
//    {
//        if (Social.localUser.authenticated)
//        {
//            LoadFromCloud((data) => {
//                if (data != null)
//                {
//                    onLoaded?.Invoke(data);
//                }
//                else
//                {
//                    LoadFromLocal(onLoaded);
//                }
//            });
//        }
//        else
//        {
//            LoadFromLocal(onLoaded);
//        }
//    }

//    // ------------------------ LOCAL SAVE ------------------------
//    private void SaveToLocal(string json)
//    {
//        string path = Path.Combine(Application.persistentDataPath, localFileName);
//        File.WriteAllText(path, json);
//        Debug.Log("Đã lưu local: " + path);
//    }

//    private void LoadFromLocal(Action<GameData> onLoaded)
//    {
//        string path = Path.Combine(Application.persistentDataPath, localFileName);
//        if (File.Exists(path))
//        {
//            string json = File.ReadAllText(path);
//            GameData data = JsonUtility.FromJson<GameData>(json);
//            Debug.Log("Đã load từ local.");
//            onLoaded?.Invoke(data);
//        }
//        else
//        {
//            Debug.Log("Không tìm thấy file local.");
//            onLoaded?.Invoke(new GameData()); // Trả về dữ liệu mặc định nếu chưa có
//        }
//    }

//    // ------------------------ GPGS CLOUD SAVE ------------------------
//    private void SaveToCloud(string json)
//    {
//        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

//        savedGameClient.OpenWithAutomaticConflictResolution(
//            saveFileName,
//            DataSource.ReadCacheOrNetwork,
//            ConflictResolutionStrategy.UseLongestPlaytime,
//            (status, game) => {
//                if (status == SavedGameRequestStatus.Success)
//                {
//                    byte[] bytes = Encoding.UTF8.GetBytes(json);
//                    SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder().Build();

//                    savedGameClient.CommitUpdate(game, update, bytes, (commitStatus, metadata) => {
//                        if (commitStatus == SavedGameRequestStatus.Success)
//                        {
//                            Debug.Log("Đã lưu dữ liệu lên GPGS cloud.");
//                        }
//                        else
//                        {
//                            Debug.LogWarning("Lỗi khi lưu lên GPGS.");
//                        }
//                    });
//                }
//                else
//                {
//                    Debug.LogWarning("Không mở được file GPGS.");
//                }
//            });
//    }
//    public void TestAdd()
//    {
//        Test(5, 3);
//    }
//    public void TestAdd2()
//    {
//        Test(54, 34);
//    }
//    public void Test(int gold,int level)
//    {
//        GameData data = new GameData
//        {
//            gold = gold,
//            level = level,
//            selectedCharacter = "Archer",
//            itemsOwned = new List<string> { "Bow", "Quiver" }
//        };
//        SaveGame(data);

//        // Tải dữ liệu
//        LoadGame((loadedData) =>
//        {
//            text.text=("Gold: " + loadedData.gold+ "-Level: " + loadedData.level);
//            text2.text = ("Gold: " + loadedData.gold + "-Level: " + loadedData.level);
//        });
//    }
//        private void LoadFromCloud(Action<GameData> onLoaded)
//    {
//        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

//        savedGameClient.OpenWithAutomaticConflictResolution(
//            saveFileName,
//            DataSource.ReadCacheOrNetwork,
//            ConflictResolutionStrategy.UseLongestPlaytime,
//            (status, game) => {
//                if (status == SavedGameRequestStatus.Success)
//                {
//                    savedGameClient.ReadBinaryData(game, (readStatus, data) => {
//                        if (readStatus == SavedGameRequestStatus.Success)
//                        {
//                            string json = Encoding.UTF8.GetString(data);
//                            GameData gameData = JsonUtility.FromJson<GameData>(json);
//                            Debug.Log("Đã load dữ liệu từ GPGS cloud.");
//                            onLoaded?.Invoke(gameData);
//                        }
//                        else
//                        {
//                            Debug.LogWarning("Lỗi khi đọc từ GPGS.");
//                            onLoaded?.Invoke(null);
//                        }
//                    });
//                }
//                else
//                {
//                    Debug.LogWarning("Không mở được file GPGS.");
//                    onLoaded?.Invoke(null);
//                }
//            });
//    }
//}
