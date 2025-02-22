using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScreenController : MonoBehaviour
{
    public GameObject mainScreen;
    public GameObject SelectZone;
    public Button btnStart;
    public GameManager gameManager;
    public GameObject panelTransition;
    private void Start()
    {
        SoundManager.instance.PlayMusic("Start Screen");
        btnStart.onClick.AddListener(() => Transition());
    }
    public void Transition()
    {
        gameManager.DisableMain();
        panelTransition.gameObject.SetActive(true);
        SelectZone.SetActive(true);
        mainScreen.SetActive(false);
        ShowTransition();
    }
    public void ShowTransition()
    {
        StopAllCoroutines();

        StartCoroutine(HideTransitionAfterDelay(1f));
    }
    private IEnumerator HideTransitionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideMainScreen();
    }
    public void HideMainScreen()
    {
        panelTransition.gameObject.SetActive(false);

    }
}
