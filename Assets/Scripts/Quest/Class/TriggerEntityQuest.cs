public class TriggerEntityQuest : QuestBase
{
    public int objectId;
    public int requiredTriggerCount;
    public string requiredMode;
    public int currentTriggers;

    public TriggerEntityQuest(string id, string desc, int reward, int objId, int triggers, string mode)
    {
        questId = id;
        description = desc;
        base.reward = reward;
        objectId = objId;
        requiredTriggerCount = triggers;
        requiredMode = mode;
    }

    public override void UpdateProgress(int triggers,int id)
    {
        if (id == objectId)
        {
            currentTriggers += triggers;
            SaveQuest();
        }
    }

    public override bool CheckCompletion()
    {
        return currentTriggers >= requiredTriggerCount;
    }

    public override void UpdateProgress(int progress)
    {
        
    }
}
