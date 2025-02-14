using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private GameObject info;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private StageHeroController stageHeroController;
    [SerializeField] private List<ButtonStage> buttons = new List<ButtonStage>();
    [SerializeField] private GameObject notiObject;
    [SerializeField] private TextMeshProUGUI notiTxt;
    [SerializeField] private GameObject transitionLevel;
    [SerializeField] private TutorialController tutorial;
    [SerializeField] private ObjectLibraryController objectLibraryController;

    void Start()
    {
        notiObject.gameObject.SetActive(false);
        CreateButtons();
        stageZone.SetActive(false);

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
                // Hiển thị tutorial và gán sự kiện đóng để load level
                if (tutorial != null)
                {
                    tutorial.gameObject.SetActive(true);
                    tutorial.close.onClick.AddListener(() => StartLevel(levelIndex));
                    return;
                }
            }

            // Nếu không phải lần đầu, load level ngay
            StartLevel(levelIndex);
            //playZone.gameObject.SetActive(true);
            //transitionLevel.gameObject.SetActive(true);
            //levelManager.StartCoroutine(HideAfterDelay(1f));
            //levelManager.SetLevel(levelIndex);
            //PlayerController.instance.SetCurrentData(stageHeroController.GetCurrentHeroData());
            //GameManager.instance.LoadLevel();
            //levelManager.ClearObject();
            //FirstPlayManager.instance.FirstPlay(() =>
            //{
            //    levelManager.LoadObject(levelManager.GetLevel(levelIndex-1));
            //});
            //info.gameObject.SetActive(false);
            //stageZone.gameObject.SetActive(false);
        }
        else
        {
            ShowNotification("Please select a character!");
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

