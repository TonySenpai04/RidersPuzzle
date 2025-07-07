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
    [SerializeField] public Image notOwnedImg;
    [SerializeField] public TextMeshProUGUI notOwnedTxt;
    [SerializeField] public Image redNotiDot;
    public int Id { get => id; set => id = value; }

    private void Start()
    {
        var hero = HeroManager.instance.GetHero(this.Id);
        if (hero != null && hero.Value.heroImage!=null)
        {
            txtID.gameObject.SetActive(false);
            heroImage.gameObject.SetActive(true);
            notOwnedImg.gameObject.SetActive(!hero.Value.isUnlock);
            notOwnedTxt.text = LocalizationManager.instance.GetLocalizedText("tag_not_owned");
        }
        else
        {
            txtID.gameObject.SetActive(true);
            heroImage.gameObject.SetActive(false);
        }
   

    }
  
    public void SetHero(int id,Sprite heroSprite)
    {
        this.Id = id;
        this.heroImage.sprite = heroSprite;
        this.txtID.text = id.ToString();
        this.txtID2.text = id.ToString();
        redNotiDot.gameObject.SetActive(NewBoughtHeroManager.instance.IsNewHero(id));
    }
    private void OnEnable()
    {
        var hero = HeroManager.instance.GetHero(this.Id);
        if (hero!=null && hero.Value.heroImage != null) 
        {
            txtID.gameObject.SetActive(false);
            heroImage.gameObject.SetActive(true);
            heroImage.sprite = hero.Value.heroImage;
            redNotiDot.gameObject.SetActive(NewBoughtHeroManager.instance.IsNewHero(id));
            notOwnedImg.gameObject.SetActive(!hero.Value.isUnlock);
            notOwnedTxt.text = LocalizationManager.instance.GetLocalizedText("tag_not_owned");
        }
        else
        {
            txtID.gameObject.SetActive(true);
            heroImage.gameObject.SetActive(false);
           
        }
        
    }
}
