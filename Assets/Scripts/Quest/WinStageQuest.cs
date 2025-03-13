public class WinStageQuest : QuestBase
{
    public int requiredWins;
    private int currentWins;

    public WinStageQuest(string id, string desc, string reward, int wins)
    {
        questId = id;
        description = desc;
        rewardId = reward;
        requiredWins = wins;
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
