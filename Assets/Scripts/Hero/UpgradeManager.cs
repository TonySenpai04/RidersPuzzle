using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[Serializable]
public class HeroUpgradeData
{
    public int level;
    public int hp;
    public int masteryPoint;

    public int upgradeResourceType1;
    public int upgradeResourceId1;
    public int amount1;

    public int upgradeResourceType2;
    public int upgradeResourceId2;
    public int amount2;

    public int upgradeResourceType3;
    public int upgradeResourceId3;
    public int amount3;
}

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    // Dữ liệu nâng cấp: key là heroId
    public Dictionary<int, List<HeroUpgradeData>> upgradeDataDict = new();

    void Awake()
    {
        Instance = this;
      //  LoadUpgradeDataFromJson(); // Hoặc từ sheet Excel convert ra
    }

    public HeroUpgradeData GetUpgradeData(int heroId, int level)
    {
        if (upgradeDataDict.TryGetValue(heroId, out var upgrades))
        {
            return upgrades.FirstOrDefault(u => u.level == level);
        }
        return null;
    }
}

