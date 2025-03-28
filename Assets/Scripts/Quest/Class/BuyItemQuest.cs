public class BuyItemQuest : QuestBase
{
    public int requiredItemCount;
    public string resourceUsed;
    public int currentCount;

    public BuyItemQuest(string id, string desc, int reward, int count, string resource)
    {
        questId = id;
        description = desc;
        base.reward = reward;
        requiredItemCount = count;
        resourceUsed = resource;
    }

    public override void UpdateProgress(int count)
    {
        currentCount = count;
        SaveQuest();
    }

    public override bool CheckCompletion()
    {
        return currentCount >= requiredItemCount;
    }
}
