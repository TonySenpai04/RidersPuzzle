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

    public int Id { get => id; set => id = value; }

    private void Start()
    {
        var hero = HeroManager.instance.GetHero(this.Id);
        if (hero != null && hero.Value.hp>0)
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
  
    public void SetHero(int id,Sprite heroSprite)
    {
        this.Id = id;
        this.heroImage.sprite = heroSprite;
        this.txtID.text = id.ToString();
        this.txtID2.text = id.ToString();
    }
    private void OnEnable()
    {
        var hero = HeroManager.instance.GetHero(this.Id);
        if (hero!=null ) 
        {
            txtID.gameObject.SetActive(false);
            heroImage.gameObject.SetActive(true);
            heroImage.sprite = hero.Value.heroImage;
        }
        else
        {
            txtID.gameObject.SetActive(true);
            heroImage.gameObject.SetActive(false);
        }

    }
}
