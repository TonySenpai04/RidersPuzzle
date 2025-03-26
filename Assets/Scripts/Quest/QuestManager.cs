using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<QuestBase> activeQuests = new List<QuestBase>();
    public static QuestManager instance;
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
        activeQuests.Add(new LoginQuest("001", "Log in 5 days and 3 consecutive days", 10, 3, 0));
        activeQuests.Add(new PlayStageQuest("002", "Play stage 3 times", 5, 3, "", ""));
        activeQuests.Add(new WinStageQuest("003", "Win 3 times", 5, 3));
        foreach (var quest in activeQuests)
        {
            quest.LoadQuest(); 
        }
    }

    public void CheckQuests()
    {
        foreach (var quest in activeQuests)
        {
            if (quest.CheckCompletion())
            {
                
                Debug.Log($"Quest {quest.questId} completed!");
            }
        }
    }
    public void UpdateQuest(string id, int progress)
    {
        foreach (var quest in activeQuests)
        {
            if (quest.questId == id)
            {
                quest.UpdateProgress(progress);
                Debug.Log($"Updated quest {quest.questId} with progress: {progress}");

                if (quest.CheckCompletion())
                {
                    RewardQuest(id);
                    Debug.Log($"Quest {quest.questId} completed!");
                }
                return;
            }
        }
        Debug.LogWarning($"Quest with ID {id} not found!");
    }
    public void RewardQuest(string id)
    {
        foreach (var quest in activeQuests)
        {
            if (quest.questId == id && !quest.isReward && quest.CheckCompletion())
            {
                GoldManager.instance.AddGold(quest.rewardId);
                quest.isReward = true;
                return;
            }
        }
    }
}

