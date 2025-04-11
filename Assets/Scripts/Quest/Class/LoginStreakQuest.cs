using System;
using System.IO;
using UnityEngine;

public class LoginStreakQuest : QuestBase
{
    public int requiredConsecutiveDays;
    public int currentConsecutiveDays;
    public string lastLoginDate;

    public LoginStreakQuest(string id, string desc, int reward, int consecutiveDays)
    {
        questId = id;
        description = desc;
        base.reward = reward;
        requiredConsecutiveDays = consecutiveDays;
        lastLoginDate = "";
    }
    public void OnPlayerLogin()
    {
        string today = DateTime.UtcNow.Date.ToString("yyyy-MM-dd");
        Debug.Log("lastLoginDate:" + lastLoginDate);
        if (!string.IsNullOrEmpty(lastLoginDate))
        {
            DateTime lastLogin = DateTime.Parse(lastLoginDate);
            DateTime todayDate = DateTime.Parse(today);

            if ((todayDate - lastLogin).TotalDays == 1)
            {
                currentConsecutiveDays++; // Tăng streak login
            }
            else if ((todayDate - lastLogin).TotalDays > 1)
            {
                currentConsecutiveDays = 1; // Reset streak nếu bỏ lỡ ngày
            }
        }
        else
        {
            currentConsecutiveDays = 1; // Lần đầu đăng nhập
        }

        lastLoginDate = today;
        SaveQuest();
    }
    public override void UpdateProgress(int days, int consecutiveDays)
    {
        //string today = DateTime.UtcNow.Date.ToString("yyyy-MM-dd");

        //if (!string.IsNullOrEmpty(lastLoginDate))
        //{
        //    DateTime lastLogin = DateTime.Parse(lastLoginDate);
        //    DateTime todayDate = DateTime.Parse(today);

        //    if ((todayDate - lastLogin).TotalDays == 1)
        //    {
        //        currentConsecutiveDays++; // Tăng streak login
        //    }
        //    else if ((todayDate - lastLogin).TotalDays > 1)
        //    {
        //        currentConsecutiveDays = 1; // Reset streak nếu bỏ lỡ ngày
        //    }
        //}
        //else
        //{
        //    currentConsecutiveDays = 1; // Lần đầu đăng nhập
        //}

        //lastLoginDate = today;
        //currentDays++; // Cập nhật tổng số ngày đăng nhập
        //SaveQuest();
        ////currentDays += days;
        ////currentConsecutiveDays += consecutiveDays;
        ////SaveQuest();
    }

    public override bool CheckCompletion()
    {
        return  currentConsecutiveDays >= requiredConsecutiveDays;
    }

    public override void UpdateProgress(int days)
    {
       // currentDays += days;
    }

    public override Tuple<int, int> GetProgress()
    {
        return Tuple.Create(currentConsecutiveDays, requiredConsecutiveDays);
    }
    // ✅ Override lại để load cả `progress` và `consecutiveProgress`

}
