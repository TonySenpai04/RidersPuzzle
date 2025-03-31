using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] public int targetRow = 5; 
    [SerializeField] public int targetCol = 4; 
    public GameObject panelWin;
    public GameObject panelLose;
    public GameObject playZone;
    public TextMeshProUGUI stageTxt;
    public GameObject winCellPrefab;
    [Header("References")]
    public MovementController movementController;
    public  GridController gridController;
    public static GameManager instance;
    public bool isEnd=false;
    public BackgroundManager backgroundController;
    GameObject objectWin;
    public bool isMainActive = true;
    private bool hasPlayedWinSound = false; 
    private bool hasPlayedLoseSound = false;


    private void Awake()
    {
        instance = this; 
    }
    void Start()
    {

    //   LoadLevel();
       playZone.gameObject.SetActive(false);
    }
    private void FixedUpdate() => CheckWinCondition();

    #region Level Management
    public void LoadLevel()
    {
        ResetLevelState();
        LevelManager.instance.LoadLevel();

        var level = LevelManager.instance.GetCurrentLevelData();
        targetRow = (int)level.endPos.x;
        targetCol = (int)level.endPos.y;

        SetupWinCell();
        ApplyText.instance.UpdateTitleStage(level.level);
        backgroundController.UpdateRandomArt();
        PlayerController.instance.LoadLevel();

         foreach (var quest in QuestManager.instance.GetQuestsByType<PlayStageQuest>())
        {
            QuestManager.instance.UpdateQuest(quest.questId, 1, StageHeroController.instance.currentId);
        }
    }
    public void LoadLangue()
    {
        var level = LevelManager.instance.GetCurrentLevelData();
        ApplyText.instance.UpdateTitleStage(level.level);
    }
   
    public void LoadNextLevel()
    {
        SoundManager.instance.StopSFX();
        LevelManager.instance.LoadNextLevel();

        if (LevelManager.instance.isFinal())
        {
            NotiManager.instance.ShowNotificationInGame("Complete all current stages");
        }
        else
        {
            LoadLevel();
            isEnd = false;
        }
    }

    public void ReplayLevel()
    {
        if (isEnd || Time.timeScale == 0) return;
        ReloadLevel();
    }

    public void ReplayLevelWhenEnd()
    {
        if (Time.timeScale == 0) return;
        ReloadLevel();
    }

    private void ReloadLevel()
    {
        SoundManager.instance.StopSFX();
        LoadLevel();
        isEnd = false;
    }

    private void ResetLevelState()
    {
        SoundManager.instance.StopSFX();
        if (!isMainActive) SoundManager.instance.PlaySFX("Stage Start");
        panelWin.SetActive(false);
        panelLose.SetActive(false);
        if (objectWin != null) Destroy(objectWin);
    }

    private void SetupWinCell()
    {
        Transform winPos = gridController.grid[targetRow, targetCol].transform;
        float screenWidth = Camera.main.orthographicSize * 2 * Screen.width / Screen.height;
        float cellSize = (screenWidth - 0.1f * (6 - 1)) / 6 - 0.1f;

        objectWin = Instantiate(winCellPrefab, winPos.position, Quaternion.identity);
        objectWin.transform.localScale = new Vector3(cellSize, cellSize, 1);
        objectWin.transform.SetParent(winPos);
    }
    #endregion

    #region Game State Checks
    private void CheckWinCondition()
    {
        if (HasPlayerReachedTarget() && PlayerController.instance.hitPoint.GetCurrentHealth() > 0)
            HandleWin();
        else if (IsPlayerOutOfMovesOrHealth())
            HandleLose();
        else
            ResetEndState();
    }

    private bool HasPlayerReachedTarget()
    {
        return movementController.GetPos().Item1 == targetRow && movementController.GetPos().Item2 == targetCol;
    }

    private bool IsPlayerOutOfMovesOrHealth() =>
        movementController.numberOfMoves.GetCurrentMove() <= 0 ||
        PlayerController.instance.hitPoint.GetCurrentHealth() <= 0;

    private void HandleWin()
    {
        if (!hasPlayedWinSound)
        {
            SoundManager.instance.PlaySFX("Stage Clear");
           
            foreach (var quest in QuestManager.instance.GetQuestsByType<WinStageNoDamageQuest>())
            {
                QuestManager.instance.UpdateQuest(quest.questId, 1,0);
            }
            foreach (var quest in QuestManager.instance.GetQuestsByType<WinStageQuest>())
            {
                QuestManager.instance.UpdateQuest(quest.questId, 1, 0);
            }
            LevelManager.instance.UnlockNextLevel();
            hasPlayedWinSound = true;
        }
        // SaveGameManager.instance.SaveLevelProgress(LevelManager.instance.GetCurrentLevelData().level, true, true);
        panelWin.SetActive(true);
        LevelManager.instance.ClearObject();
        Destroy(objectWin, 0.5f);
        objectWin = null;
        isEnd = true;
    }

    private void HandleLose()
    {
        if (!hasPlayedLoseSound)
        {
            SoundManager.instance.PlaySFX("Stage Failed");
            hasPlayedLoseSound = true;
        }

        panelLose.SetActive(true);
        LevelManager.instance.ClearObject();
        Destroy(objectWin, 0.5f);
        objectWin = null;
        isEnd = true;
    }

    private void ResetEndState()
    {
        isEnd = false;
        hasPlayedWinSound = false;
        hasPlayedLoseSound = false;
    }
    #endregion

    #region Game Control
    public void PauseGame()
    {
        StopAllCoroutines();
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        StopAllCoroutines();
        Time.timeScale = 1;
    }

    public void PauseGameAfterDelay(float delay = 1) => StartCoroutine(PauseGameAfterDelayCoroutine(delay));

    public void ResumeGameAfterDelay(float delay = 1) => StartCoroutine(ResumeGameAfterDelayCoroutine(delay));

    private IEnumerator PauseGameAfterDelayCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        PauseGame();
    }

    private IEnumerator ResumeGameAfterDelayCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResumeGame();
    }
    #endregion

    public void DisableMain() => isMainActive = false;


}
