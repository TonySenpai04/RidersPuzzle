using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Collections;

public class DailyGift : MonoBehaviour
{
    [SerializeField] private string saveFilePath;
    [SerializeField] private int goldAmount = 5;
    [SerializeField] private Button giftButton;
    [SerializeField] private Button exChangeBtn;
    [SerializeField] private ReceiveGold receiveGold;

    private void Start()
    {
        receiveGold.exchangeBtn.onClick.AddListener(()=> ReceiveGift());
        saveFilePath = Application.persistentDataPath + "/dailyGift.json";
        StartCoroutine(WaitForServerTime());
    }

    private IEnumerator WaitForServerTime()
    {
        while (!TimeManager.Instance.IsTimeFetched) // Chờ TimeManager lấy xong thời gian
        {
            yield return null;
        }

        string serverDate = TimeManager.Instance.ServerDate;
        DailyGiftData giftData = LoadGiftData();

        if (giftData.lastClaimDate == serverDate)
        {
            ApplyText.instance.textLocalizer.SetLocalizedText("shop_claimed",
                giftButton.GetComponentInChildren<TextMeshProUGUI>());
           // giftButton.GetComponentInChildren<TextMeshProUGUI>().text = "Claimed";
        }
        else
        {
            ApplyText.instance.textLocalizer.SetLocalizedText("shop_daily_pack_tag_free",
               giftButton.GetComponentInChildren<TextMeshProUGUI>());
            //giftButton.GetComponentInChildren<TextMeshProUGUI>().text = "FREE";
            giftButton.onClick.AddListener(() => ShowExchangeBtn());
            giftButton.onClick.AddListener(() => SoundManager.instance.PlaySFX("Click Sound"));
            exChangeBtn.onClick.AddListener(ClaimGift);
        }
    }
    public void ShowExchangeBtn()
    {

        exChangeBtn.gameObject.SetActive(!exChangeBtn.gameObject.activeSelf);
    }
   

    public void ClaimGift()
    {
        string serverDate = TimeManager.Instance.ServerDate;

        if (serverDate == null)
        {
            Debug.LogError("Không thể nhận quà vì chưa lấy được thời gian!");
            return;
        }

        DailyGiftData giftData = LoadGiftData();
        giftData.lastClaimDate = serverDate;
        SaveGiftData(giftData);

        receiveGold.gameObject.SetActive(true);
        receiveGold.SetGold(this.goldAmount);
        exChangeBtn.gameObject.SetActive(false);

    }
    public void ReceiveGift()
    {
        ApplyText.instance.textLocalizer.SetLocalizedText("shop_claimed",
              giftButton.GetComponentInChildren<TextMeshProUGUI>());
      //  giftButton.GetComponentInChildren<TextMeshProUGUI>().text = "Claimed";
        giftButton.onClick.RemoveAllListeners();
        giftButton.onClick.AddListener(() => SoundManager.instance.PlaySFX("Click Sound"));

    }
    private DailyGiftData LoadGiftData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            return JsonUtility.FromJson<DailyGiftData>(json);
        }
        return new DailyGiftData();
    }

    private void SaveGiftData(DailyGiftData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
    }

   
}
