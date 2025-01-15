using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;

public class FirstPlayManager : MonoBehaviour
{
    private string firstPlayPath => Path.Combine(Application.persistentDataPath, "FirstPlay.json");
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject noti;
    [SerializeField] private TextMeshProUGUI notiTxt;
    public static FirstPlayManager instance;
    public bool isFirst=true;
    private Coroutine hideScreenCoroutine;
    private void Awake()
    {
        instance = this;
        LoadDataFirstPlay();
        startScreen.SetActive(false);
        noti.gameObject.SetActive(false);
        ResetUI();
    }
    public void FirstPlay(System.Action onDelayedAction)
    {
        if (isFirst)
        {
            ShowStartScreen(onDelayedAction);
          
            isFirst = false;
            SaveFirstPlayData();
        }
        else
        {
            onDelayedAction?.Invoke();
        }
    }

    private void ShowStartScreen(System.Action onDelayedAction)
    {
        startScreen.SetActive(true);
        noti.gameObject.SetActive(true);
        notiTxt.text = "Reach the girl to win!";
        StartCoroutine(HideStartScreenAfterDelay(3f, onDelayedAction));
    }

    private IEnumerator HideStartScreenAfterDelay(float delay, System.Action onDelayedAction)
    {
        yield return new WaitForSeconds(delay);
        startScreen.SetActive(false);
        startScreen.gameObject.SetActive(false);
        noti.gameObject.SetActive(false);
        onDelayedAction?.Invoke();
    }
    //public void FirstPlay()
    //{
    //    if (/*PlayerPrefs.GetInt("FirstPlay", 1) == 1*/isFirst)
    //    {
    //        ShowStartScreen();
    //        //PlayerPrefs.SetInt("FirstPlay", 0);
    //       // PlayerPrefs.Save();
    //        isFirst = false;
          
    //    }
    //    else
    //    {
    //        startScreen.SetActive(false);
    //    }
    //}

    private void ShowStartScreen()
    {
        startScreen.SetActive(true);
        noti.gameObject.SetActive(true);
        notiTxt.text = "Reach the girl to win!";
        StartCoroutine(HideFirstStartAfterDelay(3f));

    }
    private IEnumerator HideFirstStartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        startScreen.gameObject.SetActive(false);
        noti.gameObject.SetActive(false);
    }
    public void ResetUI()
    {
        // Dừng coroutine nếu đang chạy
        if (hideScreenCoroutine != null)
        {
            StopCoroutine(hideScreenCoroutine);
            hideScreenCoroutine = null;
        }

        // Đặt lại trạng thái UI
        startScreen.SetActive(false);
        noti.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        ResetUI(); 
    }
    private void SaveFirstPlayData()
    {
        var data = new FirstPlayData { isFirst = this.isFirst };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(firstPlayPath, json);
        Debug.Log($"Saved hero data to {firstPlayPath}");
    }

    private void LoadDataFirstPlay()
    {
        if (File.Exists(firstPlayPath))
        {
            string json = File.ReadAllText(firstPlayPath);
            var data = JsonUtility.FromJson<FirstPlayData>(json);
            this.isFirst = data.isFirst;
            Debug.Log("Loaded hero data from JSON.");
        }
        else
        {
            Debug.LogWarning("No saved hero data found. Using default hero ID (0).");
            isFirst = true; // ID mặc định nếu không có dữ liệu
        }
    }
}
[System.Serializable]
public class FirstPlayData
{
    public bool isFirst;
}
