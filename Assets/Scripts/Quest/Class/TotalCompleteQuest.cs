using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalCompleteQuest : QuestBase
{

    public int requiredComplete;
    public int currentComplete;

    public TotalCompleteQuest(string id, string desc, int reward, int total)
    {
        questId = id;
        description = desc;
        base.reward = reward;
        requiredComplete = total;

    }

    public override void UpdateProgress(int plays)
    {
        currentComplete = plays;
        SaveQuest();
    }

    public override bool CheckCompletion()
    {
        return currentComplete >= requiredComplete;
    }
}
