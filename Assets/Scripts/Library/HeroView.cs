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

    void Start()
    {
        PlaySoundHero();
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
        ApplyText.instance.UpdateSkillInfoLib(id);
        this.isUnlock=hero.Value.isUnlock;
        storyTxt.text = hero.Value.story;
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
    public void ToParty()
    {
        this.gameObject.SetActive(false);
        library.SetActive(false);
        map.gameObject.SetActive(true);

        FooterController.instance.SelectButton("map");
        stageHeroController.gameObject.SetActive(true);
        stageHeroController.SetHeroID(this.id);
    }
}
