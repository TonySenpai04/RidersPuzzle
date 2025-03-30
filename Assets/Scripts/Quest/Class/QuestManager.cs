using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private static string path => Path.Combine(Application.persistentDataPath, "questData.json");
    public List<QuestBase> activeQuests = new List<QuestBase>();
    private Dictionary<int, Action> questLists = new Dictionary<int, Action>();
    public static QuestManager instance;
    public int currentQuestList;
    public int stampCount;
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
        StartCoroutine(WaitForServerTime());
        // LoadQuests();
    }
    public void QuestList1()
    {
        activeQuests.Add(new PlayStageQuest("001", "Play stage 2 times with 1001", 10, 2, "1001", ""));
        activeQuests.Add(new PlayStageQuest("002", "Play stage 2 times with 1002", 10, 2, "1002", ""));
        activeQuests.Add(new WinStageQuest("003", "Win 5 times", 10, 5));
        activeQuests.Add(new WinStageNoDamageQuest("004", "Win stage no damage", 10, 2, "", ""));
        activeQuests.Add(new UseRiderSkillQuest("007", "Use skill 5 times", 10, 5, ""));
        activeQuests.Add(new TriggerEntityQuest("008", "Trigger Pandora box 3 times", 10, 3000, 3, ""));
        activeQuests.Add(new TotalCompleteQuest("013", "Complete 6 quest", 40, 6));
    }
    public void QuestList2()
    {
        activeQuests.Add(new PlayStageQuest("001", "Play stage 2 times with 1001", 10, 2, "1001", ""));
        activeQuests.Add(new PlayStageQuest("002", "Play stage 2 times with 1002", 10, 2, "1002", ""));
        activeQuests.Add(new WinStageQuest("003", "Win 5 times", 10, 5));
        activeQuests.Add(new WinStageNoDamageQuest("004", "Win stage no damage", 10, 2, "", ""));
        activeQuests.Add(new UseRiderSkillQuest("007", "Use skill 5 times", 10, 5, ""));
        activeQuests.Add(new TriggerEntityQuest("008", "Trigger  Monstera  3 times", 10, 2001, 3, ""));
        activeQuests.Add(new TotalCompleteQuest("013", "Complete 6 quest", 40, 6));
    }
    public void QuestList3()
    {
        activeQuests.Add(new PlayStageQuest("001", "Play stage 2 times with 1001", 10, 2, "1001", ""));
        activeQuests.Add(new PlayStageQuest("002", "Play stage 2 times with 1002", 10, 2, "1002", ""));
        activeQuests.Add(new WinStageQuest("003", "Win 5 times", 10, 5));
        activeQuests.Add(new WinStageNoDamageQuest("004", "Win stage no damage", 10, 2, "", ""));
        activeQuests.Add(new UseRiderSkillQuest("007", "Use skill 5 times", 10, 5, ""));
        activeQuests.Add(new TriggerEntityQuest("008", "Trigger  spike 3 times", 10, 2003, 3, ""));
        activeQuests.Add(new TotalCompleteQuest("013", "Complete 6 quest", 40, 6));
    }
    public void QuestList4()
    {
        activeQuests.Add(new PlayStageQuest("001", "Play stage 2 times with 1001", 10, 2, "1001", ""));
        activeQuests.Add(new PlayStageQuest("002", "Play stage 2 times with 1002", 10, 2, "1002", ""));
        activeQuests.Add(new WinStageQuest("003", "Win 5 times", 10, 5));
        activeQuests.Add(new WinStageNoDamageQuest("004", "Win stage no damage", 10, 2, "", ""));
        activeQuests.Add(new UseRiderSkillQuest("007", "Use skill 5 times", 10, 5, ""));
        activeQuests.Add(new TriggerEntityQuest("008", "Trigger Bomboz 3 times", 10, 2002, 3, ""));
        activeQuests.Add(new TotalCompleteQuest("013", "Complete 6 quest", 40, 6));
    }
    private IEnumerator WaitForServerTime()
    {
        while (!TimeManager.Instance.IsTimeFetched)
        {
            yield return null; 
        }

        AssignDailyQuestList();
    
    }

    private void AssignDailyQuestList()
    {
        QuestData questData = LoadQuestData(); // Tải dữ liệu từ JSON
        string today = TimeManager.Instance.ServerDate;

        if (questData.lastAssignedDate == today)
        {
            currentQuestList = questData.currentQuestList;
            questLists[currentQuestList]?.Invoke();
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

    void LoadQuests()
    {
       
        foreach (var quest in activeQuests)
        {
            quest.LoadQuest();
            Debug.Log(quest.questId + "-" + quest.description);
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
        int currentComplete = activeQuests.Where(h => h.CheckCompletion()).Count() ;
        UpdateQuest("013", currentComplete, 0);
    }

    public void UpdateQuest(string id, int progress,int progress2)
    {
        QuestData questData = LoadQuestData();
        foreach (var quest in activeQuests)
        {
            if (quest.questId == id)
            {
                quest.UpdateProgress(progress);
                quest.UpdateProgress(progress, progress2); 
                if (quest.CheckCompletion())
                {
                  //  RewardQuest(id);
                    Debug.Log($"Quest {quest.questId} completed!");
                    if (!questData.hasReceivedStampToday)
                    {
                        questData.stampCount += 1;
                        questData.hasReceivedStampToday = true;
                        SaveQuestData(questData);
                        Debug.Log("Nhận 1 stamp cho ngày hôm nay!");
                    }
                }
                return;
            }
        }

        Debug.LogWarning($"Quest with ID {id} not found!");
    }
    public void RewardQuest(string id)
    {
        //foreach (var quest in activeQuests)
        //{
        //    if (quest.questId == id && !quest.isReward && quest.CheckCompletion())
        //    {
        //        GoldManager.instance.AddGold(quest.reward);
        //        quest.isReward = true;
        //        return;
        //    }
        //}
    }
    public QuestBase GetQuestById(string id)
    {
        return activeQuests.FirstOrDefault(h=>h.questId == id);
    }
    public static void SaveQuestData(QuestData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }

    public  QuestData LoadQuestData()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<QuestData>(json);
        }
        return new QuestData(); // Trả về dữ liệu mặc định nếu chưa có file
    }
}

[System.Serializable]
public class QuestData
{
    public int currentQuestList;
    public string lastAssignedDate;
    public int stampCount;
    public bool hasReceivedStampToday;

}
