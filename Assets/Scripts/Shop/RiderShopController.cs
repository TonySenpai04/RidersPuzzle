using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RiderShopController : MonoBehaviour
{
    [SerializeField] private HeroShopItem heroShopItemPrefabs;
    [SerializeField] private Transform content;
    [SerializeField] private List<HeroShopItem> heroShops;
    void Start()
    {
        Init();
    }

    public void Init()
    {
        int count = HeroManager.instance.heroDatas.Count;
        for (int i = 0; i < count; i++)
        {
            HeroShopItem hero = Instantiate(heroShopItemPrefabs, content.transform);
            hero.SetDataHero(HeroManager.instance.heroDatas[i].id);
            hero.OnHeroSelected += HandleHeroSelected;
            heroShops.Add(hero);


        }
    }
    void HandleHeroSelected(int selectedHeroId)
    {
        SoundManager.instance
             .PlayHeroSFX(selectedHeroId);
    }

}
