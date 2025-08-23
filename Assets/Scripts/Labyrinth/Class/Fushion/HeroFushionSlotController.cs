using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroFushionSlotController : MonoBehaviour
{
    [SerializeField] private List<HeroFusionSlot> heroFusionSlots;
    [SerializeField] private GattaiFusionManager gattaiFusion;
    [SerializeField] private GameObject selectHeroView;
    [SerializeField] private GameObject slotHeroView;
    [SerializeField] private GameObject banner;
    [SerializeField] private GameObject fuhsionBtn;
    [SerializeField] private GameObject warningPopup;
    private void Start()
    {
        foreach(var hero in heroFusionSlots)
        {
            hero.SetClickAction(OnclickHeroSlot);
        }
    }
    private void OnEnable()
    {
        UpdateSlotState();
    }
    public void UpdateSlotState()
    {
        warningPopup.SetActive(false);
        List<int> selectedHeroes = gattaiFusion.GetSelectedHeroes();
        if (selectedHeroes.Count <3)
        {

            banner.SetActive(true);
            fuhsionBtn.SetActive(false);
            foreach (var hero in heroFusionSlots)
            {
                hero.ResetData();
            }
        }
        else
        {

            for (int i = 0; i < heroFusionSlots.Count; i++)
            {
                int count = selectedHeroes.Count;
                if (i <= count - 1)
                {
                    var Hero = HeroManager.instance.GetHero(selectedHeroes[i]);
                    heroFusionSlots[i].SetData(selectedHeroes[i],
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
            warningPopup.SetActive(true);
        }
    }
}
