public class PlayStageQuest : QuestBase
{
    public int requiredPlays;
    public string requiredRider;
    public string requiredMode;
    public int currentPlays;

    public PlayStageQuest(string id, string desc, int reward, int plays, string rider, string mode)
    {
        questId = id;
        description = desc;
        base.reward = reward;
        requiredPlays = plays;
        requiredRider = rider;
        requiredMode = mode;
    }

    public override void UpdateProgress(int plays)
    {
        currentPlays += plays;
        SaveQuest();
    }
    public override void UpdateProgress(int count,int riderId)
    {
        if (riderId.ToString() == requiredRider)
        {
            currentPlays += count;
            SaveQuest();
        }
    }

    public override bool CheckCompletion()
    {
        return currentPlays >= requiredPlays;
    }
}
