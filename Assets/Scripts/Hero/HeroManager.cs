using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
[Serializable]
public struct DataHero
{
    public int id;
    public int price;
    public string name;
    public int hp;
    public int mp;
    public int currentMP;
    public Sprite icon;
    public bool isUnlock;
    public Sprite heroImage;
    public Sprite heroCardImage;
    public string skillDescription;
    public string story;
    public int level;

    public bool isTrial;
    public long trialExpireTimestamp;
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
    public int mp;
    public int currentMp;
    public bool isTrial;
    public long trialExpireTimestamp;

    //public HeroProgress(int id, int hp, int level,int mp,int currentMp)
    //{
    //    this.id = id;
    //    this.hp = hp;
    //    this.level = level;
    //    this.mp=mp;
    //    this.currentMp = currentMp;
    //}
    public HeroProgress(int id, int hp, int level, int mp, int currentMp, bool isTrial, long trialExpireTimestamp)
    {
        this.id = id;
        this.hp = hp;
        this.level = level;
        this.mp = mp;
        this.currentMp = currentMp;
        this.isTrial = isTrial;
        this.trialExpireTimestamp = trialExpireTimestamp;
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
    public IHeroProgressService heroProgressService;
    public IHeroTrialService heroTrialService;

    void Awake()
    {
        instance = this;
        LoadUnlockHero();
        heroProgressService = new HeroProgressService();
        heroTrialService = new HeroTrialService();
        LoadHeroesData();


    }
    private async void Start()
    {
        await Task.Delay(3000);
        StartCoroutine(CheckTrialsWhenTimeReady());
    }

    private IEnumerator CheckTrialsWhenTimeReady()
    {
        yield return new WaitUntil(() => TimeManager.Instance.IsTimeFetched);
        CheckTrials();

    
    }
    public  void CheckTrials()
    {
        heroTrialService.CheckTrials(heroDatas);
        bool changed = heroTrialService.IsChangeTrials(heroDatas);

        if (changed)
        {
            SaveHeroesData();
            SaveHeroesDataToFirebase();
        }
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
        for (int i = 0; i < heroDatas.Count; i++)
        {
            var hero = heroDatas[i];
            hero.isUnlock = false;
            heroDatas[i] = hero;
        }
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
    public void UnlockHeroPermanent(int id)
    {
        int index = heroDatas.FindIndex(h => h.id == id);
        if (index != -1)
        {
            DataHero hero = heroDatas[index];
            // Nếu đang trial thì clear trial, vì đã mua thật
            if (hero.isTrial)
            {
                hero.isTrial = false;
                hero.trialExpireTimestamp = 0;
            }
            hero.isUnlock = true;
            heroDatas[index] = hero;

            SaveUnlockHero();
        }
        else
        {
            Debug.LogError($"Hero với ID {id} không tồn tại!");
        }
    }
    public void SaveUnlockHero() {
        List<int> seenObjectIds = heroDatas.Where(h => h.isUnlock).Select(h => h.id).ToList();
        string json = JsonUtility.ToJson(new UnlockHeroData(seenObjectIds));
        File.WriteAllText(Application.persistentDataPath + "/unlockHeros.json", json);

    }
    public void LoadUnlockHero() {
        string path = Application.persistentDataPath + "/unlockHeros.json";
        if (File.Exists(path)) { string json = File.ReadAllText(path); 
            UnlockHeroData data = JsonUtility.FromJson<UnlockHeroData>(json); 
            if (data.seenHeroIds.Count <= 1)
            { 
                data.seenHeroIds.Add(1001); 
                 data.seenHeroIds.Add(1002); 
            }
            for (int i = 0; i < heroDatas.Count; i++)
            {
                var hero = heroDatas[i];
                hero.isUnlock = false;   
                heroDatas[i] = hero;
            }
            foreach (int id in data.seenHeroIds)
            {
                UnlockHero(id);
            }
        }
    
    }


    public void SaveHeroesData()
    {
        heroProgressService.SaveProgress(heroDatas);
    }

    public void LoadHeroesData()
    {
        heroProgressService.LoadProgress(heroDatas);


    }
    public void SaveHeroesDataToFirebase()
    {
        heroProgressService.SaveProgressToFirebase(heroDatas);
    

    }

    public void LoadHeroesDataFromFirebase()
    {
        heroProgressService.LoadProgressFromFirebase(heroDatas);

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
    public void TestTrial()
    {
       heroTrialService.StartTrialSeconds(heroDatas,1005, 30);
    }

    #region HERO TRIAL
    public void StartHeroTrial(int heroId, int durationDays)
    {
         heroTrialService.StartTrial(heroDatas, heroId,durationDays);
        SaveHeroesData();
        SaveHeroesDataToFirebase();
    }


    public void CheckHeroTrials()
    {
        heroTrialService.CheckTrials(heroDatas);
        SaveHeroesData();
        SaveHeroesDataToFirebase();
    }


    public string GetTrialRemainingTime(int heroId)
    {
        return heroTrialService.GetTrialRemainingTime(heroDatas, heroId);
    }

    public List<(DataHero hero, string remainingTime)> GetAllTrialHeroes()
    {
        List<(DataHero hero, string remainingTime)> trials = new();
        foreach (var hero in heroDatas)
        {
            if (hero.isTrial)
            {
                string timeStr = GetTrialRemainingTime(hero.id);
                trials.Add((hero, timeStr));
            }
        }
        return trials;
    }
    #endregion
    public bool IsTrialHero(int heroId)
    {
      return  heroTrialService.IsTrialHero(heroDatas, heroId);
    }

    public bool IsTrialExpired(int heroId)
    {
        return heroTrialService.IsTrialExpired(heroDatas, heroId);

    }
}
