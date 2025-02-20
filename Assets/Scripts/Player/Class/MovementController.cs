using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] public GridController gridController;
    [SerializeField] private Transform player;
    [SerializeField] private int currentRow = 0; 
    [SerializeField] private int currentCol = 0; 
    [SerializeField] private Vector3 targetPosition; 
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private AnimationController animationController;
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
        SetPosition((int)level.startPos.x, (int)level.startPos.y);
    }

    public void Movement()
    {
        if (!LevelManager.instance.IsCompleteLoadObject()) return;

        player.transform.position =
            Vector3.MoveTowards(player.transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Input.GetMouseButtonDown(0) && Time.timeScale != 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null) MoveToCell(hit.collider.gameObject);
        }
    }

    private void MoveToCell(GameObject cell)
    {
        for (int row = 0; row < gridController.rows; row++)
        {
            for (int col = 0; col < gridController.cols; col++)
            {
                if (gridController.grid[row, col] == cell &&
                    Mathf.Abs(row - currentRow) + Mathf.Abs(col - currentCol) == 1 &&
                    numberOfMoves.GetCurrentMove() > 0)
                {
                    ExecuteMove(row, col);
                }
            }
        }
    }

    private void ExecuteMove(int newRow, int newCol)
    {
        moveHistory.AddMove(currentRow, currentCol);
        SetPosition(newRow, newCol);
        numberOfMoves.ReduceeMove(1);
        SoundManager.instance.PlaySFX("Click Sound");
        immortal?.OnMove();
    }

    public void UndoLastMove(int steps)
    {
        for (int i = 0; i < steps && moveHistory.HasHistory(); i++)
        {
            Tuple<int, int> lastPosition = moveHistory.UndoMove();
            SetPosition(lastPosition.Item1, lastPosition.Item2);
        }
    }

    public Vector2Int GetCurrentDirection()
    {
        Tuple<int, int> lastPosition = moveHistory.HasHistory()
            ? moveHistory.GetLastMove()
            : Tuple.Create(currentRow - 1, currentCol);

        return new Vector2Int(currentRow - lastPosition.Item1, currentCol - lastPosition.Item2);
    }

    public void MoveForward(int steps)
    {
        Vector2Int direction = GetCurrentDirection();
        moveHistory.AddMove(currentRow, currentCol);
        for (int i = 0; i < steps; i++)
        {
            int newRow = currentRow + direction.x;
            int newCol = currentCol + direction.y;
            if (IsValidCell(newRow, newCol))
            {
                SetPosition(newRow, newCol);
            }
            else break;
        }
    }

    private bool IsValidCell(int row, int col)
    {
        return row >= 0 && row < gridController.rows &&
               col >= 0 && col < gridController.cols &&
               gridController.grid[row, col].GetComponent<Collider2D>()?.enabled == true;
    }

    private void SetPosition(int row, int col)
    {
        currentRow = row;
        currentCol = col;
        UpdateCharacterPosition();
    }

    private void UpdateCharacterPosition()
    {
        targetPosition = gridController.grid[currentRow, currentCol].transform.position;
        player.transform.position = targetPosition;
        animationController.SetPos(player);
    }

    public void MoveStartPoint()
    {
        LevelDataInfo level = LevelManager.instance.GetCurrentLevelData();
        SetPosition((int)level.startPos.x, (int)level.startPos.y);
    }

    public void MoveEndPoint()
    {
        LevelDataInfo level = LevelManager.instance.GetCurrentLevelData();
        SetPosition((int)level.endPos.x, (int)level.endPos.y);
    }

    public Tuple<int, int> GetPos() => Tuple.Create(currentRow, currentCol);

    public void MoveToBlock(int row, int col)
    {
        if (IsValidCell(row, col))
        {
            SetPosition(row, col);
            moveHistory.AddMove(currentRow, currentCol);
        }
    }
    public Tuple<int, int> GetLastMove()
    {
        return moveHistory.GetLastMove();
    }
}
