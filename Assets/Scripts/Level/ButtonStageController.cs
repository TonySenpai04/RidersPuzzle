﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonStageController : MonoBehaviour
{
    [SerializeField] private ButtonStage stageButtonPrefab;
    [SerializeField] private int totalLevels;
    [SerializeField] private Transform buttonParent;
    [SerializeField] private GameObject playZone;
    [SerializeField] private GameObject stageZone;
    [SerializeField] private GameObject info;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private StageHeroController stageHeroController;
    [SerializeField] private List<ButtonStage> buttons = new List<ButtonStage>();
    [SerializeField] private GameObject notiObject;
    [SerializeField] private TextMeshProUGUI notiTxt;
    [SerializeField] private GameObject transitionLevel;
    [SerializeField] private TutorialController tutorial;
    [SerializeField] private ObjectLibraryController objectLibraryController;

    [SerializeField] private int levelsPerPage = 150;
    [SerializeField] private int currentPage = 0;
    [SerializeField] private int totalPages = 0;
    [SerializeField] private Button nextPageButton;
    [SerializeField] private Button prevPageButton;

 
    private async void Start()
    {
        await Task.Delay(2500);
        notiObject.gameObject.SetActive(false);
        totalLevels = levelManager.GetTotalLevel();
        totalPages = Mathf.CeilToInt((float)totalLevels / levelsPerPage);

        nextPageButton.onClick.AddListener(NextPage);
        prevPageButton.onClick.AddListener(PreviousPage);


        CreateButtons();
        ShowPageLevel();
        stageZone.SetActive(false);

    }
    
    private void UpdateVisibleButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            int start = currentPage * levelsPerPage;
            int end = Mathf.Min(start + levelsPerPage, totalLevels);

            buttons[i].gameObject.SetActive(i >= start && i < end);
        }

        UpdateNavigationButtons();
    }

    private void NextPage()
    {
        if (currentPage < totalPages - 1)
        {
            currentPage++;
            buttonParent.GetComponent<RectTransform>().transform.position = new Vector2(0, -9065.08f);
            StartCoroutine(DelayedLayoutRebuild());
            UpdateVisibleButtons();
        }
    }
    private IEnumerator DelayedLayoutRebuild()
    {
        yield return null; // Chờ 1 frame để đảm bảo nút đã spawn
        LayoutRebuilder.ForceRebuildLayoutImmediate(buttonParent.GetComponent<RectTransform>());
    }
    public void ShowPageLevel()
    {
        int completedLevel = levelManager.GetAllLevelComplete();
        currentPage = Mathf.Clamp((completedLevel - 1) / levelsPerPage, 0, totalPages - 1);
        buttonParent.GetComponent<RectTransform>().transform.position = new Vector2(0, -9065.08f);
        StartCoroutine(DelayedLayoutRebuild());
        UpdateVisibleButtons();
    }

    private void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            buttonParent.GetComponent<RectTransform>().transform.position = new Vector2(0, -9065.08f);
            StartCoroutine(DelayedLayoutRebuild());
            UpdateVisibleButtons();
        }
    }

    private void UpdateNavigationButtons()
    {
        nextPageButton.gameObject.SetActive(currentPage < totalPages - 1);
        prevPageButton.gameObject.SetActive(currentPage > 0);
    }

    private void CreateButtons()
    {
        totalLevels = levelManager.GetTotalLevel();
        for (int i = 0; i < totalLevels; i++)
        {
            bool isUnlocked = levelManager.IsLevelUnlocked(i + 1);
            ButtonStage button = Instantiate(stageButtonPrefab, buttonParent);
            button.GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();
            button.Initialize(i + 1, LoadLevel, isUnlocked,this);
            buttons.Add(button);
        }
        StartCoroutine(DelayedLayoutRebuild());
        UpdateVisibleButtons();
        buttonParent.GetComponent<RectTransform>().transform.position = new Vector2(0, -9065.08f);
    }

    private void FixedUpdate()
    {
        UpdateButtons();
        
    }
    private void OnEnable()
    {

        if (!GameManager.instance.isMainActive)
        {
            SoundManager.instance.PlayMusic("Home Screen");
        }
        if (buttons.Count > 0)
        {
            ShowPageLevel();
        }

    }
    public void UpdateButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            bool isUnlocked = levelManager.IsLevelUnlocked(i + 1);

            buttons[i].SetButtonState(isUnlocked);
        }
    }
  
    public void LoadLevel(int levelIndex)
    {
        if (stageHeroController.isHero())
        {
            SoundManager.instance.PlaySFX("Click Sound");
            if (FirstPlayManager.instance.isFirst)
            {
                if (tutorial != null)
                {
                    tutorial.gameObject.SetActive(true);
                    
                    tutorial.GetComponent<Image>().enabled = true;
                    tutorial.close.onClick.RemoveAllListeners();
                    tutorial.close.onClick.AddListener(() => StartLevel(levelIndex));
                    tutorial.close.onClick.AddListener(() => tutorial.CloseFirst());
                    return;
                }
            }

            StartLevel(levelIndex);
        }
        else
        {
            ShowNotification(LocalizationManager.instance.GetLocalizedText("warning_character"));
        }
    }
    private void StartLevel(int levelIndex)
    {
        playZone.gameObject.SetActive(true);
        transitionLevel.gameObject.SetActive(true);
        levelManager.StartCoroutine(HideAfterDelay(1f));
        levelManager.SetLevel(levelIndex);
        PlayerController.instance.SetCurrentData(stageHeroController.GetCurrentHeroData());
        GameManager.instance.LoadLevel();
        levelManager.ClearObject();
        FirstPlayManager.instance.FirstPlay(() =>
        {
            levelManager.LoadObject(levelManager.GetLevel(levelIndex - 1));
        });
        Level level= levelManager.GetLevel(levelIndex-1);

        foreach(var levelData in level.hiddenObjects)
        {
            HiddenObjectManager.instance.SetSeenObjectById(levelData.objectPrefab.GetComponent<HiddenObject>().id);
        }
       
        info.gameObject.SetActive(false);
        tutorial.close.onClick.RemoveAllListeners();
        tutorial.GetComponent<Image>().enabled = false;
        tutorial.close.onClick.AddListener(() => tutorial.Close());
        stageZone.gameObject.SetActive(false);
  
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
    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        transitionLevel.gameObject.SetActive(false);
    }
}

