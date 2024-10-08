using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public int targetRow = 5; // Hàng chiến thắng (hàng 5)
    [SerializeField] public int targetCol = 4; // Cột chiến thắng (cột 4)
    public TextMeshProUGUI txt;

    public MovementController movementController;
    public GameObject winCellPrefab;
    public  GridController gridController;
    public static GameManager instance;
    public bool isEnd=false;
    private void Awake()
    {
        instance = this; 
    }
    void Start()
    {
        txt.gameObject.SetActive(false);
        Transform winPos = gridController.grid[targetRow, targetCol].transform;
        GameObject objectWin= Instantiate(winCellPrefab, winPos.transform.position, Quaternion.identity);
        objectWin.transform.SetParent(gridController.grid[targetRow, targetCol].transform);
        

    }
    public void FixedUpdate()
    {
        CheckWinCondition();
    }
    public void CheckWinCondition()
    {

        if (movementController.GetPos().Item1 == targetRow && movementController.GetPos().Item2 == targetCol)
        {
            txt.gameObject.SetActive(true);

            txt.text = "Chúc mừng! Bạn đã chiến thắng!";
            isEnd = true;

        }
        else if (movementController.numberOfMoves.GetCurrentMove() <= 0 
            || PlayerController.instance.hitPoint.GetCurrentHealth()<=0)
        {
            txt.gameObject.SetActive(true);
            txt.text = "Thua!";
            isEnd = true; 

        }
    }
}
