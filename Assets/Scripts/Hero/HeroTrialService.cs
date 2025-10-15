using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using System.Reflection;

public class HeroTrialService : IHeroTrialService
{

    public void StartTrial(List<DataHero> _heroes, int heroId, int durationDays)
    {
        if (!TimeManager.Instance.IsTimeFetched) return;

        int index = _heroes.FindIndex(h => h.id == heroId);
        if (index == -1) return;

        DataHero hero = _heroes[index];

        if (hero.isUnlock && !hero.isTrial)
        {
            Debug.Log($"⚠️ Hero {heroId} đã được sở hữu vĩnh viễn, không thể trial nữa.");
            return;
        }

        if (hero.isTrial)
        {
            hero.trialExpireTimestamp += durationDays * 24 * 60 * 60; // cộng thêm số giây tương ứng
            Debug.Log($"🔄 Hero {heroId} đang trial, được cộng thêm {durationDays} ngày. Hết hạn mới:" +
                $" {UnixTimeToDate(hero.trialExpireTimestamp)}");
        }
        else
        {
            // Nếu chưa trial bao giờ thì bắt đầu mới
            hero.isTrial = true;
            hero.level = 1;
            var levelData = ReadCSVDataHeroStat.instance.GetHeroLevelData(hero.id, hero.level);
            if (levelData != null)
            {
                hero.hp = levelData.hp;
                hero.currentMP = levelData.masteryPoint;
                hero.mp = levelData.masteryPoint;
            }
            hero.trialExpireTimestamp =
                new DateTimeOffset(TimeManager.Instance.ServerDateTime.AddDays(durationDays)).ToUnixTimeSeconds();

            Debug.Log($"✅ Hero {heroId} bắt đầu trial trong {durationDays} ngày. Hết hạn: {UnixTimeToDate(hero.trialExpireTimestamp)}");
        }

        _heroes[index] = hero;
        UpdateHero(_heroes, hero);
        Debug.Log($"✅ Hero {heroId} bắt đầu trial trong {durationDays} ngày.");
    }
    private string UnixTimeToDate(long unixTime)
    {
        return DateTimeOffset.FromUnixTimeSeconds(unixTime)
            .ToLocalTime()
            .ToString("dd/MM/yyyy HH:mm:ss");
    }

    public void CheckTrials(List<DataHero> _heroes)
    {
        if (!TimeManager.Instance.IsTimeFetched) return;

        long now = new DateTimeOffset(TimeManager.Instance.ServerDateTime).ToUnixTimeSeconds();
        for (int i = 0; i < _heroes.Count; i++)
        {
            var hero = _heroes[i];
            if (hero.isTrial && now > hero.trialExpireTimestamp)
            {
                hero.isTrial = false;
                hero.isUnlock = false;
                hero.trialExpireTimestamp = 0;
                hero.level = 0;
                var levelData = ReadCSVDataHeroStat.instance.GetHeroLevelData(hero.id, hero.level);
                if (levelData != null)
                {
                    hero.hp = levelData.hp;
                    hero.currentMP = levelData.masteryPoint;
                    hero.mp = levelData.masteryPoint;
                }
                _heroes[i] = hero;
                Debug.Log($"⛔ Trial Hero {hero.id} đã hết hạn.");
            }
        }
    }

    public bool IsTrialHero(List<DataHero> _heroes, int heroId) =>
        _heroes.Any(h => h.id == heroId && h.isTrial);

    public bool IsTrialExpired(List<DataHero> _heroes, int heroId)
    {
        if (!TimeManager.Instance.IsTimeFetched) return false;
        var hero = _heroes.FirstOrDefault(h => h.id == heroId);
        if (!hero.isTrial) return false;

        long now = new DateTimeOffset(TimeManager.Instance.ServerDateTime).ToUnixTimeSeconds();
        return now > hero.trialExpireTimestamp;
    }

    public string GetTrialRemainingTime(List<DataHero> _heroes, int heroId)
    {
        if (!TimeManager.Instance.IsTimeFetched) return "";
        var hero = _heroes.FirstOrDefault(h => h.id == heroId);
        if (!hero.isTrial) return "";

        long now = new DateTimeOffset(TimeManager.Instance.ServerDateTime).ToUnixTimeSeconds();
        long remaining = hero.trialExpireTimestamp - now;
        if (remaining <= 0) return "Expired";

        var ts = TimeSpan.FromSeconds(remaining);

        int days = (int)ts.TotalDays;
        int hours = ts.Hours;

        return $"{days} day{(days != 1 ? "s" : "")} {hours} hour{(hours != 1 ? "s" : "")}";
    }
    public string GetTrialRemainingTime_Full(List<DataHero> _heroes, int heroId)
    {
        if (!TimeManager.Instance.IsTimeFetched) return "";
        var hero = _heroes.FirstOrDefault(h => h.id == heroId);
        if (!hero.isTrial) return "";

        long now = new DateTimeOffset(TimeManager.Instance.ServerDateTime).ToUnixTimeSeconds();
        long remaining = hero.trialExpireTimestamp - now;
        if (remaining <= 0) return "Expired";


        var ts = TimeSpan.FromSeconds(remaining);

        int days = (int)ts.TotalDays;
        int hours = ts.Hours;
        int minutes = ts.Minutes;

        if (days >= 1)
            // Sử dụng dòng "time_remaining_detail_day" trong CSV
            return LocalizationManager.instance.GetLocalizedText("time_remaining_detail_day", days, hours);
        else
            // Sử dụng dòng "time_remaining_detail_hours" trong CSV
            return LocalizationManager.instance.GetLocalizedText("time_remaining_detail_hours", hours, minutes);

    }

    public string GetTrialRemainingTime_Short(List<DataHero> _heroes, int heroId)
    {
        if (!TimeManager.Instance.IsTimeFetched) return "";
        var hero = _heroes.FirstOrDefault(h => h.id == heroId);
        if (!hero.isTrial) return "";

        long now = new DateTimeOffset(TimeManager.Instance.ServerDateTime).ToUnixTimeSeconds();
        long remaining = hero.trialExpireTimestamp - now;
        if (remaining <= 0) return "Expired";

        var ts = TimeSpan.FromSeconds(remaining);

        int days = (int)ts.TotalDays;
        int hours = ts.Hours;
        int minutes = ts.Minutes;

        if (days >= 1)
            // Dùng dòng "time_trail_day" trong file CSV
            return LocalizationManager.instance.GetLocalizedText("time_trail_day", days, hours);
        else
            // Dùng dòng "time_trail_hours" trong file CSV
            return LocalizationManager.instance.GetLocalizedText("time_trail_hours", hours, minutes);

    }

    private void UpdateHero(List<DataHero> _heroes, DataHero updated)
    {
        int index = _heroes.FindIndex(h => h.id == updated.id);
        if (index != -1) _heroes[index] = updated;
    }
    public void StartTrialSeconds(List<DataHero> _heroes, int heroId, int durationSeconds)
    {
        if (!TimeManager.Instance.IsTimeFetched) return;

        int index = _heroes.FindIndex(h => h.id == heroId);
        if (index == -1) return;

        DataHero hero = _heroes[index];

        if (hero.isUnlock && !hero.isTrial)
        {
            Debug.Log($"⚠️ Hero {heroId} đã sở hữu vĩnh viễn, không thể trial nữa.");
            return;
        }

        hero.isTrial = true;
        hero.trialExpireTimestamp =
            new DateTimeOffset(TimeManager.Instance.ServerDateTime.AddSeconds(durationSeconds)).ToUnixTimeSeconds();

        _heroes[index] = hero;
        UpdateHero(_heroes, hero);

        Debug.Log($"✅ Hero {heroId} bắt đầu trial trong {durationSeconds} giây. Hết hạn: {UnixTimeToDate(hero.trialExpireTimestamp)}");
    }
    public bool IsChangeTrials(List<DataHero> heroDatas)
    {
        bool hasChanged = false;

        for (int i = 0; i < heroDatas.Count; i++)
        {
            var hero = heroDatas[i];
            if (hero.isTrial && IsTrialExpired(heroDatas, hero.id))
            {
                hero.isTrial = false;
                hero.isUnlock = false;
                heroDatas[i] = hero; // nếu là struct
                hasChanged = true;
            }
        }

        return hasChanged;
    }


}
