public class ReachStageQuest : QuestBase
{
    public int requiredStage;
    public string requiredMode;
    private bool reached = false;

    public ReachStageQuest(string id, string desc, string reward, int stage, string mode)
    {
        questId = id;
        description = desc;
        rewardId = reward;
        requiredStage = stage;
        requiredMode = mode;
    }

    public void MarkReached()
    {
        reached = true;
    }

    public override bool CheckCompletion()
    {
        return reached;
    }
}
