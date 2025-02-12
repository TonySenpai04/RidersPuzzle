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
        this.heroImage.sprite = heroSprite;
        this.heroTxt.text = $"{id} {name}";
        DataHero hero = HeroManager.instance.GetHero(this.id);
        this.hpTxt.text = "HP:" + hero.hp;
        this.skillTxt.text = "Skill:" + hero.skillDescription;

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
