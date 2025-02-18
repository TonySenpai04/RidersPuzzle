using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public int targetRow = 5; 
    [SerializeField] public int targetCol = 4; 
    public GameObject panelWin;
    public GameObject panelLose;
    public MovementController movementController;
    public GameObject winCellPrefab;
    public  GridController gridController;
    public static GameManager instance;
    public bool isEnd=false;
    public GameObject playZone;
    public TextMeshProUGUI stageTxt;
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

       LoadLevel();
       playZone.gameObject.SetActive(false);
    }
    public void DisableMain()
    {
        isMainActive = false;
    }
    public void LoadLevel()
    {
        SoundManager.instance.StopSFX();
        if (!isMainActive)
        {
            SoundManager.instance.PlaySFX("Stage Start");
        }
        LevelManager.instance.LoadLevel();
        LevelDataInfo level = LevelManager.instance.GetCurrentLevelData();
        targetRow = (int)level.endPos.x;
        targetCol = (int)level.endPos.y;

        float screenWidth = Camera.main.orthographicSize * 2 * Screen.width / Screen.height;
        float cellSize = (float)(screenWidth - 0.1 * (6 - 1)) / 6 - 0.1f;

        panelWin.gameObject.SetActive(false);
        panelLose.gameObject.SetActive(false);

        if (objectWin != null)
        {
            Destroy(objectWin);
            objectWin = null; 
        }
        Transform winPos = gridController.grid[targetRow, targetCol].transform;
        objectWin = Instantiate(winCellPrefab, winPos.transform.position, Quaternion.identity);
        objectWin.transform.localScale = new Vector3(cellSize, cellSize, 1);
        objectWin.transform.SetParent(gridController.grid[targetRow, targetCol].transform);

        ApplyText.instance.UpdateTitleStage(level.level);
        backgroundController.UpdateRandomArt();

        PlayerController.instance.LoadLevel();

    }
    public void FixedUpdate()
    {
        CheckWinCondition();
       
    }
    public void CheckWinCondition()
    {

        if (movementController.GetPos().Item1 == targetRow && movementController.GetPos().Item2 == targetCol 
            && PlayerController.instance.hitPoint.GetCurrentHealth() > 0) 
        {
            if (!hasPlayedWinSound) 
            {
                SoundManager.instance.PlaySFX("Stage Clear");
                hasPlayedWinSound = true; 
            }
           SaveGameManager.instance.SaveLevelProgress(LevelManager.instance.GetCurrentLevelData().level, true,true);
            panelWin.SetActive(true);
            LevelManager.instance.UnlockNextLevel();
            LevelManager.instance.ClearObject();
            Destroy(objectWin, 0.5f);
            objectWin = null;
            isEnd = true;

        }
        else if (movementController.numberOfMoves.GetCurrentMove() <= 0 
            || PlayerController.instance.hitPoint.GetCurrentHealth()<=0)
        {
            if (!hasPlayedLoseSound) // Chỉ phát âm thanh nếu chưa phát
            {
                SoundManager.instance.PlaySFX("Stage Failed");
                hasPlayedLoseSound = true; // Đánh dấu đã phát
            }
            panelLose.gameObject.SetActive(true);
            LevelManager.instance.ClearObject();
            Destroy(objectWin, 0.5f);
            objectWin = null;
            isEnd = true;

        }
        else
        {
            isEnd=false;
            hasPlayedWinSound = false;
            hasPlayedLoseSound = false;
        }
    }
    public void LoadNextLevel()
    {
        SoundManager.instance.StopSFX();
        LevelManager.instance.LoadNextLevel();
        Debug.Log(LevelManager.instance.isFinal());
        if (LevelManager.instance.isFinal())
        {
            NotiManager.instance.ShowNotificationInGame("Complete all current stages");
            return;
        }
        else
        {
            LoadLevel();
            isEnd = false;
        }

    }
    public void ReplayLevel()
    {
        if (isEnd)
            return;
        if (Time.timeScale == 0)
            return;
        SoundManager.instance.StopSFX();
        LoadLevel();
        isEnd = false;
    }
    public void ReplayLevelWhenEnd()
    {
        if (Time.timeScale == 0)
            return;
        SoundManager.instance.StopSFX();
        LoadLevel();
        isEnd = false;
    }
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
    public void PauseGameAfterDelay()
    {
        StartCoroutine(PauseGameAfterDelay(1));
    }
    public void ResumaGameAfterDelay()
    {
        StartCoroutine(ResumeGameAfterDelay(1));
    }
    private IEnumerator ResumeGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResumeGame();
    }
    private IEnumerator PauseGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PauseGame();
    }


}
