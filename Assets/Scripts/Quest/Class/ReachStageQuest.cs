public class ReachStageQuest : QuestBase
{
    public int requiredStage;
    public string requiredMode;
    public bool reached = false;

    public ReachStageQuest(string id, string desc, int reward, int stage, string mode)
    {
        questId = id;
        description = desc;
        base.reward = reward;
        requiredStage = stage;
        requiredMode = mode;
    }

    public  void MarkReached()
    {
        reached = true;
    }

    public override bool CheckCompletion()
    {
        return reached;
    }

    public override void UpdateProgress(int update)
    {
        throw new System.NotImplementedException();
    }
}
