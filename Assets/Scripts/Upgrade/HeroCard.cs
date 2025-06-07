using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeroCard : MonoBehaviour
{
    [SerializeField] private int heroID;
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private TextMeshProUGUI healthTxt;

    public UpgradeView upgradeView; // Gắn qua Inspector
    public GameObject heroCardview;

    public void OnClickHeroCard()
    {
        upgradeView.gameObject.SetActive(true);
        var heroDatas = HeroManager.instance.heroDatas;
        int index = heroDatas.FindIndex(h => h.id == heroID);
        if (index == -1)
        {
            Debug.LogWarning("❌ Không tìm thấy hero.");
            return;
        }

        DataHero heroData = heroDatas[index];
        int nextLevel = heroData.level + 1;
       

        var nextData = ReadCSVDataHeroStat.instance.GetHeroLevelData(heroID, nextLevel);
        var currentData = ReadCSVDataHeroStat.instance.GetHeroLevelData(heroID, heroData.level);
        if (nextData == null)
        {
            Debug.Log("✅ Hero đã đạt cấp tối đa.");
            return;
        }

        upgradeView.currentID = heroID;
        upgradeView.SetUpgradeView(currentData, nextData);
        heroCardview.SetActive(false);
    }

    public void SetHeroInfo(DataHero hero)
    {
        heroID = hero.id;
        levelTxt.text = $"Lv. {hero.level}";
        healthTxt.text = $"HP: {hero.hp}";
    }
}
