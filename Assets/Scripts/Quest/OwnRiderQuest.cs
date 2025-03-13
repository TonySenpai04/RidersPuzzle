public class OwnRiderQuest : QuestBase
{
    public int requiredRiderCount;
    private int currentCount;

    public OwnRiderQuest(string id, string desc, string reward, int count)
    {
        questId = id;
        description = desc;
        rewardId = reward;
        requiredRiderCount = count;
    }

    public void UpdateProgress(int count)
    {
        currentCount = count;
    }

    public override bool CheckCompletion()
    {
        return currentCount >= requiredRiderCount;
    }
}
