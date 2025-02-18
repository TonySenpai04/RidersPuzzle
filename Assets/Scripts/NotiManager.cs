using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotiManager : MonoBehaviour
{
    [SerializeField] private GameObject notiObject;
    [SerializeField] private TextMeshProUGUI notiTxt;
    [SerializeField] private GameObject notiObjectingame;
    [SerializeField] private TextMeshProUGUI notiTxtInGame;
    public static NotiManager instance;
    private void Awake()
    {
        instance = this;
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
}
