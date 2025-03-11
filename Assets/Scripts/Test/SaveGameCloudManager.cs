//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
//using GooglePlayGames.BasicApi.SavedGame;
//using System.Text;
//using System.Linq;

//[Serializable]
//public class GameSaveData
//{
//    public UnlockHeroData unlockHeroData;
//    public List<LevelProgressData> levelProgressData; 
//    public GoldData goldData;
//    public SeenObjectsData seenObjectsData;
//    public LanguageData languageData;

//    public GameSaveData(UnlockHeroData unlockHeroData, List<LevelProgressData> levelProgressData, 
//        GoldData goldData, SeenObjectsData seenObjectsData, LanguageData languageData)
//    {
//        this.unlockHeroData = unlockHeroData;
//        this.levelProgressData = levelProgressData;
//        this.goldData = goldData;
//        this.seenObjectsData = seenObjectsData;
//        this.languageData = languageData;
//    }
//}
//public class SaveGameCloudManager : MonoBehaviour
//{
//    public static SaveGameCloudManager instance;
//    private void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//        }
//    }
//    //public void SaveGame()
//    //{
//    //    var data = new GoldData { amount = GoldManager.instance.GetGold() };
//    //    var seenObjectsData = new SeenObjectsData (HiddenObjectManager.instance.GetSeenObjectID() );
//    //    var languageData =new LanguageData(LocalizationManager.instance.GetCurrentLangaue());
//    //    GameSaveData saveData = new GameSaveData(
//    //        new UnlockHeroData(HeroManager.instance.GetUnlockHero().Select(h => h.id).ToList()),
//    //        SaveGameManager.instance.LoadAllProgress(),
//    //        data,
//    //        seenObjectsData,
//    //        languageData
//    //    );

//    //   SaveToCloud(saveData);
//    //}

//    private void SaveDataToCloud<T>(string fileName, T data)
//    {
//        string json = JsonUtility.ToJson(data);
//        byte[] bytes = Encoding.UTF8.GetBytes(json);

//        PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConflictResolution(
//            fileName,
//            DataSource.ReadCacheOrNetwork,
//            ConflictResolutionStrategy.UseLongestPlaytime,
//            (status, game) =>
//            {
//                if (status == SavedGameRequestStatus.Success)
//                {
//                    SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder()
//                        .WithUpdatedDescription("Lưu lúc: " + DateTime.Now)
//                        .Build();

//                    PlayGamesPlatform.Instance.SavedGame.CommitUpdate(game, update, bytes, (commitStatus, savedGame) =>
//                    {
//                        Debug.Log(commitStatus == SavedGameRequestStatus.Success ? $"Lưu {fileName} thành công!" : $"Lỗi khi lưu {fileName}.");
//                    });
//                }
//                else
//                {
//                    Debug.LogError($"Lỗi khi mở file {fileName} trên Cloud.");
//                }
//            }
//        );
//    }
//    private void LoadDataFromCloud<T>(string fileName, Action<T> onDataLoaded) where T : new()
//    {
//        PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConflictResolution(
//            fileName,
//            DataSource.ReadCacheOrNetwork,
//            ConflictResolutionStrategy.UseLongestPlaytime,
//            (status, game) =>
//            {
//                if (status == SavedGameRequestStatus.Success)
//                {
//                    PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(game, (readStatus, data) =>
//                    {
//                        if (readStatus == SavedGameRequestStatus.Success && data.Length > 0)
//                        {
//                            string json = Encoding.UTF8.GetString(data);
//                            T loadedData = JsonUtility.FromJson<T>(json);
//                            onDataLoaded?.Invoke(loadedData);
//                        }
//                        else
//                        {
//                            Debug.LogError($"Lỗi khi tải {fileName} hoặc dữ liệu trống.");
//                            onDataLoaded?.Invoke(new T());
//                        }
//                    });
//                }
//                else
//                {
//                    Debug.LogError($"Lỗi khi mở file {fileName} trên Cloud.");
//                    onDataLoaded?.Invoke(new T());
//                }
//            }
//        );
//    }

//    // 🎯 Từng loại dữ liệu sẽ có hàm riêng để lưu/tải

//    public void SaveUnlockHeroData(UnlockHeroData data) => SaveDataToCloud("unlock_hero", data);
//    public void LoadUnlockHeroData(Action<UnlockHeroData> callback) => LoadDataFromCloud("unlock_hero", callback);

//    public void SaveLevelProgressData(LevelProgressData data) => SaveDataToCloud("level_progress", data);
//    public void LoadLevelProgressData(Action<LevelProgressData> callback) => LoadDataFromCloud("level_progress", callback);

//    public void SaveGoldData(GoldData data) => SaveDataToCloud("gold_data", data);
//    public void LoadGoldData(Action<GoldData> callback) => LoadDataFromCloud("gold_data", callback);

//    public void SaveSeenObjectsData(SeenObjectsData data) => SaveDataToCloud("seen_objects", data);
//    public void LoadSeenObjectsData(Action<SeenObjectsData> callback) => LoadDataFromCloud("seen_objects", callback);

//    public void SaveLanguageData(LanguageData data) => SaveDataToCloud("language_data", data);
//    public void LoadLanguageData(Action<LanguageData> callback) => LoadDataFromCloud("language_data", callback);


//}
