using System.IO;
using UnityEngine;

public class LoginQuest : QuestBase
{
    public int requiredDays;
    public int requiredConsecutiveDays;
    public int currentDays;
    public int currentConsecutiveDays;

    public LoginQuest(string id, string desc, int reward, int days, int consecutiveDays)
    {
        questId = id;
        description = desc;
        base.reward = reward;
        requiredDays = days;
        requiredConsecutiveDays = consecutiveDays;
    }

    public override void UpdateProgress(int days, int consecutiveDays)
    {
        currentDays += days;
        currentConsecutiveDays += consecutiveDays;
        SaveQuest();
    }

    public override bool CheckCompletion()
    {
        return currentDays >= requiredDays && currentConsecutiveDays >= requiredConsecutiveDays;
    }

    public override void UpdateProgress(int days)
    {
       
    }
    public override void SaveQuest()
    {
        string path = Application.persistentDataPath + $"/quest_{questId}.json";
        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(path, json);
    }

    // ✅ Override lại để load cả `progress` và `consecutiveProgress`
    public override void LoadQuest()
    {
        string path = Application.persistentDataPath + $"/quest_{questId}.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }
}
