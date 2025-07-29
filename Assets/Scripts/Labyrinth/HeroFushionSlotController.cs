using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroFushionSlotController : MonoBehaviour
{
    [SerializeField] private List<HeroFusionSlot> heroFusionSlots;
    [SerializeField] private GattaiFusionUI gattaiFusion;
    [SerializeField] private GameObject selectHeroView;
    [SerializeField] private GameObject slotHeroView;
    [SerializeField] private GameObject banner;
    [SerializeField] private GameObject fuhsionBtn;
    private void Start()
    {
        foreach(var hero in heroFusionSlots)
        {
            hero.SetClickAction(OnclickHeroSlot);
        }
    }
    private void OnEnable()
    {
        if (gattaiFusion.selectedHeroes.Count <= 0)
        {
            banner.SetActive(true);
            fuhsionBtn.SetActive(false);
        }
        else
        {
            for (int i = 0; i < heroFusionSlots.Count; i++)
            {
                int count = gattaiFusion.selectedHeroes.Count;
                if (i <= count - 1)
                {
                    var Hero = HeroManager.instance.GetHero(gattaiFusion.selectedHeroes[i]);
                    heroFusionSlots[i].SetData(gattaiFusion.selectedHeroes[i],
                        Hero.Value.heroCardImage, Hero.Value.level, Hero.Value.hp, Hero.Value.mp);
                }
                else
                {
                    heroFusionSlots[i].ResetData();
                }
            }
            banner.SetActive(false);
            fuhsionBtn.SetActive(true);
        }
    }
    public void OnclickHeroSlot()
    {
        var unlockHeros = HeroManager.instance.GetUnlockHero();
        int heroMpCount = 0;
        foreach (var heroData in unlockHeros)
        {
            if (heroData.currentMP > 0)
                heroMpCount++;

        }
        if (heroMpCount > 3)
        {
            selectHeroView.SetActive(true);
            slotHeroView.SetActive(false);
        }
        else
        {
            Debug.Log("not enough hero");
        }
    }
}
