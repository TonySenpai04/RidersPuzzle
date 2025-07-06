using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
[System.Serializable]
public class RedNotiItem
{
    public string name;
    public bool isShow;
}

[System.Serializable]
public class RedNotiData
{
    public List<RedNotiItem> redNotiList = new List<RedNotiItem>();
}
[System.Serializable]
public class RedDotItem
{
    public string name;
    public GameObject redDotObject;
}

public class NotiManager : MonoBehaviour
{
    [SerializeField] private GameObject notiObject;
    [SerializeField] private TextMeshProUGUI notiTxt;
    [SerializeField] private GameObject notiObjectingame;
    [SerializeField] private TextMeshProUGUI notiTxtInGame;
    [SerializeField] private GameObject notiRedDot;
    public static NotiManager instance;
    [Header("Red Dot List")]
    [SerializeField] private List<RedDotItem> redDots = new List<RedDotItem>();

    private Dictionary<string, GameObject> redDotDict = new Dictionary<string, GameObject>();
    [SerializeField] private RedNotiData redNotiData = new RedNotiData();
    private const string LAST_LOGIN_DATE_KEY = "LastLoginDate";
    [SerializeField] private string saveDailyGiftFilePath;
    private string currentUID;
    public string notiPath => Path.Combine(Application.persistentDataPath, "Noti.json");
    private void Awake()
    {
        instance = this;
        saveDailyGiftFilePath = Application.persistentDataPath + "/dailyGift.json";
        foreach (var dot in redDots)
        {
            redDotDict[dot.name] = dot.redDotObject;
        }
        LoadLocal();
    }
   
    public void ShowNotification(string message)
    {
        StopAllCoroutines();
        notiObject.gameObject.SetActive(true);
        notiTxt.text = message;
        StartCoroutine(HideNotificationAfterDelay(1f));
    }
    private IEnumerator HideNotificationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        notiObject.gameObject.SetActive(false);
    }
    public void ShowNotificationInGame(string message)
    {
        StopAllCoroutines();
        notiObjectingame.gameObject.SetActive(true);
        notiTxtInGame.text = message;
        StartCoroutine(HideNotificationAfterDelayInGame(1f));
    }
    private IEnumerator HideNotificationAfterDelayInGame(float delay)
    {
        yield return new WaitForSeconds(delay);
        notiObjectingame.gameObject.SetActive(false);
    }
    public void ShowNotiRedDot(string name)
    {
        if (redDotDict.ContainsKey(name))
        {
            redDotDict[name].SetActive(true);
            UpdateRedDotStatus(name, true);
            SaveRedDots();
        }
    }

    public void ClearNotiRedDot(string name)
    {
        if (redDotDict.ContainsKey(name))
        {
            redDotDict[name].SetActive(false);
            UpdateRedDotStatus(name, false);
            SaveRedDots();
        }
    }

    private void UpdateRedDotStatus(string name, bool isShow)
    {
        var item = redNotiData.redNotiList.Find(x => x.name == name);
        if (item != null)
        {
            item.isShow = isShow;
        }
        else
        {
            redNotiData.redNotiList.Add(new RedNotiItem { name = name, isShow = isShow });
        }
    }
    public void Init(string uid)
    {
        currentUID = string.IsNullOrEmpty(uid) ? "guest" : uid;
        LoadLocal();
    }

    private string GetPath() => Path.Combine(Application.persistentDataPath, $"Noti_{currentUID}.json");

    public void LoadLocal()
    {
        if (File.Exists(notiPath))
        {
            string json = File.ReadAllText(notiPath);
            redNotiData = JsonUtility.FromJson<RedNotiData>(json);

            foreach (var item in redNotiData.redNotiList)
            {
                if (redDotDict.ContainsKey(item.name))
                {
                    redDotDict[item.name].SetActive(item.isShow);
                }
            }
        }
        else
        {
            foreach (var dot in redDots)
            {
                dot.redDotObject.SetActive(false);
                redNotiData.redNotiList.Add(new RedNotiItem { name = dot.name, isShow = false });
            }
            SaveRedDots();
        }
    }

    public void SaveRedDots()
    {
        string json = JsonUtility.ToJson(redNotiData, true);
        File.WriteAllText(notiPath, json);
    }
    public void CoomingSoon()
    {
        StopAllCoroutines();
        notiObject.gameObject.SetActive(true);
        notiTxt.text = LocalizationManager.instance.GetLocalizedText("warning_coming_soon");
        StartCoroutine(HideNotificationAfterDelay(1f));
    }
    private IEnumerator WaitForServerTime()
    {
        while (!TimeManager.Instance.IsTimeFetched)
        {
            yield return null;
        }

        CheckDailyLogin();

    }
    public void ClearMultipleNotiRedDots(List<string> names)
    {
        foreach (var name in names)
        {
            if (redDotDict.ContainsKey(name))
            {
                redDotDict[name].SetActive(false);
                UpdateRedDotStatus(name, false);
            }
        }
        SaveRedDots();
    }
 
    public void ShowMultipleNotiRedDots(List<string> names)
    {
        foreach (var name in names)
        {
            if (redDotDict.ContainsKey(name))
            {
                redDotDict[name].SetActive(true);
                UpdateRedDotStatus(name, true);
            }
        }
        SaveRedDots();
    }
    public  void CheckDailyLogin()
    {

        ShowMultipleNotiRedDots(new List<string> { "shop", "dailygift" });
    }
    public DailyGiftData LoadGiftData()
    {
        if (File.Exists(saveDailyGiftFilePath))
        {
            string json = File.ReadAllText(saveDailyGiftFilePath);
            return JsonUtility.FromJson<DailyGiftData>(json);
        }
        return new DailyGiftData();
    }
    public bool IsRedDotActive(string name)
    {
        var item = redNotiData.redNotiList.Find(x => x.name == name);
        return item != null && item.isShow;
    }

}

