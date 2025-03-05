using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReceiveGold : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameGiftTxt;
    [SerializeField] private TextMeshProUGUI goldAmountTxt;
    [SerializeField] public Button exchangeBtn;
    [SerializeField] private int goldAmount;
    [SerializeField] private TextMeshProUGUI allGoldTxt;
    private void Start()
    {
        exchangeBtn.onClick.AddListener(() => Exchange());
    }
    public void SetGold(int goldAmount)
    {
        this.goldAmount = goldAmount;
        goldAmountTxt.text="x"+goldAmount.ToString();
    }
    public void Exchange()
    {
        nameGiftTxt.text = "You received";
        GoldManager.instance.AddGold(goldAmount);
        exchangeBtn.gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        allGoldTxt.text = GoldManager.instance.GetGold().ToString();
    }
}
