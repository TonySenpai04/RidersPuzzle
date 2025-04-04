using System;

public class OwnRiderQuest : QuestBase
{
    public int requiredRiderCount;
    public int currentCount;

    public OwnRiderQuest(string id, string desc, int reward, int count)
    {
        questId = id;
        description = desc;
        base.reward = reward;
        requiredRiderCount = count;
    }

    public override void UpdateProgress(int count)
    {
        currentCount = count;
        SaveQuest();
    }

    public override bool CheckCompletion()
    {
        return HeroManager.instance.GetUnlockHero().Count >= requiredRiderCount;
    }
    public override Tuple<int, int> GetProgress()
    {
        return Tuple.Create(HeroManager.instance.GetUnlockHero().Count, requiredRiderCount);
    }
}
