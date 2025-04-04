using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        LoadQuests();
    }

    void LoadQuests()
    {
        activeQuests.Add(new LoginNormalQuest("0001",
                  LocalizationManager.instance.GetLocalizedText("quest_001_1",7),
                  288, 7));
        activeQuests.Add(new LoginNormalQuest("0002",
                 LocalizationManager.instance.GetLocalizedText("quest_001_1", 14),
                 288, 14));
        activeQuests.Add(new LoginNormalQuest("0003",
            LocalizationManager.instance.GetLocalizedText("quest_001_1", 21),
            288, 21));
        activeQuests.Add(new LoginNormalQuest("0004",
                 LocalizationManager.instance.GetLocalizedText("quest_001_1", 28),
                 288, 28));
        activeQuests.Add(new LoginNormalQuest("0005",
                 LocalizationManager.instance.GetLocalizedText("quest_001_1", 35),
                 288, 35));

        activeQuests.Add(new LoginStreakQuest("0006",
                LocalizationManager.instance.GetLocalizedText("quest_001_2", 7),
                688, 7));
        activeQuests.Add(new LoginStreakQuest("0007",
                 LocalizationManager.instance.GetLocalizedText("quest_001_2", 14),
                 688, 14));
        activeQuests.Add(new LoginStreakQuest("0008",
               LocalizationManager.instance.GetLocalizedText("quest_001_2", 21),
               688, 21));
        activeQuests.Add(new LoginStreakQuest("0009",
                 LocalizationManager.instance.GetLocalizedText("quest_001_2", 28),
                 688, 28));
        activeQuests.Add(new LoginStreakQuest("0010",
                 LocalizationManager.instance.GetLocalizedText("quest_001_2", 35),
                 688, 35));

        activeQuests.Add(new WinStageQuest("0011",
           LocalizationManager.instance.GetLocalizedText("quest_003_1", 99),
           199, 99));
        activeQuests.Add(new WinStageQuest("0012",
          LocalizationManager.instance.GetLocalizedText("quest_003_1", 199),
          199, 199));
        activeQuests.Add(new WinStageQuest("0013",
          LocalizationManager.instance.GetLocalizedText("quest_003_1", 299),
          199, 299));
        activeQuests.Add(new WinStageQuest("0014",
          LocalizationManager.instance.GetLocalizedText("quest_003_1",399),
          199, 399));
        activeQuests.Add(new WinStageQuest("0015",
          LocalizationManager.instance.GetLocalizedText("quest_003_1", 499),
          199, 499));

        activeQuests.Add(new ReachStageQuest("0016",
         LocalizationManager.instance.GetLocalizedText("quest_005_1", 50),
         100, 50,""));
        activeQuests.Add(new ReachStageQuest("0017",
        LocalizationManager.instance.GetLocalizedText("quest_005_1", 100),
        100, 100, ""));


        activeQuests.Add(new OwnRiderQuest("0018",
        LocalizationManager.instance.GetLocalizedText("quest_006_1", 3),
        499, 3));

        activeQuests.Add(new DailyGiftQuest("0019",
         LocalizationManager.instance.GetLocalizedText("quest_012_1", 3),
         99, 3));
        activeQuests.Add(new DailyGiftQuest("0020",
         LocalizationManager.instance.GetLocalizedText("quest_012_1", 13),
         99, 13));
        activeQuests.Add(new DailyGiftQuest("0021",
         LocalizationManager.instance.GetLocalizedText("quest_012_1", 23),
         99, 23));
        activeQuests.Add(new DailyGiftQuest("0022",
         LocalizationManager.instance.GetLocalizedText("quest_012_1", 33),
         99, 33));
        activeQuests.Add(new DailyGiftQuest("0023",
       LocalizationManager.instance.GetLocalizedText("quest_012_1", 43),
       99, 43));
        foreach (var quest in activeQuests)
        {
            quest.LoadQuest();
            Debug.Log(quest.questId + "-" + quest.description);
        }
        OnPlayerLogin();

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

}
