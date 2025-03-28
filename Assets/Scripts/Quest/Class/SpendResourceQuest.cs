public class SpendResourceQuest : QuestBase
{
    public int requiredAmount;
    public string resourceType;
    public string requiredMode;
    public int currentAmount;

    public SpendResourceQuest(string id, string desc, int reward, int amount, string type, string mode)
    {
        questId = id;
        description = desc;
        base.reward = reward;
        requiredAmount = amount;
        resourceType = type;
        requiredMode = mode;
    }

    public override void UpdateProgress(int amount)
    {
        currentAmount = amount;
        SaveQuest();
    }

    public override bool CheckCompletion()
    {
        return currentAmount >= requiredAmount;
    }
}
