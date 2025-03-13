public class WinStageNoDamageQuest : QuestBase
{
    public int requiredWins;
    public string requiredRider;
    public string requiredMode;
    private int currentWins;

    public WinStageNoDamageQuest(string id, string desc, string reward, int wins, string rider, string mode)
    {
        questId = id;
        description = desc;
        rewardId = reward;
        requiredWins = wins;
        requiredRider = rider;
        requiredMode = mode;
    }

    public void UpdateProgress(int wins)
    {
        currentWins = wins;
    }

    public override bool CheckCompletion()
    {
        return currentWins >= requiredWins;
    }
}
