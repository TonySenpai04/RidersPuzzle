using UnityEngine;

public abstract class QuestBase
{
    public string questId;
    public string description;
    public string rewardId;

    public abstract bool CheckCompletion();
}
