using System;

public class UseRiderSkillQuest : QuestBase
{
    public int requiredSkillUsage;
    public string requiredMode;
    public int currentUsage;

    public UseRiderSkillQuest(string id, string desc, int reward, int usage, string mode)
    {
        questId = id;
        description = desc;
        base.reward = reward;
        requiredSkillUsage = usage;
        requiredMode = mode;
    }

    public override void UpdateProgress(int usage)
    {
        currentUsage += usage;
        SaveQuest();
    }

    public override bool CheckCompletion()
    {
        return currentUsage >= requiredSkillUsage;
    }
    public override Tuple<int, int> GetProgress()
    {
        return Tuple.Create(currentUsage, requiredSkillUsage);
    }
}
