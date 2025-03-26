public class UseRiderSkillQuest : QuestBase
{
    public int requiredSkillUsage;
    public string requiredMode;
    private int currentUsage;

    public UseRiderSkillQuest(string id, string desc, int reward, int usage, string mode)
    {
        questId = id;
        description = desc;
        rewardId = reward;
        requiredSkillUsage = usage;
        requiredMode = mode;
    }

    public override void UpdateProgress(int usage)
    {
        currentUsage = usage;
    }

    public override bool CheckCompletion()
    {
        return currentUsage >= requiredSkillUsage;
    }
}
