using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroShopItem : MonoBehaviour
{
    [SerializeField] private int heroId;
    [SerializeField] private Image heroImage;
    [SerializeField] private TextMeshProUGUI heroName;
    [SerializeField] private TextMeshProUGUI priceTxt;
    [SerializeField] private TextMeshProUGUI onwed;
    public Action<int> OnHeroSelected;
    [SerializeField] private Button exchangeBtn;
    [SerializeField] private TextMeshProUGUI exchangeTxt;
    [SerializeField] private Button buyBtn;
    public int HeroId { get => heroId; set => heroId = value; }
    public Button BuyBtn { get => buyBtn; set => buyBtn = value; }
    public Button ExchangeBtn { get => exchangeBtn; set => exchangeBtn = value; }
    private void Start()
    {
        exchangeBtn.gameObject.SetActive(false); 

       
    }
    private void OnEnable()
    {
        UpdateHero();
    }

    public void SetDataHero(int id)
    {
        this.HeroId = id;
        var hero = HeroManager.instance.GetHero(id);
        if(hero != null)
        {
            heroName.text = hero.Value.name;
            if (hero.Value.isUnlock)
            {
                onwed.gameObject.SetActive(true);
                ApplyTextManager.instance.textLocalizer.SetLocalizedText("shop_owned_rider", onwed);
                priceTxt.gameObject.SetActive(false);

            }
            else
            {
                onwed.gameObject.SetActive(false);
                priceTxt.gameObject.SetActive(true);
                priceTxt.text = hero.Value.price.ToString();

            }
            heroImage.sprite = hero.Value.heroImage;
        }

    }
    public void UpdateHero()
    {

        ApplyTextManager.instance.textLocalizer.SetLocalizedText("button_exchange", exchangeTxt);
        var hero = HeroManager.instance.GetHero(HeroId);
        if (hero != null)
        {
            heroName.text = hero.Value.name;
            if (hero.Value.isUnlock)
            {
                onwed.gameObject.SetActive(true);
                ApplyTextManager.instance.textLocalizer.SetLocalizedText("shop_owned_rider", onwed);
                priceTxt.gameObject.SetActive(false);

            }
            else
            {
                onwed.gameObject.SetActive(false);
                priceTxt.gameObject.SetActive(true);
                priceTxt.text = hero.Value.price.ToString();
            }
            heroImage.sprite = hero.Value.heroImage;
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnHeroSelected?.Invoke(HeroId);
    }
}
