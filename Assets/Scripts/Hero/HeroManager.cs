using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
[Serializable]
public struct DataHero
{
    public int id;
    public int price;
    public string name;
    public int hp;
    public Sprite icon;
    public bool isUnlock;
    public Sprite heroImage;
    public string skillDescription;
    public string story;
    public int level;
}

[System.Serializable]
public class Serialization<T>
{
    public T target;
    public Serialization(T target)
    {
        this.target = target;
    }
}
[System.Serializable]
public struct HeroProgress
{
    public int id;
    public int hp;
    public int level;

    public HeroProgress(int id, int hp, int level)
    {
        this.id = id;
        this.hp = hp;
        this.level = level;
    }
}

[System.Serializable]
public class HeroProgressList
{
    public List<HeroProgress> heroProgresses;

    public HeroProgressList(List<HeroProgress> progresses)
    {
        heroProgresses = progresses;
    }
}

public class HeroManager : MonoBehaviour
{
     public List<DataHero> heroDatas;
     public static HeroManager instance;
    public Dictionary<int, int> heroLevels = new(); // heroId -> level

    void Awake()
     {
        instance = this;
       LoadUnlockHero();
        LoadHeroesData();


     }
    public DataHero? GetHero(int id)
    {
        return heroDatas.FirstOrDefault(h => h.id == id);
    }
    public int HeroOwnedQuantity()
    {
        int unlockedCount = heroDatas.Count(hero => hero.isUnlock);
        return unlockedCount;
    }
    public List<DataHero> GetUnlockHero()
    {
        return heroDatas.Where(h=>h.isUnlock).ToList();
    }
    public UnlockHeroData GetUnlockHeroID()
    {
        List<int> ids = heroDatas.Where(h => h.isUnlock).Select(h => h.id).ToList();
        return new UnlockHeroData(ids);
    }
    public void LoadCloudUnlockHero()
    {
        FirebaseDataManager.Instance.LoadPlayerData((loadedData) =>
        {
            foreach (int id in loadedData.unlockHeroData.seenHeroIds)
            {
                UnlockHero(id);
            }
        });
    }
    public void UnlockHero(int id)
    {
        int index = heroDatas.FindIndex(h => h.id == id);
        if (index != -1)
        {
            DataHero hero = heroDatas[index]; 
            hero.isUnlock = true;           
            heroDatas[index] = hero;
            SaveUnlockHero();

        }
        else
        {
            Debug.LogError($"Hero với ID {id} không tồn tại!");
        }
    }
    public void SaveUnlockHero()
    {
        List<int> seenObjectIds = heroDatas.Where(h => h.isUnlock).Select(h => h.id).ToList();
        string json = JsonUtility.ToJson(new UnlockHeroData(seenObjectIds));
        File.WriteAllText(Application.persistentDataPath + "/unlockHeros.json", json);
    }
    public void LoadUnlockHero()
    {
        string path = Application.persistentDataPath + "/unlockHeros.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            UnlockHeroData data = JsonUtility.FromJson<UnlockHeroData>(json);
            if (data.seenHeroIds.Count == 0)
            {
                data.seenHeroIds.Add(1001);

                data.seenHeroIds.Add(1002);
            }
            foreach (int id in data.seenHeroIds)
            {
                UnlockHero(id);
            }
        }
    }

    public void SaveHeroesData()
    {
        List<HeroProgress> progressList = heroDatas.Select(h => new HeroProgress(h.id, h.hp, h.level)).ToList();
        string json = JsonUtility.ToJson(new HeroProgressList(progressList));
        File.WriteAllText(Application.persistentDataPath + "/heroesData.json", json);
    }



    public void LoadHeroesData()
    {
        string path = Application.persistentDataPath + "/heroesData.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            HeroProgressList data = JsonUtility.FromJson<HeroProgressList>(json);

            foreach (var progress in data.heroProgresses)
            {
                int index = heroDatas.FindIndex(h => h.id == progress.id);
                if (index != -1)
                {
                    DataHero hero = heroDatas[index];
                    hero.hp = progress.hp;
                    hero.level = progress.level;
                    heroDatas[index] = hero;
                }
            }
        }
    }

    public void OnUpgradeHeroButtonClicked(int heroId)
    {
        bool success = UpgradeManager.Instance.TryUpgradeHero(heroId);

        if (success)
        {
            Debug.Log("Nâng cấp hero thành công!");
        }
        else
        {
            Debug.Log("Nâng cấp thất bại! Không đủ tài nguyên hoặc đã max cấp.");
        }
    }
}
