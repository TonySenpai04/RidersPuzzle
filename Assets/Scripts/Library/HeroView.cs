using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroView : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private Image heroImage;
    [SerializeField] private TextMeshProUGUI heroTxt;
    [SerializeField] private TextMeshProUGUI hpTxt;
    [SerializeField] private TextMeshProUGUI skillTxt;
    [SerializeField] private GameObject library;
    [SerializeField] private GameObject map;
    [SerializeField] private StageHeroController stageHeroController;
    [SerializeField] private Image disableParty;
    [SerializeField] private Image enableParty;
    [SerializeField] private Image disableShop;
    [SerializeField] private Image enableShop;
    [SerializeField] private bool isUnlock;
    [SerializeField] private TextMeshProUGUI shopTxt;
    [SerializeField] private TextMeshProUGUI partyTxt;
    [SerializeField] private Button toShop;
    [SerializeField] private Button toParty;
    [SerializeField] private TextMeshProUGUI storyTxt;
    [SerializeField] private List<DataHero> onwedHero;
    [SerializeField] private int currentIndex;
    private ISwipeDetector swipeDetector;
    void Start()
    {
        PlaySoundHero();
        onwedHero = HeroManager.instance.GetUnlockHero();
        swipeDetector = new SwipeDetector(NextHero, PreviousHero);
    }
    private void Update()
    {
        swipeDetector.DetectSwipe();
    }
    public void PlaySoundHero()
    {
        EventTrigger trigger = heroImage.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => SoundManager.instance.PlayHeroSFX(id));

        trigger.triggers.Add(entry);
    }
    public void SetHero(int id)
    {
        this.id = id;
        var hero = HeroManager.instance.GetHero(this.id);
        if (hero == null)
            return;
        this.heroImage.sprite = hero.Value.heroImage;
        this.heroTxt.text = $"{id} {hero.Value.name}";
        
        this.hpTxt.text =  hero.Value.hp.ToString();
        // ApplyText.instance.UpdateSkillInfoLib(id);
        ApplyTextManager.instance.UpdateSkillInfoLib(id);
        this.isUnlock=hero.Value.isUnlock;
        currentIndex = onwedHero.FindIndex(o => o.id == id);
        if (this.isUnlock)
        {

            enableParty.gameObject.SetActive(true);
            disableParty.gameObject.SetActive(false);
            enableShop.gameObject.SetActive(false);
            disableShop.gameObject.SetActive(true);
            toParty.interactable = true;
            toShop.interactable = false;
            shopTxt.color = ColorUtility.TryParseHtmlString("#556169", out Color disableShopColor) ? disableShopColor : Color.white;
            partyTxt.color = ColorUtility.TryParseHtmlString("#25AAFE", out Color enablePartyColor) ? enablePartyColor : Color.white; 
        }
        else
        {
            enableParty.gameObject.SetActive(false);
            disableParty.gameObject.SetActive(true);
            enableShop.gameObject.SetActive(true);
            disableShop.gameObject.SetActive(false);
            toParty.interactable = false;
            toShop.interactable = true;
            partyTxt.color = ColorUtility.TryParseHtmlString("#556169", out Color disablePartyColor) ? disablePartyColor : Color.white;
            shopTxt.color = ColorUtility.TryParseHtmlString("#25AAFE", out Color enableShopColor) ? enableShopColor : Color.white;


        }
    }
    private void OnEnable()
    {
        onwedHero.Clear();
        onwedHero = HeroManager.instance.GetUnlockHero();
    }
    public void ToParty()
    {
        this.gameObject.SetActive(false);
        library.SetActive(false);
        map.gameObject.SetActive(true);

        FooterController.instance.SelectButton("Map");
        stageHeroController.gameObject.SetActive(true);
        stageHeroController.SetHeroID(this.id);
    }
    private void LoadHero(int index)
    {
        if (index >= 0 && index < onwedHero.Count)
        {
            DataHero obj = onwedHero[index];
            SetHero(obj.id);


        }
    }
    public void NextHero()
    {
        if (currentIndex < onwedHero.Count - 1)
        {
            currentIndex++;
            LoadHero(currentIndex);
        }
    }
    public void PreviousHero()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            LoadHero(currentIndex);
        }
    }
}
