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
        string json = JsonUtility.ToJson(new Serialization<List<DataHero>>(heroDatas));
        File.WriteAllText(Application.persistentDataPath + "/heroesData.json", json);
    }


    public void LoadHeroesData()
    {
        string path = Application.persistentDataPath + "/heroesData.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            // Tải lại danh sách heroDatas từ file
            Serialization<List<DataHero>> data = JsonUtility.FromJson<Serialization<List<DataHero>>>(json);
            heroDatas = data.target;
        }
    }



    // Cập nhật cấp độ hero
    public bool TryUpgradeHero(int heroId)
    {
        // Tìm hero trong heroDatas
        var index = heroDatas.FindIndex(h => h.id == heroId);
        if (index == -1)
        {
            Debug.Log("Hero không tồn tại.");
            return false;
        }

        // Lấy hero hiện tại và cấp độ hiện tại
        DataHero hero = heroDatas[index];
        int currentLevel = hero.level;
        Debug.Log($"Cấp hiện tại của hero {heroId}: {currentLevel}");

        // Lấy dữ liệu cấp độ tiếp theo
        var nextLevelData = ReadCSVDataHeroStat.instance.GetHeroLevelData(heroId, currentLevel + 1);
        if (nextLevelData == null)
        {
            Debug.Log("Hero đã đạt cấp tối đa.");
            return false;
        }

        // Kiểm tra tài nguyên có đủ không
        foreach (var req in nextLevelData.upgradeRequirements)
        {
            if (!ResourceManager.Instance.HasEnough(req.resourceType, req.resourceId, req.amount))
            {
                Debug.Log("Không đủ tài nguyên.");
                return false;
            }
        }

        // Trừ tài nguyên
        foreach (var req in nextLevelData.upgradeRequirements)
        {
            ResourceManager.Instance.ConsumeResource(req.resourceType, req.resourceId, req.amount);
        }

        // Cập nhật cấp độ và HP của hero
        hero.level = currentLevel + 1;
        hero.hp = nextLevelData.hp;

        // Cập nhật lại hero trong danh sách
        heroDatas[index] = hero;

        // Lưu lại dữ liệu sau khi nâng cấp
        SaveHeroesData();

        // In ra kết quả sau khi nâng cấp
        Debug.Log($"Đã nâng cấp Hero {heroId} lên cấp {currentLevel + 1}");
        return true;
    }

    public void OnUpgradeHeroButtonClicked(int heroId)
    {
        bool success = HeroManager.instance.TryUpgradeHero(heroId);

        if (success)
        {
            // Cập nhật UI hoặc thông báo nâng cấp thành công
            Debug.Log("Nâng cấp hero thành công!");
        }
        else
        {
            // Thông báo lỗi nếu không đủ tài nguyên
            Debug.Log("Nâng cấp thất bại! Không đủ tài nguyên.");
        }
    }
}
