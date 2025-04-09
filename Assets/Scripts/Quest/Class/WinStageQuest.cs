
using Firebase.Database;
using Firebase.Extensions;
using System;
using UnityEngine;

public class WinStageQuest : QuestBase
{
    public int requiredWins;
    public int currentWins;

    public WinStageQuest(string id, string desc, int reward, int wins)
    {
        questId = id;
        description = desc;
        base.reward = reward;
        requiredWins = wins;
    }

    public override void UpdateProgress(int wins)
    {
        currentWins += wins;
        
        SaveQuest();
    }

    public override bool CheckCompletion()
    {
        return currentWins >= requiredWins;
    }
    public override Tuple<int, int> GetProgress()
    {
        return Tuple.Create(currentWins, requiredWins);
    }
  

}
