using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainScreenController : MonoBehaviour
{
    public GameObject mainScreen;
    public GameObject SelectZone;
    public Button btnStart;
    public GameManager gameManager;
    public GameObject panelTransition;
    public TextMeshProUGUI tipTxt;
    [SerializeField] private List<string> tipList;

    private void Start()
    {
        SoundManager.instance.PlayMusic("Start Screen");
        btnStart.onClick.AddListener(() => Transition());
        tipList = new List<string>
         {
        LocalizationManager.instance.GetLocalizedText("loading_screen_tip_1"),
        LocalizationManager.instance.GetLocalizedText("loading_screen_tip_2"),
        LocalizationManager.instance.GetLocalizedText("loading_screen_tip_3"),
        LocalizationManager.instance.GetLocalizedText("loading_screen_tip_4"),
        LocalizationManager.instance.GetLocalizedText("loading_screen_tip_5")
        };

        InvokeRepeating(nameof(ShowRandomTip), 0f, 3f);


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

    [System.Obsolete]
    private void ShowRandomTip()
    {
        if (tipTxt.gameObject.activeInHierarchy)
        {
            tipTxt.text = tipList[Random.Range(0, tipList.Count)];
        }
    }



}
