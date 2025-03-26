
public class WinStageQuest : QuestBase
{
    public int requiredWins;
    public int currentWins;

    public WinStageQuest(string id, string desc, int reward, int wins)
    {
        questId = id;
        description = desc;
        rewardId = reward;
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
}
