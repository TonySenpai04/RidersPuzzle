public class TriggerEntityQuest : QuestBase
{
    public string objectId;
    public int requiredTriggerCount;
    public string requiredMode;
    private int currentTriggers;

    public TriggerEntityQuest(string id, string desc, int reward, string objId, int triggers, string mode)
    {
        questId = id;
        description = desc;
        rewardId = reward;
        objectId = objId;
        requiredTriggerCount = triggers;
        requiredMode = mode;
    }

    public override void UpdateProgress(int triggers)
    {
        currentTriggers = triggers;
    }

    public override bool CheckCompletion()
    {
        return currentTriggers >= requiredTriggerCount;
    }
}
