using Firebase.Auth;
using Firebase.Database;
using System;
using System.IO;
using UnityEngine;

internal class LoginNormalQuest : QuestBase
{
    public int requiredDays;
    public int currentDays;
    public string lastLoginDate;

    public LoginNormalQuest(string id, string desc, int reward, int days)
    {
        questId = id;
        description = desc;
        base.reward = reward;
        requiredDays = days;
        lastLoginDate = "";
    }
    public async void OnPlayerLogin()
    {
        string today = TimeManager.Instance.ServerDate;
       
        string key = "questData";
        FirebaseUser currentUser = FirebaseDataManager.Instance.GetCurrentUser();
        if (currentUser != null)
        {
            string userId = FirebaseDataManager.Instance.GetCurrentUser().UserId;
            DataSnapshot snapshot = await FirebaseDatabase.DefaultInstance.RootReference
                .Child("users")
                .Child(userId)
                .Child(key)
                .GetValueAsync();

            if (snapshot.Exists)
            {
                string json = snapshot.GetRawJsonValue();
                QuestData data = JsonUtility.FromJson<QuestData>(json);
                lastLoginDate = data.lastAssignedDate;


            }
        }
        if (lastLoginDate != today)
        {
            currentDays++;
            lastLoginDate = today;
            Debug.Log("lastLoginDate:"+lastLoginDate);
            SaveQuest();
        }
        else
        {
            Debug.Log($"[LoginNormalQuest] Đã cộng điểm hôm nay rồi ({today}), không cộng lại.");
        }
    }
    public override void UpdateProgress(int days, int consecutiveDays)
    {
    }

    public override bool CheckCompletion()
    {
        return currentDays >= requiredDays ;
    }

    public override void UpdateProgress(int days)
    {
        // currentDays += days;
    }

    public override Tuple<int, int> GetProgress()
    {
        return Tuple.Create(currentDays, requiredDays);
    }
    // ✅ Override lại để load cả `progress` và `consecutiveProgress`

}