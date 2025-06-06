using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//[Serializable]
//public class HeroUpgradeData
//{
//    public int level;
//    public int hp;
//    public int masteryPoint;

//    public int upgradeResourceType1;
//    public int upgradeResourceId1;
//    public int amount1;

//    public int upgradeResourceType2;
//    public int upgradeResourceId2;
//    public int amount2;

//    public int upgradeResourceType3;
//    public int upgradeResourceId3;
//    public int amount3;
//}

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        HeroManager.instance.LoadHeroesData();
    }
    public bool TryUpgradeHero(int heroId)
    {
        var heroData = HeroManager.instance.heroDatas;
        int index = heroData.FindIndex(h => h.id == heroId);
        if (index == -1)
        {
            Debug.Log("Hero không tồn tại.");
            return false;
        }

        DataHero hero = heroData[index];
        int currentLevel = hero.level;

        var nextLevelData = ReadCSVDataHeroStat.instance.GetHeroLevelData(heroId, currentLevel + 1);
        if (nextLevelData == null)
        {
            Debug.Log("Hero đã đạt cấp tối đa.");
            return false;
        }

        foreach (var req in nextLevelData.upgradeRequirements)
        {
            if (!ResourceManager.Instance.HasEnough(req.resourceType, req.resourceId, req.amount))
            {
                Debug.Log("Không đủ tài nguyên.");
                return false;
            }
        }

        foreach (var req in nextLevelData.upgradeRequirements)
        {
            ResourceManager.Instance.ConsumeResource(req.resourceType, req.resourceId, req.amount);
        }

        float rand = UnityEngine.Random.Range(0f, 1f);
        if (rand <= nextLevelData.upgradeRate)
        {
            hero.level = currentLevel + 1;
            hero.hp = nextLevelData.hp;
            heroData[index] = hero;

            HeroManager.instance.SaveHeroesData();

            Debug.Log($"✅ Thành công! Hero {heroId} đã lên cấp {hero.level}");
            return true;
        }
        else
        {
            Debug.Log($"❌ Thất bại! Hero {heroId} không lên cấp.");
            return false;
        }
    }
    
}

