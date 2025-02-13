using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotiManager : MonoBehaviour
{
    [SerializeField] private GameObject notiObject;
    [SerializeField] private TextMeshProUGUI notiTxt;
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
}
