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
    public BackgroundController backgroundController;
    GameObject objectWin;
    private void Awake()
    {
        instance = this; 
    }
    void Start()
    {

       LoadLevel();
       playZone.gameObject.SetActive(false);
    }
    public void LoadLevel()
    {
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

        stageTxt.text = "STAGE " + level.level;
        backgroundController.UpdateRandomArt();

        PlayerController.instance.LoadLevel();

        Time.timeScale = 1;
    }
    public void FixedUpdate()
    {
        CheckWinCondition();
       
    }
    public void CheckWinCondition()
    {

        if (movementController.GetPos().Item1 == targetRow && movementController.GetPos().Item2 == targetCol)
        {
            panelWin.SetActive(true);
            LevelManager.instance.UnlockNextLevel();
            isEnd = true;
            Time.timeScale = 0;

        }
        else if (movementController.numberOfMoves.GetCurrentMove() <= 0 
            || PlayerController.instance.hitPoint.GetCurrentHealth()<=0)
        {
            panelLose.gameObject.SetActive(true);
            isEnd = true;
            Time.timeScale = 0;

        }
        else
        {
            isEnd=false;
            Time.timeScale = 1;
        }
    }
    public void LoadNextLevel()
    {
        LevelManager.instance.LoadNextLevel();
        LoadLevel();
        isEnd = false;
        Time.timeScale = 1;
    }
    public void ReplayLevel()
    {
        LoadLevel();
        isEnd = false;
        Time.timeScale = 1;
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
