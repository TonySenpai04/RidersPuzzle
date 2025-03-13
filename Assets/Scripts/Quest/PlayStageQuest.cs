public class PlayStageQuest : QuestBase
{
    public int requiredPlays;
    public string requiredRider;
    public string requiredMode;
    private int currentPlays;

    public PlayStageQuest(string id, string desc, string reward, int plays, string rider, string mode)
    {
        questId = id;
        description = desc;
        rewardId = reward;
        requiredPlays = plays;
        requiredRider = rider;
        requiredMode = mode;
    }

    public void UpdateProgress(int plays)
    {
        currentPlays = plays;
    }

    public override bool CheckCompletion()
    {
        return currentPlays >= requiredPlays;
    }
}
