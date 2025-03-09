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
    [SerializeField] private GameObject Tutorial;
    [SerializeField] private GameObject receiveHero;
    [SerializeField] private GameObject heroView;
    [SerializeField] private bool isHeroShop; 
    [SerializeField] private GameObject receiveGift;
    private void Start()
    {
        goldTxt.text=GoldManager.instance.GetGold().ToString();
        heroBtn.onClick.AddListener(() => ToHeroShop());
        dailyBtn.onClick.AddListener(() => ToDailyShop());


       // ToDailyShop();
    }
    private void FixedUpdate()
    {
        goldTxt.text = GoldManager.instance.GetGold().ToString();
    }
    private void OnEnable()
    {
        Tutorial.SetActive(false);
        receiveHero.SetActive(false);
        receiveGift.SetActive(false);
        heroView.SetActive(false);
        heroBtn.gameObject.SetActive(true);
        dailyBtn.gameObject.SetActive(true);
        if (!isHeroShop)
        {
            ToDailyShop();
        }
    }
    public void ToHeroShop()
    {
        isHeroShop=true;
        NotiManager.instance.ShowNotification("Cooming soon");
      //  SetSelectedButton(heroBtn);
        isHeroShop = false;

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
