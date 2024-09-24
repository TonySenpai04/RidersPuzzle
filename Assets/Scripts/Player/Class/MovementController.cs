using System;
using System.Collections;
using System.Collections.Generic;
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
    void Start()
    {
        moveHistory = new MoveHistory();
        UpdateCharacterPosition(); 

    }

    void Update()
    {
        player.transform.position = 
            Vector3.MoveTowards(player.transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Input.GetMouseButtonDown(0))
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
        // Lấy hàng và cột của ô được click
        for (int row = 0; row < gridController.rows; row++)
        {
            for (int col = 0; col < gridController.cols; col++)
            {
                if (gridController.grid[row, col] == cell)
                {
                    // Kiểm tra có phải là ô liền kề không
                    if (Mathf.Abs(row - currentRow) + Mathf.Abs(col - currentCol) == 1)
                    {
                        moveHistory.AddMove(currentRow, currentCol);
                        currentRow = row;
                        currentCol = col;
                        //Debug.Log(row + "-" + col);
                        UpdateCharacterPosition(); // Cập nhật vị trí mục tiêu
                    }
                }
            }
        }
    }
    public void UndoLastMove()
    {
        if (moveHistory.HasHistory())
        {
            // Lấy vị trí cuối cùng từ lịch sử
            Tuple<int, int> lastPosition = moveHistory.UndoMove();

            // Cập nhật vị trí nhân vật
            currentRow = lastPosition.Item1;
            currentCol = lastPosition.Item2;

            UpdateCharacterPosition(); // Cập nhật vị trí mục tiêu
        }
    }

    void UpdateCharacterPosition()
    {
        targetPosition = new Vector3(gridController.grid[currentRow, currentCol].transform.position.x,
                                     gridController.grid[currentRow, currentCol].transform.position.y, 0);
        player.transform.position = targetPosition;
        
    }

    public Tuple<int,int> GetPos()
    {
        return Tuple.Create(currentRow, currentCol);
    }

}
