using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonStageController : MonoBehaviour
{
    [SerializeField] private ButtonStage stageButtonPrefab; 
    [SerializeField] private int totalLevels;
    [SerializeField] private Transform buttonParent;
    [SerializeField] private GameObject playZone;
    [SerializeField] private GameObject stageZone;

    void Start()
    {
        CreateButtons();
    }

    private void CreateButtons()
    {
        for (int i = 0; i < totalLevels; i++)
        {
            ButtonStage button = Instantiate(stageButtonPrefab, buttonParent);
            button.GetComponentInChildren<TextMeshProUGUI>().text =  (i + 1).ToString();
            button.Initialize(i + 1, LoadLevel);
        }
    }

    private void LoadLevel(int levelIndex)
    {
        playZone.gameObject.SetActive(true);
        LevelManager.instance.LoadLevel(levelIndex);
        GameManager.instance.LoadLevel();
        PlayerController.instance.LoadLevel();
        stageZone.gameObject.SetActive(false);

    }


}
