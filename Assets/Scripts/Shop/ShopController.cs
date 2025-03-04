using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldTxt;
    [SerializeField] private Button heroBtn;
    [SerializeField] private GameObject heroZone;
    [SerializeField] private Button dailyBtn;
    [SerializeField] private GameObject dailyZone;
    [SerializeField]private Sprite selectedBtn;
    [SerializeField] private Sprite unSelectedBtn;
    [SerializeField] private Sprite selectedZone;

    private void Start()
    {
        goldTxt.text=GoldManager.instance.GetGold().ToString();
        heroBtn.onClick.AddListener(() => SetSelectedButton(heroBtn));
        dailyBtn.onClick.AddListener(() => SetSelectedButton(dailyBtn));


       // ToDailyShop();
    }
    private void FixedUpdate()
    {
        goldTxt.text = GoldManager.instance.GetGold().ToString();
    }
    private void OnEnable()
    {
        
    }
    public void ToHeroShop()
    {
        SetSelectedButton(heroBtn);
 
    }
    public void ToDailyShop()
    {
        SetSelectedButton(dailyBtn);
        
    }
    public void SetSelectedButton(Button clickedBtn)
    {
        if (clickedBtn == heroBtn)
        {
            heroBtn.image.sprite = selectedBtn;
            heroBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            dailyBtn.GetComponentInChildren<TextMeshProUGUI>().color = ColorUtility.TryParseHtmlString("#25AAFE", out Color enablePartyColor) ? enablePartyColor : Color.white; 
            dailyBtn.image.sprite = unSelectedBtn;
            heroZone.SetActive(true);
            dailyZone.SetActive(false);
        }
        else if (clickedBtn == dailyBtn)
        {
            dailyBtn.image.sprite = selectedBtn;
            dailyBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            heroBtn.GetComponentInChildren<TextMeshProUGUI>().color = ColorUtility.TryParseHtmlString("#25AAFE", out Color enablePartyColor) ? enablePartyColor : Color.white;
            heroBtn.image.sprite = unSelectedBtn;
            heroZone.SetActive(false);
            dailyZone.SetActive(true);
        }
    }
}
