using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroCard : MonoBehaviour
{
    [SerializeField] public int heroID;
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private TextMeshProUGUI healthTxt;
    [SerializeField] private TextMeshProUGUI mpTxt;
    [SerializeField] private Image heroImg;
    [SerializeField] public Image redNotiDot;
    public UpgradeView upgradeView; 
    public GameObject heroCardview;
    public void SetData(int heroID,UpgradeView upgradeView,GameObject heroCardView,int level,int health,
        int mp,Sprite heroSprite)
    {
        this.heroID = heroID;
        this.upgradeView = upgradeView;
        this.heroCardview = heroCardView;
        levelTxt.text = LocalizationManager.instance.GetLocalizedText("level_title") + " " +level.ToString();
        healthTxt.text = health.ToString();
        mpTxt.text = mp.ToString();
        heroImg.sprite = heroSprite;
      //  redNotiDot.gameObject.SetActive(NewBoughtHeroManager.instance.IsNewHero(heroID));


    }
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
            
        }

        upgradeView.currentID = heroID;
        upgradeView.SetUpgradeView(currentData, nextData);
        heroCardview.SetActive(false);
        //if (NewBoughtHeroManager.instance.IsNewHero(heroID))
        //{
        //    NewBoughtHeroManager.instance.RemoveHero(heroID);
        //    redNotiDot.gameObject.SetActive(false);

        //    if (NewBoughtHeroManager.instance.AllSeen())
        //    {
        //        NotiManager.instance.ClearMultipleNotiRedDots(new List<string> { "upgrade", "enhance" });
        //    }
        //}


    }

    public void SetHeroInfo(DataHero hero)
    {
        heroID = hero.id;
        levelTxt.text = $"Lv. {hero.level}";
        healthTxt.text = $"HP: {hero.hp}";
       
    }
}
