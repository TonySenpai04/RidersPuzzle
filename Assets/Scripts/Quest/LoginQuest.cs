public class LoginQuest : QuestBase
{
    public int requiredDays;
    public int requiredConsecutiveDays;
    private int currentDays;
    private int currentConsecutiveDays;

    public LoginQuest(string id, string desc, string reward, int days, int consecutiveDays)
    {
        questId = id;
        description = desc;
        rewardId = reward;
        requiredDays = days;
        requiredConsecutiveDays = consecutiveDays;
    }

    public void UpdateProgress(int days, int consecutiveDays)
    {
        currentDays = days;
        currentConsecutiveDays = consecutiveDays;
    }

    public override bool CheckCompletion()
    {
        return currentDays >= requiredDays && currentConsecutiveDays >= requiredConsecutiveDays;
    }
}
