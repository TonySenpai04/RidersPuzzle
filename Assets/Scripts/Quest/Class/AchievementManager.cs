using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public List<QuestBase> activeQuests = new List<QuestBase>();
    public static AchievementManager instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
       
    }

    void LoadQuests()
    {
        activeQuests.Clear();
        activeQuests.Add(new LoginNormalQuest("0001",
                  LocalizationManager.instance.GetLocalizedText("quest_001_1",7),
                  288, 7)
        { groupId = "LoginNormal" });
        activeQuests.Add(new LoginNormalQuest("0002",
                 LocalizationManager.instance.GetLocalizedText("quest_001_1", 14),
                 288, 14)
        { groupId = "LoginNormal" });
        activeQuests.Add(new LoginNormalQuest("0003",
            LocalizationManager.instance.GetLocalizedText("quest_001_1", 21),
            288, 21)
        { groupId = "LoginNormal" });
        activeQuests.Add(new LoginNormalQuest("0004",
                 LocalizationManager.instance.GetLocalizedText("quest_001_1", 28),
                 288, 28)
        { groupId = "LoginNormal" });
        activeQuests.Add(new LoginNormalQuest("0005",
                 LocalizationManager.instance.GetLocalizedText("quest_001_1", 35),
                 288, 35)
        { groupId = "LoginNormal" });

        activeQuests.Add(new LoginStreakQuest("0006",
                LocalizationManager.instance.GetLocalizedText("quest_001_2", 7),
                688, 7)
        { groupId = "LoginStreakQuest" });
        activeQuests.Add(new LoginStreakQuest("0007",
                 LocalizationManager.instance.GetLocalizedText("quest_001_2", 14),
                 688, 14)
        { groupId = "LoginStreakQuest" });
        activeQuests.Add(new LoginStreakQuest("0008",
               LocalizationManager.instance.GetLocalizedText("quest_001_2", 21),
               688, 21)
        { groupId = "LoginStreakQuest" });
        activeQuests.Add(new LoginStreakQuest("0009",
                 LocalizationManager.instance.GetLocalizedText("quest_001_2", 28),
                 688, 28)
        { groupId = "LoginStreakQuest" });
        activeQuests.Add(new LoginStreakQuest("0010",
                 LocalizationManager.instance.GetLocalizedText("quest_001_2", 35),
                 688, 35)
        { groupId = "LoginStreakQuest" });

        activeQuests.Add(new WinStageQuest("0011",
           LocalizationManager.instance.GetLocalizedText("quest_003_1", 99),
           199, 99)
        { groupId = "WinStageQuest" });
        activeQuests.Add(new WinStageQuest("0012",
          LocalizationManager.instance.GetLocalizedText("quest_003_1", 199),
          199, 199)
        { groupId = "WinStageQuest" });
        activeQuests.Add(new WinStageQuest("0013",
          LocalizationManager.instance.GetLocalizedText("quest_003_1", 299),
          199, 299)
        { groupId = "WinStageQuest" });
        activeQuests.Add(new WinStageQuest("0014",
          LocalizationManager.instance.GetLocalizedText("quest_003_1",399),
          199, 399)
        { groupId = "WinStageQuest" });
        activeQuests.Add(new WinStageQuest("0015",
          LocalizationManager.instance.GetLocalizedText("quest_003_1", 499),
          199, 499)
        { groupId = "WinStageQuest" });

        activeQuests.Add(new ReachStageQuest("0016",
         LocalizationManager.instance.GetLocalizedText("quest_005_1", 50),
         100, 50,"")
        { groupId = "ReachStageQuest" });
        activeQuests.Add(new ReachStageQuest("0017",
        LocalizationManager.instance.GetLocalizedText("quest_005_1", 100),
        100, 100, "")
        { groupId = "ReachStageQuest" });


        activeQuests.Add(new OwnRiderQuest("0018",
        LocalizationManager.instance.GetLocalizedText("quest_006_1", 3),
        499, 3)
        { groupId = "OwnRiderQuest" });

        activeQuests.Add(new DailyGiftQuest("0019",
         LocalizationManager.instance.GetLocalizedText("quest_012_1", 3),
         99, 3)
        { groupId = "DailyGiftQuest" });
        activeQuests.Add(new DailyGiftQuest("0020",
         LocalizationManager.instance.GetLocalizedText("quest_012_1", 13),
         99, 13)
        { groupId = "DailyGiftQuest" });
        activeQuests.Add(new DailyGiftQuest("0021",
         LocalizationManager.instance.GetLocalizedText("quest_012_1", 23),
         99, 23)
        { groupId = "DailyGiftQuest" });
        activeQuests.Add(new DailyGiftQuest("0022",
         LocalizationManager.instance.GetLocalizedText("quest_012_1", 33),
         99, 33)
        { groupId = "DailyGiftQuest" });
        activeQuests.Add(new DailyGiftQuest("0023",
       LocalizationManager.instance.GetLocalizedText("quest_012_1", 43),
       99, 43)
        { groupId = "DailyGiftQuest" });

        activeQuests.Add(new OwnRiderQuest("0024",
       LocalizationManager.instance.GetLocalizedText("quest_006_1", 4),
       499, 4)
        { groupId = "OwnRiderQuest" });
        activeQuests.Add(new ReachStageQuest("0025",
        LocalizationManager.instance.GetLocalizedText("quest_005_1", 150),
        100, 150, "")
        { groupId = "ReachStageQuest" });

        activeQuests.Add(new CollectGoldQuest("0026",
      LocalizationManager.instance.GetLocalizedText("quest_009_1_2", 1000, "hapi"),
      200, 1000)
        { groupId = "CollectGoldQuest" });
        activeQuests.Add(new CollectGoldQuest("0027",
     LocalizationManager.instance.GetLocalizedText("quest_009_1_2", 2000, "hapi"),
     200, 2000)
        { groupId = "CollectGoldQuest" });

        activeQuests.Add(new CollectGoldQuest("0028",
     LocalizationManager.instance.GetLocalizedText("quest_009_1_2", 5000, "hapi"),
     200, 5000)
        { groupId = "CollectGoldQuest" });
        activeQuests.Add(new CollectGoldQuest("0029",
  LocalizationManager.instance.GetLocalizedText("quest_009_1_2", 8000, "hapi"),
  200, 8000)
        { groupId = "CollectGoldQuest" });

        activeQuests.Add(new CollectGoldQuest("0030",
      LocalizationManager.instance.GetLocalizedText("quest_009_1_2", 15000, "hapi"),
      200, 15000)
        { groupId = "CollectGoldQuest" });

        activeQuests.Add(new ReachStageQuest("0031",
        LocalizationManager.instance.GetLocalizedText("quest_005_1", 200),
        100, 200, "")
        { groupId = "ReachStageQuest" });
        foreach (var quest in activeQuests)
        {
            quest.LoadQuest();
        }
      //  OnPlayerLogin();

    }
    private void FixedUpdate()
    {
        foreach(var quest in activeQuests)
        {
            if (quest.CheckCompletion())
            {
                Debug.Log($"Quest {quest.questId} completed!");
            }
        }
    }
    public void LoadQuestData()
    {
        foreach (var quest in activeQuests)
        {
            quest.LoadQuest();
        }
    }
    public void ReloadQuestDes()
    {
        activeQuests.Clear();
        LoadQuests();

    }
    public int GetTotalComplete()
    {
        int count = 0;
        foreach (var quest in activeQuests)
        {
            if (quest.CheckCompletion())
            {

                count++;
            }
        }
        return count;
    }
    public List<QuestBase> GetFirstUncompletedQuestEachGroup()
    {
        return activeQuests
            .Where(q => !q.isReward) // Chưa nhận thưởng
            .GroupBy(q => q.groupId) // Gom nhóm theo groupId
            .Select(g => g.OrderBy(q => q.questId).First()) // Lấy nhiệm vụ nhỏ nhất trong mỗi nhóm
            .ToList();
    }
    public QuestBase GetQuestById(string id)
    {
        return activeQuests.FirstOrDefault(h => h.questId == id);
    }
    public void UpdateQuest(string id, int progress, int progress2)
    {
        foreach (var quest in activeQuests)
        {
            if (quest.questId == id)
            {
                quest.UpdateProgress(progress);
                quest.UpdateProgress(progress, progress2);
                if (quest.CheckCompletion())
                {
                    Debug.Log($"Quest {quest.questId} completed!");
                
                }
                return;
            }
        }

        Debug.LogWarning($"Quest with ID {id} not found!");
    }
    public void OnPlayerLogin()
    {
        foreach (var quest in activeQuests)
        {
            if (quest is LoginStreakQuest loginQuest)
            {
                loginQuest.OnPlayerLogin();
                if (loginQuest.CheckCompletion())
                {
                    Debug.Log($"✅ Login Quest {loginQuest.questId} hoàn thành!");
                }
            }
        }
        foreach (var quest in activeQuests)
        {
            if (quest is LoginNormalQuest loginQuest)
            {
                loginQuest.OnPlayerLogin();
                if (loginQuest.CheckCompletion())
                {
                    Debug.Log($"✅ Login Quest {loginQuest.questId} hoàn thành!");
                }
            }
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
    public async void SyncLocalQuestsToFirebaseIfNotExist()
    {
        foreach (var quest in activeQuests)
        {
            quest.SyncLocalQuestsToFirebaseIfNotExist();
        }
         LoadQuests();
        await Task.Delay(2500);
        OnPlayerLogin();
    }
}
