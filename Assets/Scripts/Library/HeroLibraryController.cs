using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroLibraryController : MonoBehaviour
{
    [SerializeField] private HeroLibrary HeroLibraryPrefabs;
    [SerializeField] private Transform content;
    [SerializeField] private HeroView HeroView;
    [SerializeField] private GameObject heroParent;
    [SerializeField] private List<HeroLibrary> heroLibraries;
    [SerializeField] private TextMeshProUGUI heroOwnerTxt;

    public List<HeroLibrary> HeroLibraries { get => heroLibraries; set => heroLibraries = value; }

    void Start()
    {
        int count=HeroManager.instance.heroDatas.Count;
        for(int i=0;i<count; i++)
        {
            HeroLibrary hero = Instantiate(HeroLibraryPrefabs, content.transform);
            hero.SetHero(HeroManager.instance.heroDatas[i].id, HeroManager.instance.heroDatas[i].heroImage);
            var heroData = HeroManager.instance.GetHero(hero.Id);

            hero.GetComponent<Button>().onClick.AddListener(() => SetHeroView(hero.Id));
            hero.GetComponent<Button>().onClick.AddListener(() => SoundManager.instance
            .PlayHeroSFX(hero.Id));
            HeroLibraries.Add(hero);


        }
        for (int i = 1; i < 10; i++)
        {
            HeroLibrary hero = Instantiate(HeroLibraryPrefabs, content.transform);
            hero.SetHero(1002+i,null);
            HeroLibraries.Add(hero);

        }
        heroOwnerTxt.text = "Owned:" + HeroManager.instance.HeroOwnedQuantity() + "/" + heroLibraries.Count;
    }
    private void OnEnable()
    {
        heroOwnerTxt.text ="Owned:"+HeroManager.instance.HeroOwnedQuantity() + "/" + heroLibraries.Count;
        foreach (var hero in HeroLibraries)
        {
            var heroData = HeroManager.instance.GetHero(hero.Id);
            if (heroData.HasValue && heroData.Value.isUnlock)
            {
                HeroView.SetHero(heroData.Value.id, heroData.Value.heroImage,
                    heroData.Value.name,heroData.Value.isUnlock);
           
            }
            
        }
    }
    public void SetHeroView(int id)
    {
        var hero = HeroManager.instance.GetHero(id);
        if (hero == null)
            return;
        HeroView.SetHero(id, hero.Value.heroImage, hero.Value.name,hero.Value.isUnlock);
        this.HeroView.gameObject.SetActive(true);
        heroParent.gameObject.SetActive(false);
        BackgroundManager.instance.SetHeroBg();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
