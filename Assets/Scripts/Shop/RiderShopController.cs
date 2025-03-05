using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RiderShopController : MonoBehaviour
{
    [SerializeField] private HeroShopItem heroShopItemPrefabs;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject heroZone;
    [SerializeField] private List<HeroShopItem> heroShops;
    [SerializeField] private HeroView heroView;
    [SerializeField] private Button heroBtn;
    [SerializeField] private Button hotDealBtn;
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject gold;
    [SerializeField] private GameObject buyPopup;
    [SerializeField] private Button buyBtn;
    [SerializeField] private ReceiveHero receivedHero;
    void Start()
    {
        Init();
    }

    public void Init()
    {
        List<DataHero> allHeroes = HeroManager.instance.heroDatas;
        List<DataHero> unlockedHeroes = HeroManager.instance.GetUnlockHero();

        List<DataHero> lockedHeroes = allHeroes.Where(h => !unlockedHeroes.Any(u => u.id == h.id)).ToList();

        List<DataHero> sortedHeroes = lockedHeroes.Concat(unlockedHeroes).ToList();
        foreach (var heroData in sortedHeroes)
        {
            HeroShopItem hero = Instantiate(heroShopItemPrefabs, content.transform);
            hero.SetDataHero(heroData.id);
            if (!heroData.isUnlock)
            {
                ShowExchangeBtn(hero);
                hero.ExchangeBtn.onClick.AddListener(()=> ShowBuyPopup(hero));
                hero.ExchangeBtn.onClick.AddListener(() =>  SoundManager.instance.PlaySFX("Click Sound"));
            }
            hero.GetComponent<Button>().onClick.AddListener(()=>SoundManager.instance
             .PlaySFX("Click Sound"));
            hero.GetComponent<Button>().onClick.AddListener(()=>SetHeroView(hero.HeroId));
            heroShops.Add(hero);
        }
       
    }
    public void ShowBuyPopup(HeroShopItem hero)
    {

        buyPopup.gameObject.SetActive(true);
        buyBtn.onClick.AddListener(() => BuyHero(hero.HeroId, hero));
    }
    public void ShowExchangeBtn(HeroShopItem hero)
    {
        GridLayoutGroup grid = content.GetComponent<GridLayoutGroup>();

        hero.BuyBtn.onClick.AddListener(() =>
        {
            SoundManager.instance.PlaySFX("Click Sound");
            hero.ExchangeBtn.gameObject.SetActive(!hero.ExchangeBtn.gameObject.activeSelf);
            foreach(var heroItem in heroShops)
            {
                if (heroItem != hero)
                {
                    heroItem.ExchangeBtn.gameObject.SetActive(false);
                }
            }
            if (hero.ExchangeBtn.gameObject.activeSelf)

                grid.spacing = new Vector2(grid.spacing.x, 140);

            else
                grid.spacing = new Vector2(grid.spacing.x, 50);
        });
    
    }
    public void BuyHero(int id,HeroShopItem heroShopitem)
    {
        var hero = HeroManager.instance.GetHero(id);

        if (hero == null)
        {
            Debug.LogError("Hero không tồn tại!");
            return;
        }

        int heroPrice = hero.Value.price;
        int currentGold = GoldManager.instance.GetGold();

        if (currentGold >= heroPrice )
        {
            receivedHero.gameObject.SetActive(true);
            receivedHero.SetData(id);
            heroShopitem.ExchangeBtn.gameObject.SetActive(false);
            heroShopitem.ExchangeBtn.onClick.RemoveAllListeners();
            heroShopitem.BuyBtn.onClick.RemoveAllListeners();
            GoldManager.instance.SpendGold( heroPrice);

            HeroManager.instance.UnlockHero(id);
            heroShopitem.UpdateHero();
            Debug.Log($"Mua thành công hero {id}");
        }
        else
        {
            NotiManager.instance.ShowNotification("not enough gold ");
        }
    }
    public void SetHeroView(int id)
    {

        heroView.SetHero(id);
        this.heroView.gameObject.SetActive(true);
        heroZone.gameObject.SetActive(false);
        BackgroundManager.instance.SetHeroBg();
        heroBtn.gameObject.SetActive(false);
        hotDealBtn.gameObject.SetActive(false);
        title.gameObject.SetActive(false);
        gold.gameObject.SetActive(false);
    }

}
