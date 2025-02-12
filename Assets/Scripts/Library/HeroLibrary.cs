using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroLibrary : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private Image heroImage;
    [SerializeField] private TextMeshProUGUI txtID;
    [SerializeField] private TextMeshProUGUI txtID2;
    [SerializeField] private HeroView heroView;
    [SerializeField] private GameObject heroParent;

    private void Start()
    {
        DataHero hero = HeroManager.instance.GetHero(this.id);
        if (hero.isUnlock)
        {
            txtID.gameObject.SetActive(false);
            heroImage.gameObject.SetActive(true);
        }
        else
        {
            txtID.gameObject.SetActive(true);
            heroImage.gameObject.SetActive(false);
        }
        GetComponent<Button>().onClick.AddListener(() =>  SetHeroView());

    }
    public void SetHeroView()
    {
        DataHero hero = HeroManager.instance.GetHero(this.id);

        heroView.SetHero(id, hero.heroImage, hero.name);
        this.heroView.gameObject.SetActive(true);
        heroParent.gameObject.SetActive(false);
    }
    public void SetHero(int id,Sprite heroSprite, HeroView heroView,GameObject heroParent)
    {
        this.id = id;
        this.heroImage.sprite = heroSprite;
        this.txtID.text = id.ToString();
        this.txtID2.text = id.ToString();
        this.heroView = heroView;
        this.heroParent = heroParent;
    }
    private void OnEnable()
    {
        DataHero hero = HeroManager.instance.GetHero(this.id);
        if (hero.isUnlock)
        {
            txtID.gameObject.SetActive(false);
            heroImage.gameObject.SetActive(true);
        }
        else
        {
            txtID.gameObject.SetActive(true);
            heroImage.gameObject.SetActive(false);
        }
        

        
    }
}
