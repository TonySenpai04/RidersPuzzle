public class EarnResourceQuest : QuestBase
{
    public int requiredAmount;
    public string resourceType;
    public string requiredMode;
    private int currentAmount;

    public EarnResourceQuest(string id, string desc, int reward, int amount, string type, string mode)
    {
        questId = id;
        description = desc;
        rewardId = reward;
        requiredAmount = amount;
        resourceType = type;
        requiredMode = mode;
    }

    public override void UpdateProgress(int amount)
    {
        currentAmount = amount;
    }

    public override bool CheckCompletion()
    {
        return currentAmount >= requiredAmount;
    }
}
