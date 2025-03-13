using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<QuestBase> activeQuests = new List<QuestBase>();

    void Start()
    {
        LoadQuests();
    }

    void LoadQuests()
    {
        // Tạo nhiệm vụ từ JSON hoặc hardcode
        activeQuests.Add(new LoginQuest("001", "Log in 5 days and 3 consecutive days", "REWARD_001", 5, 3));
        activeQuests.Add(new PlayStageQuest("002", "Play stage 3 times", "REWARD_002", 3, "", ""));
        activeQuests.Add(new WinStageQuest("003", "Win 5 times", "REWARD_003", 5));
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
}

