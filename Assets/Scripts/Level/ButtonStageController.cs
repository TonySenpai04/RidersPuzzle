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
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private StageHeroController stageHeroController;
    private List<ButtonStage> buttons = new List<ButtonStage>(); 

    void Start()
    {
        CreateButtons();
    }

    private void CreateButtons()
    {
        for (int i = 0; i < totalLevels; i++)
        {
            bool isUnlocked = levelManager.IsLevelUnlocked(i + 1);
            ButtonStage button = Instantiate(stageButtonPrefab, buttonParent);
            button.GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();
            button.Initialize(i + 1, LoadLevel, isUnlocked);
            buttons.Add(button);
        }
    }
    private void FixedUpdate()
    {
        UpdateButtons();
    }

    public void UpdateButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            bool isUnlocked = levelManager.IsLevelUnlocked(i + 1);

            buttons[i].SetButtonState(isUnlocked);
        }
    }

    private void LoadLevel(int levelIndex)
    {
        if (stageHeroController.isHero())
        {
            playZone.gameObject.SetActive(true);
            levelManager.SetLevel(levelIndex);
            PlayerController.instance.SetCurrentData(stageHeroController.GetCurrentHeroData());
            GameManager.instance.LoadLevel();
            stageZone.gameObject.SetActive(false);
        }
    }
}
