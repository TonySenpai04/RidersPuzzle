using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyGiftQuest : QuestBase
{
    public int requiredItemCount;
    public int currentCount;

    public DailyGiftQuest(string id, string desc, int reward, int count)
    {
        questId = id;
        description = desc;
        base.reward = reward;
        requiredItemCount = count;
    }

    public override void UpdateProgress(int count)
    {
        currentCount += count;
        SaveQuest();
    }

    public override bool CheckCompletion()
    {
        return currentCount >= requiredItemCount;
    }
    public override Tuple<int, int> GetProgress()
    {
        return Tuple.Create(currentCount, requiredItemCount);
    }
}
