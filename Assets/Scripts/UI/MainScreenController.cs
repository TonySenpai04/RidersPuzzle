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
    private void Start()
    {
        btnStart.onClick.AddListener(() => HideMainScreen());
    }
    public void HideMainScreen()
    {
        gameManager.DisableMain();
        SelectZone.SetActive(true);
     
        mainScreen.SetActive(false);
    }
}
