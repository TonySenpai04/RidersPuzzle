using Firebase.Database;
using Firebase.Extensions;
using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private static string path => Path.Combine(Application.persistentDataPath, "questData.json");
    public List<QuestBase> activeQuests = new List<QuestBase>();
    private Dictionary<int, Action> questLists = new Dictionary<int, Action>();
    public static QuestManager instance;
    public int currentQuestList;
    public int stampCount;
    private Dictionary<string, string> questDescriptions = new Dictionary<string, string>();
    public bool isShowNoti=false;

    private void Awake()
    {
        instance = this;
        InitializeQuestLists();


    }
    private void InitializeQuestLists()
    {
        questLists.Add(1, QuestList1);
        questLists.Add(2, QuestList2);
        questLists.Add(3, QuestList3);
        questLists.Add(4, QuestList4);
    }

    void Start()
    {
       
        // LoadQuests();
    }
    public void CoroutineAssignDailyQuestList()
    {
        StartCoroutine(WaitForServerTime());
    }
    public void ReloadQuestDes()
    {
        activeQuests.Clear();
        questLists[currentQuestList]?.Invoke();
    
        LoadQuests();

    }
    public void QuestList1()
    {
        activeQuests.Clear();
        activeQuests.Add(new PlayStageQuest("001",
            LocalizationManager.instance.GetLocalizedText("quest_002_1_2",2,"Itsui"),
            10, 2, "1001", ""));
        activeQuests.Add(new PlayStageQuest("002",
            LocalizationManager.instance.GetLocalizedText("quest_002_1_2", 2,"Nigou"),
            10, 2, "1002", ""));
        activeQuests.Add(new PlayStageQuest("014",
         LocalizationManager.instance.GetLocalizedText("quest_002_1_2", 2, "Skulz"),
         50, 2, "1003", ""));

        activeQuests.Add(new PlayStageQuest("015",
           LocalizationManager.instance.GetLocalizedText("quest_002_1_2", 2, "Akale"),
           50, 2, "1004", ""));

        activeQuests.Add(new WinStageQuest("003",
            LocalizationManager.instance.GetLocalizedText("quest_003_1", 5),
            10, 5));

        activeQuests.Add(new WinStageNoDamageQuest("004",
            LocalizationManager.instance.GetLocalizedText("quest_004_1", 2),
            10, 2, "", ""));

        activeQuests.Add(new UseRiderSkillQuest("007",
            LocalizationManager.instance.GetLocalizedText("quest_007_1", 5),
            10, 5, ""));

        activeQuests.Add(new TriggerEntityQuest("008",
            LocalizationManager.instance.GetLocalizedText("quest_008_1_2", "Pandora's Box", 3),
            10, 3000, 3, ""));

        activeQuests.Add(new DailyGiftQuest("012",
            LocalizationManager.instance.GetLocalizedText("quest_012_1", 1),
            5, 1));

        activeQuests.Add(new TotalCompleteQuest("013",
            LocalizationManager.instance.GetLocalizedText("quest_013_1", 7),
            85, 7));
      
        //activeQuests.Add(new PlayStageQuest("001", "Play any level with rider Itsui 2 times", 10, 2, "1001", ""));
        //activeQuests.Add(new PlayStageQuest("002", "Play any level with rider Nigou 2 times", 10, 2, "1002", ""));
        //activeQuests.Add(new WinStageQuest("003", "Win any level 5 times", 10, 5));
        //activeQuests.Add(new WinStageNoDamageQuest("004", "Win any level without losing any HP twice", 10, 2, "", ""));
        //activeQuests.Add(new UseRiderSkillQuest("007", "Use rider skill 5 times", 10, 5, ""));
        //activeQuests.Add(new TriggerEntityQuest("008", "Go through Pandora's box entity 3 times", 10, 3000, 3, ""));
        //activeQuests.Add(new DailyGiftQuest("012", "Get free gift in store once", 5, 1));
        //activeQuests.Add(new TotalCompleteQuest("013", "Complete 6 daily quests", 35, 6));
    }
    public void QuestList2()
    {
        activeQuests.Clear();
        activeQuests.Add(new PlayStageQuest("001",
         LocalizationManager.instance.GetLocalizedText("quest_002_1_2", 2, "Itsui"),
        10, 2, "1001", ""));

        activeQuests.Add(new PlayStageQuest("002",
            LocalizationManager.instance.GetLocalizedText("quest_002_1_2", 2, "Nigou"),
            10, 2, "1002", ""));
        activeQuests.Add(new PlayStageQuest("014",
       LocalizationManager.instance.GetLocalizedText("quest_002_1_2", 2, "Skulz"),
       50, 2, "1003", ""));

        activeQuests.Add(new PlayStageQuest("015",
           LocalizationManager.instance.GetLocalizedText("quest_002_1_2", 2, "Akale"),
           50, 2, "1004", ""));
        activeQuests.Add(new WinStageQuest("003",
            LocalizationManager.instance.GetLocalizedText("quest_003_1", 5),
            10, 5));

        activeQuests.Add(new WinStageNoDamageQuest("004",
            LocalizationManager.instance.GetLocalizedText("quest_004_1", 2),
            10, 2, "", ""));

        activeQuests.Add(new UseRiderSkillQuest("007",
            LocalizationManager.instance.GetLocalizedText("quest_007_1", 5),
            10, 5, ""));

        activeQuests.Add(new TriggerEntityQuest("008",
            LocalizationManager.instance.GetLocalizedText("quest_008_1_2", "Monstera", 3),
            10, 2001, 3, ""));

        activeQuests.Add(new DailyGiftQuest("012",
            LocalizationManager.instance.GetLocalizedText("quest_012_1", 1),
            5, 1));

        activeQuests.Add(new TotalCompleteQuest("013",
             LocalizationManager.instance.GetLocalizedText("quest_013_1", 6),
             85, 6));
     
        //activeQuests.Add(new PlayStageQuest("001", "Play any level with rider Itsui 2 times", 10, 2, "1001", ""));
        //activeQuests.Add(new PlayStageQuest("002", "Play any level with rider Nigou 2 times", 10, 2, "1002", ""));
        //activeQuests.Add(new WinStageQuest("003", "Win any level 5 times", 10, 5));
        // activeQuests.Add(new WinStageNoDamageQuest("004", "Win any level without losing any HP twice", 10, 2, "", ""));
        //activeQuests.Add(new UseRiderSkillQuest("007", "Use rider skill 5 times", 10, 5, ""));
        //activeQuests.Add(new TriggerEntityQuest("008", "Go through the Monstera entity 3 times", 10, 2001, 3, ""));
        //activeQuests.Add(new DailyGiftQuest("012", "Get free gift in store once", 5, 1));
        //activeQuests.Add(new TotalCompleteQuest("013", "Complete 6 daily quests", 35, 6));
    }
    public void QuestList3()
    {
        activeQuests.Clear();
        activeQuests.Add(new PlayStageQuest("001",
         LocalizationManager.instance.GetLocalizedText("quest_002_1_2", 2, "Itsui"),
        10, 2, "1001", ""));

        activeQuests.Add(new PlayStageQuest("002",
            LocalizationManager.instance.GetLocalizedText("quest_002_1_2", 2, "Nigou"),
            10, 2, "1002", ""));
        activeQuests.Add(new PlayStageQuest("014",
      LocalizationManager.instance.GetLocalizedText("quest_002_1_2", 2, "Skulz"),
      50, 2, "1003", ""));

        activeQuests.Add(new PlayStageQuest("015",
           LocalizationManager.instance.GetLocalizedText("quest_002_1_2", 2, "Akale"),
           50, 2, "1004", ""));

        activeQuests.Add(new WinStageQuest("003",
            LocalizationManager.instance.GetLocalizedText("quest_003_1", 5),
            10, 5));

        activeQuests.Add(new WinStageNoDamageQuest("004",
            LocalizationManager.instance.GetLocalizedText("quest_004_1", 2),
            10, 2, "", ""));

        activeQuests.Add(new UseRiderSkillQuest("007",
            LocalizationManager.instance.GetLocalizedText("quest_007_1", 5),
            10, 5, ""));

        activeQuests.Add(new TriggerEntityQuest("008",
            LocalizationManager.instance.GetLocalizedText("quest_008_1_2", "Spike", 3),
            10, 2003, 3, ""));

        activeQuests.Add(new DailyGiftQuest("012",
            LocalizationManager.instance.GetLocalizedText("quest_012_1", 1),
            5, 1));

        activeQuests.Add(new TotalCompleteQuest("013",
            LocalizationManager.instance.GetLocalizedText("quest_013_1", 6),
            85, 6));
    
        //activeQuests.Add(new PlayStageQuest("001", "Play any level with rider Itsui 2 times", 10, 2, "1001", ""));
        //activeQuests.Add(new PlayStageQuest("002", "Play any level with rider Nigou 2 times", 10, 2, "1002", ""));
        //activeQuests.Add(new WinStageQuest("003", "Win any level 5 times", 10, 5));
        //activeQuests.Add(new WinStageNoDamageQuest("004", "Win any level without losing any HP twice", 10, 2, "", ""));
        //activeQuests.Add(new UseRiderSkillQuest("007", "Use rider skill 5 times", 10, 5, ""));
        //activeQuests.Add(new TriggerEntityQuest("008", "Go through the Spike entity 3 times", 10, 2003, 3, ""));
        //activeQuests.Add(new DailyGiftQuest("012", "Get free gift in store once", 5, 1));
        //activeQuests.Add(new TotalCompleteQuest("013", "Complete 6 daily quests", 35, 6));
    }
    public void QuestList4()
    {
        activeQuests.Clear();
        activeQuests.Add(new PlayStageQuest("001",
         LocalizationManager.instance.GetLocalizedText("quest_002_1_2", 2, "Itsui"),
        10, 2, "1001", ""));

        activeQuests.Add(new PlayStageQuest("002",
            LocalizationManager.instance.GetLocalizedText("quest_002_1_2", 2, "Nigou"),
            10, 2, "1002", ""));
        activeQuests.Add(new PlayStageQuest("014",
   LocalizationManager.instance.GetLocalizedText("quest_002_1_2", 2, "Skulz"),
   50, 2, "1003", ""));

        activeQuests.Add(new PlayStageQuest("015",
           LocalizationManager.instance.GetLocalizedText("quest_002_1_2", 2, "Akale"),
           50, 2, "1004", ""));
        activeQuests.Add(new WinStageQuest("003",
            LocalizationManager.instance.GetLocalizedText("quest_003_1", 5),
            10, 5));

        activeQuests.Add(new WinStageNoDamageQuest("004",
            LocalizationManager.instance.GetLocalizedText("quest_004_1", 2),
            10, 2, "", ""));

        activeQuests.Add(new UseRiderSkillQuest("007",
            LocalizationManager.instance.GetLocalizedText("quest_007_1", 5),
            10, 5, ""));

        activeQuests.Add(new TriggerEntityQuest("008",
            LocalizationManager.instance.GetLocalizedText("quest_008_1_2", "Bomb", 3),
            10, 2002, 3, ""));

        activeQuests.Add(new DailyGiftQuest("012",
            LocalizationManager.instance.GetLocalizedText("quest_012_1", 1),
            5, 1));

        activeQuests.Add(new TotalCompleteQuest("013",
           LocalizationManager.instance.GetLocalizedText("quest_013_1", 6),
           85, 6));
 
        //activeQuests.Add(new PlayStageQuest("001", "Play any level with rider Itsui 2 times", 10, 2, "1001", ""));
        //activeQuests.Add(new PlayStageQuest("002", "Play any level with rider Nigou 2 times", 10, 2, "1002", ""));
        //activeQuests.Add(new WinStageQuest("003", "Win any level 5 times", 10, 5));   
        //activeQuests.Add(new WinStageNoDamageQuest("004", "Win any level without losing any HP twice", 10, 2, "", ""));
        //activeQuests.Add(new UseRiderSkillQuest("007", "Use rider skill 5 times", 10, 5, ""));
        //activeQuests.Add(new TriggerEntityQuest("008", "Go through Bomb entity 3 times", 10, 2002, 3, ""));
        //activeQuests.Add(new DailyGiftQuest("012", "Get free gift in store once", 5, 1));
        //activeQuests.Add(new TotalCompleteQuest("013", "Complete 6 daily quests", 35, 6));
    }
    private IEnumerator WaitForServerTime()
    {
        while (!TimeManager.Instance.IsTimeFetched)
        {
            yield return null; 
        }

        AssignDailyQuestList();
    
    }

    private async void AssignDailyQuestList()
    {
        QuestData questData = await LoadQuestData(); 
        string today  = TimeManager.Instance.GetEffectiveDateForDailyQuest();
        Debug.Log(today);
        string todayGift = TimeManager.Instance.ServerDate;
        if(questData.lastAssignedDate != todayGift)
        {
            NotiManager.instance.CheckDailyLogin();
        }
        else
        {
            NotiManager.instance.ClearMultipleNotiRedDots(new List<string> { "shop", "dailygift" });
        }
        if (questData.lastAssignedDate == today)
        {
            currentQuestList = questData.currentQuestList;
            stampCount = questData.stampCount;
            questLists[currentQuestList]?.Invoke();
            isShowNoti = false;
            LoadQuests();
            Debug.Log($"Loaded previous quest list {currentQuestList} for today.");
            return;
        }

        currentQuestList = UnityEngine.Random.Range(1, questLists.Count + 1);
        questData.lastAssignedDate = today;
        questData.currentQuestList = currentQuestList;
        questData.hasReceivedStampToday = false;
        SaveQuestData(questData);

        activeQuests.Clear();
        questLists[currentQuestList]?.Invoke();
        foreach (var quest in activeQuests)
        {
            quest.DeleteQuest();
        }
        LoadQuests();
        Debug.Log($"Assigned new quest list {currentQuestList} for today.");
    }

    public void LoadQuests()
    {
       
        foreach (var quest in activeQuests)
        {
            quest.LoadQuest();

        }
    }
    public List<QuestBase> GetQuestsByType<T>() where T : QuestBase
    {
        List<QuestBase> filteredQuests = new List<QuestBase>();

        foreach (var quest in activeQuests)
        {
            if (quest is T)
            {
                filteredQuests.Add(quest);
            }
        }

        return filteredQuests;
    }

    private void FixedUpdate()
    {
        //int currentComplete = activeQuests.Where(h => h.CheckCompletion()).Count() ;
        //UpdateQuest("013", currentComplete, 0);
    }

    public  async void UpdateQuest(string id, int progress,int progress2)
    {
        QuestData questData = await LoadQuestData();
        foreach (var quest in activeQuests)
        {
            if (quest.questId == id)
            {
                quest.UpdateProgress(progress);
                quest.UpdateProgress(progress, progress2);
                GetQuestById("013").UpdateProgress(activeQuests.Where(h => h.CheckCompletion()).Count());
                if (quest.CheckCompletion())
                {
                    Debug.Log($"Quest {quest.questId} completed!");
                    if (!questData.hasReceivedStampToday)
                    {
                        questData.stampCount += 1;
                        questData.hasReceivedStampToday = true;
                        isShowNoti = true;
                     
                        SaveQuestData(questData);
                        Debug.Log("Nhận 1 stamp cho ngày hôm nay!");
                    }
                }
                return;
            }
        }

        Debug.LogWarning($"Quest with ID {id} not found!");
    }
    public QuestBase GetQuestById(string id)
    {
        return activeQuests.FirstOrDefault(h=>h.questId == id);
    }
    public  void SaveQuestData(QuestData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
        SaveQuestDataToFirebase(data);
    }

    public async Task<QuestData> LoadQuestData()
    {
        QuestData localData = new QuestData();

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            localData = JsonUtility.FromJson<QuestData>(json);
        }

        var firebaseData = await LoadQuestDataFromFirebaseAsync(); // cần viết lại hàm này trả về Task<QuestData>
        if (firebaseData != null)
        {
            localData = firebaseData;
            stampCount = localData.stampCount;
            Debug.Log("⚠️ Firebase có dữ liệu QuestData");
        }
        else
        {
            Debug.Log("⚠️ Firebase không có dữ liệu QuestData. Dùng dữ liệu local.");
        }

        return localData;
    }

    public void  SyncLocalQuestsToFirebaseIfNotExist()
    {
       foreach(var quest in activeQuests)
       {
            quest.SyncLocalQuestsToFirebaseIfNotExist();
       }
        CoroutineAssignDailyQuestList();
    }
    public  void SaveQuestDataToFirebase(QuestData data)
    {
        var user = FirebaseDataManager.Instance.GetCurrentUser();
        if (user == null)
        {
            Debug.LogWarning("❌ Chưa đăng nhập - Không thể lưu QuestData lên Firebase.");
            return;
        }

        string json = JsonUtility.ToJson(data);
        FirebaseDatabase.DefaultInstance.RootReference
            .Child("users")
            .Child(user.UserId)
            .Child("questData")
            .SetRawJsonValueAsync(json)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                    Debug.Log("✅ Đã lưu QuestData lên Firebase.");
                else
                    Debug.LogError("❌ Lỗi khi lưu QuestData: " + task.Exception);
            });
    }

    public async Task<QuestData> LoadQuestDataFromFirebaseAsync()
    {
        if (FirebaseDataManager.Instance.GetCurrentUser() == null)
        {
            Debug.LogWarning("❌ Chưa đăng nhập - không thể load QuestData từ Firebase.");
            return null;
        }

        string userId = FirebaseDataManager.Instance.GetCurrentUser().UserId;
        string key = "questData";

        DataSnapshot snapshot = await FirebaseDatabase.DefaultInstance.RootReference
            .Child("users")
            .Child(userId)
            .Child(key)
            .GetValueAsync();

        if (snapshot.Exists)
        {
            string json = snapshot.GetRawJsonValue();
            Debug.Log("✅ Load QuestData từ Firebase:\n" + json);
            QuestData data = JsonUtility.FromJson<QuestData>(json);
            Debug.Log("Count:" + data.stampCount);
            //  SaveQuestData(data); 
            return data;
        }
        else
        {
            Debug.Log("☁️ Firebase chưa có -> Load local & đồng bộ lên Firebase.");
            QuestData localData = new QuestData();

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                localData = JsonUtility.FromJson<QuestData>(json);
            }

            SaveQuestDataToFirebase(localData); // cũng nên chuyển sang async luôn
            return localData;
        }
    }

}

[System.Serializable]
public class QuestData
{
    public int currentQuestList;
    public string lastAssignedDate;
    public int stampCount;
    public bool hasReceivedStampToday;
    public QuestData()
    {

    }

}
