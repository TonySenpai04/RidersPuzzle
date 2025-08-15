using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Collections;
using System;
using Firebase.Database;
using System.Threading.Tasks;
using Firebase.Extensions;
using System.Collections.Generic;

public class DailyGift : MonoBehaviour
{
    [SerializeField] private string saveFilePath;
    [SerializeField] private int goldAmount = 5;
    [SerializeField] private Button giftButton;
    [SerializeField] private Button exChangeBtn;
    [SerializeField] private ReceiveGold receiveGold;

    private async void Start()
    {
        await Task.Delay(2500);
        receiveGold.exchangeBtn.onClick.AddListener(()=> ReceiveGift());
        saveFilePath = Application.persistentDataPath + "/dailyGift.json";
        StartCoroutine(WaitForServerTime());
    }

    private IEnumerator WaitForServerTime()
    {
        while (!TimeManager.Instance.IsTimeFetched)
        {
            yield return null;
        }
        
        DateTime nowTime = TimeManager.Instance.ServerDateTime;
        DailyGiftData giftData;

        if (FirebaseDataManager.Instance.GetCurrentUser() != null)
        {
            // Load từ Firebase
            var loadTask = LoadDailyGiftDataFromFirebaseAsync();
            yield return new WaitUntil(() => loadTask.IsCompleted);
            giftData = loadTask.Result ?? new DailyGiftData();
        }
        else
        {
            // Guest → load local
            giftData = LoadGiftData();
        }

        if (!string.IsNullOrEmpty(giftData.lastClaimDate))
        {
            DateTime lastClaim = DateTime.Parse(giftData.lastClaimDate);
            double hoursPassed = (nowTime - lastClaim).TotalHours;

            if (hoursPassed < 1)
            {
                ApplyTextManager.instance.textLocalizer.SetLocalizedText("shop_claimed",
                    giftButton.GetComponentInChildren<TextMeshProUGUI>());
                NotiManager.instance.ClearMultipleNotiRedDots(new List<string> { "shop", "dailygift" });
                yield break;
            }
        }


        // Nếu đủ thời gian hoặc chưa nhận bao giờ → cho nhận
        ApplyTextManager.instance.textLocalizer.SetLocalizedText("shop_daily_pack_tag_free",
           giftButton.GetComponentInChildren<TextMeshProUGUI>());
        giftButton.onClick.AddListener(() => ShowExchangeBtn());
        giftButton.onClick.AddListener(() => SoundManager.instance.PlaySFX("Click Sound"));
        giftButton.onClick.AddListener(ClaimGift);
        NotiManager.instance.CheckDailyLogin();
    }
    public void ShowExchangeBtn()
    {

        //exChangeBtn.gameObject.SetActive(!exChangeBtn.gameObject.activeSelf);
    }
   

    public void ClaimGift()
    {
        DateTime nowTime = TimeManager.Instance.ServerDateTime;
        DailyGiftData giftData = new DailyGiftData
        {
            lastClaimDate = nowTime.ToString("yyyy-MM-dd HH:mm:ss") // đổi sang lastClaimDateTime
        };

        if (FirebaseDataManager.Instance.GetCurrentUser() != null)
        {
            SaveDailyGiftDataToFirebase(giftData);
        }
        
        SaveGiftData(giftData);
        
        receiveGold.gameObject.SetActive(true);
        receiveGold.SetGold(this.goldAmount);
        exChangeBtn.gameObject.SetActive(false);

    }
    public void ReceiveGift()
    {
        ApplyTextManager.instance.textLocalizer.SetLocalizedText("shop_claimed",
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
    public void SaveDailyGiftDataToFirebase(DailyGiftData data)
    {
        var user = FirebaseDataManager.Instance.GetCurrentUser();
        if (user == null)
        {
            Debug.LogWarning("❌ Chưa đăng nhập - Lưu local thay vì Firebase.");
            SaveGiftData(data);
            return;
        }

        string json = JsonUtility.ToJson(data);
        FirebaseDatabase.DefaultInstance.RootReference
            .Child("users")
            .Child(user.UserId)
            .Child("dailyGift")
            .SetRawJsonValueAsync(json)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                    Debug.Log("✅ Đã lưu DailyGiftData lên Firebase.");
                else
                    Debug.LogError("❌ Lỗi khi lưu DailyGiftData: " + task.Exception);
            });
    }

    public async Task<DailyGiftData> LoadDailyGiftDataFromFirebaseAsync()
    {
        var user = FirebaseDataManager.Instance.GetCurrentUser();
        if (user == null)
        {
            Debug.LogWarning("❌ Chưa đăng nhập - Load local thay vì Firebase.");
            return LoadGiftData();
        }

        string userId = user.UserId;

        DataSnapshot snapshot = await FirebaseDatabase.DefaultInstance.RootReference
            .Child("users")
            .Child(userId)
            .Child("dailyGift")
            .GetValueAsync();

        if (snapshot.Exists)
        {
            string json = snapshot.GetRawJsonValue();
            Debug.Log("✅ Load DailyGiftData từ Firebase:\n" + json);
            return JsonUtility.FromJson<DailyGiftData>(json);
        }
        else
        {
            Debug.Log("☁️ Firebase chưa có -> Load local & đồng bộ lên Firebase.");
            DailyGiftData localData = LoadGiftData();
            SaveDailyGiftDataToFirebase(localData);
            return localData;
        }
    }


}
