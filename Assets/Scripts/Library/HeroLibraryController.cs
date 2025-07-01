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
        Init();
    }
    public void Init()
    {
        int count = HeroManager.instance.heroDatas.Count;
        for (int i = 0; i < count; i++)
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
            hero.SetHero(1000 + i + count, null);
            HeroLibraries.Add(hero);

        }
        heroOwnerTxt.text = HeroManager.instance.HeroOwnedQuantity() + "/" + heroLibraries.Count;
    }
    private void OnEnable()
    {
        heroOwnerTxt.text =HeroManager.instance.HeroOwnedQuantity() + "/" + heroLibraries.Count;
        foreach (var hero in HeroLibraries)
        {
            var heroData = HeroManager.instance.GetHero(hero.Id);
            if (heroData.HasValue && heroData.Value.isUnlock)
            {
                HeroView.SetHero(heroData.Value.id);
           
            }
            
        }
    }
    public void SetHeroView(int id)
    {

        HeroView.SetHero(id);
        if (NewBoughtHeroManager.instance.IsNewHero(id))
        {
            NewBoughtHeroManager.instance.RemoveHero(id);
            //redNotiDot.gameObject.SetActive(false);

            if (NewBoughtHeroManager.instance.AllSeen())
            {
                NotiManager.instance.ClearMultipleNotiRedDots(new List<string> { "riderlib"});
            }
        }
        this.HeroView.gameObject.SetActive(true);
        heroParent.gameObject.SetActive(false);
        BackgroundManager.instance.SetHeroBg();
       
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
