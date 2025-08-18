using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
public class HeroProgressService : IHeroProgressService
{
    private readonly List<DataHero> heroes;
    private readonly string savePath = Path.Combine(Application.persistentDataPath, "heroesData.json");

 

    public void SaveProgress(List<DataHero> heroes)
    {
        List<HeroProgress> progressList = heroes
            .Select(h => new HeroProgress(h.id, h.hp, h.level, h.mp, h.currentMP, h.isTrial, h.trialExpireTimestamp))
            .ToList();

        string json = JsonUtility.ToJson(new HeroProgressList(progressList));
        File.WriteAllText(savePath, json);
    }

    public void LoadProgress(List<DataHero> heroes)
    {
        if (!File.Exists(savePath)) return;

        string json = File.ReadAllText(savePath);
        HeroProgressList data = JsonUtility.FromJson<HeroProgressList>(json);

        foreach (var progress in data.heroProgresses)
        {
            int index = heroes.FindIndex(h => h.id == progress.id);
            if (index != -1)
            {
                DataHero hero = heroes[index];
                hero.hp = progress.hp;
                hero.level = progress.level;
                hero.mp = progress.mp;
                hero.currentMP = progress.currentMp;
                hero.isTrial = progress.isTrial;
                hero.trialExpireTimestamp = progress.trialExpireTimestamp;
                heroes[index] = hero;
            }
        }
    }

    public void SaveProgressToFirebase(List<DataHero> heroes)
    {
        var user = FirebaseDataManager.Instance.GetCurrentUser();
        if (user == null) return;

        List<HeroProgress> progressList = heroes
            .Select(h => new HeroProgress(h.id, h.hp, h.level, h.mp, h.currentMP, h.isTrial, h.trialExpireTimestamp))
            .ToList();

        HeroProgressList wrapper = new HeroProgressList(progressList);
        string json = JsonUtility.ToJson(wrapper);

        FirebaseDatabase.DefaultInstance
            .RootReference
            .Child("users")
            .Child(user.UserId)
            .Child("heroProgress")
            .SetRawJsonValueAsync(json);
    }

    public void LoadProgressFromFirebase(List<DataHero> heroes)
    {
        var user = FirebaseDataManager.Instance.GetCurrentUser();
        if (user == null) return;

        FirebaseDatabase.DefaultInstance
            .RootReference
            .Child("users")
            .Child(user.UserId)
            .Child("heroProgress")
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (!task.IsCompleted || !task.Result.Exists) return;

                string json = task.Result.GetRawJsonValue();
                HeroProgressList data = JsonUtility.FromJson<HeroProgressList>(json);
                foreach (var progress in data.heroProgresses)
                {
                    int index = heroes.FindIndex(h => h.id == progress.id);
                    if (index != -1)
                    {
                        DataHero hero = heroes[index];
                        hero.hp = progress.hp;
                        hero.level = progress.level;
                        hero.mp = progress.mp;
                        hero.currentMP = progress.currentMp;
                        hero.isTrial = progress.isTrial;
                        hero.trialExpireTimestamp = progress.trialExpireTimestamp;
                        heroes[index] = hero;
                    }
                }
            });
    }
}
