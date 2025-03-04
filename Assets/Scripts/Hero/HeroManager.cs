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
}
[Serializable]
public class UnlockHeroData
{
    public List<int> seenHeroIds;

    public UnlockHeroData(List<int> ids)
    {
        seenHeroIds = ids;
    }
}
public class HeroManager : MonoBehaviour
{
     public List<DataHero> heroDatas;
     public static HeroManager instance;
     void Awake()
     {
        instance = this;
        LoadUnlockHero();
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
            foreach (int id in data.seenHeroIds)
            {
                UnlockHero(id);
            }
        }
    }


}
