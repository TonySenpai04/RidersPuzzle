using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private GridController gridController;
    [SerializeField] private Transform player;
    [SerializeField] private int currentRow = 0; 
    [SerializeField] private int currentCol = 0; 
    [SerializeField] private Vector3 targetPosition; 
    [SerializeField] private float moveSpeed = 5.0f;
    private IMoveHistory moveHistory;
    public INumberOfMoves numberOfMoves;
    public IImmortal immortal;
    private void Awake()
    {
        moveHistory = new MoveHistory();
    }
    void Start()
    {
        LoadMove();
    }
    public void LoadMove()
    {
        moveHistory.ClearHistory();
        LevelDataInfo level = LevelManager.instance.GetCurrentLevelData();
        numberOfMoves = new NumberOfMove(level.move);
        currentRow = (int)level.startPos.x;
        currentCol = (int)level.startPos.y;
        moveHistory.AddMove(currentRow, currentCol);
        UpdateCharacterPosition(currentRow, currentCol);
    }
 
    public void Movement()
    {
        if (!LevelManager.instance.IsCompleteLoadObject())
            return;
        player.transform.position =
           Vector3.MoveTowards(player.transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Input.GetMouseButtonDown(0) && Time.timeScale!=0 )
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                GameObject clickedCell = hit.collider.gameObject;
                MoveToCell(clickedCell);
            }
        }
    } 
    void MoveToCell(GameObject cell)
    {
        for (int row = 0; row < gridController.rows; row++)
        {
            for (int col = 0; col < gridController.cols; col++)
            {
                if (gridController.grid[row, col] == cell )
                {
                    if (Mathf.Abs(row - currentRow) + Mathf.Abs(col - currentCol) == 1 && numberOfMoves.GetCurrentMove() > 0)
                    {
                        moveHistory.AddMove(currentRow, currentCol);
                        currentRow = row;
                        currentCol = col;
                        
                        numberOfMoves.ReduceeMove(1);
                        if (immortal != null)
                        {
                            immortal.OnMove();
                        }
                        UpdateCharacterPosition(currentRow, currentCol); 
                    }
                }
            }
        }
    }
    public Tuple<int, int> GetLastMove()
    {
       return moveHistory.GetLastMove();
    }
    public void UndoLastMove(int jumpSteps)
    {
        for (int i = 0; i < jumpSteps; i++)
        {
            if (moveHistory.HasHistory())
            {
                Tuple<int, int> lastPosition = moveHistory.UndoMove();

                currentRow = lastPosition.Item1;
                currentCol = lastPosition.Item2;

                UpdateCharacterPosition(currentRow,currentCol); 
            }
            else
            {
                break; 
            }
        }
    }
  
    private Vector2Int GetCurrentDirection()
    {
        int previousRow = 1;
        int previousCol = 0;
        if (moveHistory.HasHistory())
        {
            Tuple<int, int> lastPosition = moveHistory.UndoMove();

            previousRow = lastPosition.Item1;
            previousCol = lastPosition.Item2;
        }
        int deltaX = currentRow - previousRow;
        int deltaY = currentCol - previousCol; 
        return new Vector2Int(deltaX, deltaY); 
    }

    public void MoveForward(int steps)
    {
        Vector2Int direction = GetCurrentDirection();
        moveHistory.AddMove(currentRow, currentCol);
        for (int i = 0; i < steps; i++)
        {
            int newRow = currentRow + direction.x;
            int newCol = currentCol + direction.y;
            if (newRow >= 0 && newRow < gridController.rows && newCol >= 0 && newCol < gridController.cols)
            {

                currentRow = newRow;
                currentCol = newCol;
                UpdateCharacterPosition(currentRow, currentCol);

            }
            else
            {
                break; 
            }
        }
    }
    public void UpdateCharacterPosition(int currentRow,int currentCol)
    {
        targetPosition = new Vector3(gridController.grid[currentRow, currentCol].transform.position.x,
                                     gridController.grid[currentRow, currentCol].transform.position.y, 0);
        player.transform.position = targetPosition;
        
    }
    public void MoveStartPoint()
    {
        LevelDataInfo level = LevelManager.instance.GetCurrentLevelData();
        currentRow = (int)level.startPos.x;
        currentCol = (int)level.startPos.y;
        UpdateCharacterPosition(currentRow, currentCol);
        

    }
    public void MoveEndPoint()
    {
        LevelDataInfo level = LevelManager.instance.GetCurrentLevelData();
        currentRow = (int)level.endPos.x;
        currentCol = (int)level.endPos.y;
        UpdateCharacterPosition(currentRow, currentCol);


    }
    public Tuple<int,int> GetPos()
    {
        return Tuple.Create(currentRow, currentCol);
    }
    public void MoveToBlock(int row,int col)
    {
        if (row >= 0 && row < gridController.rows && col >= 0 && col < gridController.cols)
        {
            currentRow = row;
            currentCol = col;
            UpdateCharacterPosition(currentRow, currentCol);
            moveHistory.AddMove(currentRow, currentCol);
        }
    }

}
