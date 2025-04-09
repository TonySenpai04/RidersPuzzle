using System;

public class WinStageNoDamageQuest : QuestBase
{
    public int requiredWins;
    public string requiredRider;
    public string requiredMode;
    public int currentWins;

    public WinStageNoDamageQuest(string id, string desc, int reward, int wins, string rider, string mode)
    {
        questId = id;
        description = desc;
        base.reward = reward;
        requiredWins = wins;
        requiredRider = rider;
    }

    public override void UpdateProgress(int wins)
    {
        if (PlayerController.instance.hitPoint.GetCurrentHealth() == PlayerController.instance.hitPoint.GetMaxHealth())
        {
            currentWins += wins;
            SaveQuest();
        }
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
