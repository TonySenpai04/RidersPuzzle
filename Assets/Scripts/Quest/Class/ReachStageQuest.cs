using System;

public class ReachStageQuest : QuestBase
{
    public int requiredStage;
    public string requiredMode;
    public int currentStage;


    public ReachStageQuest(string id, string desc, int reward, int stage, string mode)
    {
        questId = id;
        description = desc;
        base.reward = reward;
        requiredStage = stage;
        requiredMode = mode;
    }


    public override bool CheckCompletion()
    {
        return LevelManager.instance.GetAllLevelComplete() >= requiredStage;
    }

    public override void UpdateProgress(int update)
    {
        currentStage = update;
    }
    public override Tuple<int, int> GetProgress()
    {
        return Tuple.Create(currentStage, requiredStage);
    }
}
