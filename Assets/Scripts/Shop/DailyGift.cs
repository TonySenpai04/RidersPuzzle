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
            giftButton.GetComponentInChildren<TextMeshProUGUI>().text = "Claimed";
        }
        else
        {
            giftButton.GetComponentInChildren<TextMeshProUGUI>().text = "FREE";
            giftButton.onClick.AddListener(() => SoundManager.instance.PlaySFX("Click Sound"));
            giftButton.onClick.AddListener(ClaimGift);
        }
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
       
    }
    public void ReceiveGift()
    {
        giftButton.GetComponentInChildren<TextMeshProUGUI>().text = "Claimed";
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

    [System.Serializable]
    private class DailyGiftData
    {
        public string lastClaimDate = "";
    }
}
