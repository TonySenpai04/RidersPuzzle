using System;

public class CollectGoldQuest : QuestBase
{
    public int requiredGoldCount;
    public int currentCount;

    public CollectGoldQuest(string id, string desc, int reward, int count)
    {
        questId = id;
        description = desc;
        base.reward = reward;
        requiredGoldCount = count;
    }

    public override void UpdateProgress(int count)
    {
        currentCount += count;
        SaveQuest();
    }

    public override bool CheckCompletion()
    {
        return currentCount >= requiredGoldCount;
    }
    public override Tuple<int, int> GetProgress()
    {
        return Tuple.Create(currentCount, requiredGoldCount);
    }
}