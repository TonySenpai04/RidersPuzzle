using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class RiderData
{
    public int riderId;
    public string name;
    public List<RiderUpgradeLevel> levels = new List<RiderUpgradeLevel>();
}
[System.Serializable]
public class RiderUpgradeLevel
{
    public int level;
    public int hp;
    public int masteryPoint;
    public float upgradeRate;
    public List<UpgradeRequirement> upgradeRequirements = new List<UpgradeRequirement>();
}

[System.Serializable]
public class UpgradeRequirement
{
    public int resourceType;
    public int resourceId;
    public int amount;
}

public class ReadCSVDataHeroStat : MonoBehaviour
{
    public List<TextAsset> textAssets;
    public static ReadCSVDataHeroStat instance;
    public List<RiderData> riderDatas;
    public void Awake()
    {
        instance=this;
        riderDatas = new List<RiderData>();
        foreach (TextAsset textAsset in textAssets)
        {
            LoadRiderUpgradeData(textAsset);
        }
        foreach (RiderData rider in riderDatas)
        {
            // Debug Rider ID và Name trong một dòng
            string riderInfo = $"Rider ID: {rider.riderId}, Name: {rider.name}";

            foreach (RiderUpgradeLevel level in rider.levels)
            {
                // Debug Level, HP và Mastery trong một dòng
                string levelInfo = $"Level: {level.level}, HP: {level.hp}, Mastery: {level.masteryPoint},Upgrade Rate: {level.upgradeRate}";

                // Kiểm tra các yêu cầu nâng cấp (upgrade requirements)
                if (level.upgradeRequirements != null && level.upgradeRequirements.Any())
                {
                    foreach (var req in level.upgradeRequirements)
                    {
                        // Debug yêu cầu nâng cấp: Type, ID và Số lượng
                        string reqInfo = $"Req Type: {req.resourceType}, Req ID: {req.resourceId}, Amount: {req.amount}";

                        // Kết hợp tất cả thông tin thành một dòng duy nhất
                        Debug.Log($"{riderInfo} -> {levelInfo} -> {reqInfo}");
                    }
                }
                else
                {
                    // Nếu không có yêu cầu nâng cấp
                    Debug.Log($"{riderInfo} -> {levelInfo} -> No upgrade requirements.");
                }
            }
        }


    }
    public void LoadRiderUpgradeData(TextAsset csvFile)
    {
        var riders = new Dictionary<int, RiderData>();
        var lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++) // Bỏ dòng header
        {
            var line = lines[i].Trim();
            if (string.IsNullOrWhiteSpace(line)) continue;

            var parts = line.Split(',');

            if (parts.Length < 15 || string.IsNullOrWhiteSpace(parts[0])) continue;

            int idRider = int.Parse(parts[0]);
            string name = parts[1];
            int level = int.Parse(parts[2]);
            int hp = int.Parse(parts[3]);
            int mastery = int.Parse(parts[4]);
            float upgradeRate = float.Parse(parts[5]);

            var reqs = new List<UpgradeRequirement>();
            for (int j = 0; j < 3; j++)
            {
                int type = int.Parse(parts[6 + j * 3]);
                int id = int.Parse(parts[7 + j * 3]);
                int amount = int.Parse(parts[8 + j * 3]);

                //if (amount > 0)
                //{
                    reqs.Add(new UpgradeRequirement
                    {
                        resourceType = type,
                        resourceId = id,
                        amount = amount
                    });
               // }
            }

            if (!riders.ContainsKey(idRider))
            {
                riders[idRider] = new RiderData
                {
                    riderId = idRider,
                    name = name,
                    levels = new List<RiderUpgradeLevel>()
                };
            }

            riders[idRider].levels.Add(new RiderUpgradeLevel
            {
                level = level,
                hp = hp,
                masteryPoint = mastery,
                upgradeRate = upgradeRate,
                upgradeRequirements = reqs
            });
        }

        riderDatas.AddRange(riders.Values);
    }
    public RiderUpgradeLevel GetHeroLevelData(int heroId, int level)
    {
        var rider = riderDatas.FirstOrDefault(r => r.riderId == heroId);
        if (rider == null) return null;
        return rider.levels.FirstOrDefault(l => l.level == level);
    }

}
