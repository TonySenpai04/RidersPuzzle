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
    private void OnEnable()
    {
        ApplyTextManager.instance.textLocalizer.SetLocalizedText("shop_daily_pack_title",
           nameGiftTxt);
    }
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
        ApplyTextManager.instance.textLocalizer.SetLocalizedText("shop_receive_package",
             nameGiftTxt);
        //nameGiftTxt.text = "You received";
        GoldManager.instance.AddGold(goldAmount);
        foreach (var quest in QuestManager.instance.GetQuestsByType<DailyGiftQuest>())
        {
            QuestManager.instance.UpdateQuest(quest.questId, 1, 0);
        }
        foreach (var quest in AchievementManager.instance.GetQuestsByType<DailyGiftQuest>())
        {
            AchievementManager.instance.UpdateQuest(quest.questId, 1, 0);
        }
        StartCoroutine(ShowRewardTemporarily());
        exchangeBtn.gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        allGoldTxt.text = GoldManager.instance.GetGold().ToString();
    }
    private IEnumerator ShowRewardTemporarily()
    {
        this.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);
    }
}
