using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    void Start()
    {
        
    }
    public void SetHero(int id, Sprite heroSprite, string name)
    {
        this.id = id;
        var hero = HeroManager.instance.GetHero(this.id);
        if (hero == null)
            return;
        this.heroImage.sprite = heroSprite;
        this.heroTxt.text = $"{id} {name}";
        
        this.hpTxt.text = "HP:" + hero.Value.hp;
        this.skillTxt.text = "Skill:" + hero.Value.skillDescription;

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
