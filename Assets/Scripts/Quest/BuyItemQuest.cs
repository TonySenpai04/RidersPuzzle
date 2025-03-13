public class BuyItemQuest : QuestBase
{
    public int requiredItemCount;
    public string resourceUsed;
    private int currentCount;

    public BuyItemQuest(string id, string desc, string reward, int count, string resource)
    {
        questId = id;
        description = desc;
        rewardId = reward;
        requiredItemCount = count;
        resourceUsed = resource;
    }

    public void UpdateProgress(int count)
    {
        currentCount = count;
    }

    public override bool CheckCompletion()
    {
        return currentCount >= requiredItemCount;
    }
}
