public class OwnRiderQuest : QuestBase
{
    public int requiredRiderCount;
    private int currentCount;

    public OwnRiderQuest(string id, string desc, int reward, int count)
    {
        questId = id;
        description = desc;
        rewardId = reward;
        requiredRiderCount = count;
    }

    public override void UpdateProgress(int count)
    {
        currentCount += count;
    }

    public override bool CheckCompletion()
    {
        return currentCount >= requiredRiderCount;
    }
}
