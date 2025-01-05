using System.Collections;
using TMPro;
using UnityEngine;

public class FirstPlayManager : MonoBehaviour
{
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject noti;
    [SerializeField] private TextMeshProUGUI notiTxt;
    public static FirstPlayManager instance;
    public bool isFirst=true;
    private void Awake()
    {
        instance = this;
        startScreen.SetActive(false);
        noti.gameObject.SetActive(false);
    }
    public void FirstPlay()
    {
        if (/*PlayerPrefs.GetInt("FirstPlay", 1) == 1*/isFirst)
        {
            ShowStartScreen();
            //PlayerPrefs.SetInt("FirstPlay", 0);
           // PlayerPrefs.Save();
           isFirst = false;
        }
        else
        {
            startScreen.SetActive(false);
        }
    }

    private void ShowStartScreen()
    {
        startScreen.SetActive(true);
        noti.gameObject.SetActive(true);
        notiTxt.text = "Reach the girl to win!";
        StartCoroutine(HideFirstStartAfterDelay(1f));

    }
    private IEnumerator HideFirstStartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        startScreen.gameObject.SetActive(false);
        noti.gameObject.SetActive(false);
    }
}
