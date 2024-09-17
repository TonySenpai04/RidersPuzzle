using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private GridController gridController; 

    [SerializeField] private int currentRow = 0; // Vị trí hàng hiện tại của nhân vật
    [SerializeField] private int currentCol = 0; // Vị trí cột hiện tại của nhân vật
    [SerializeField] private Vector3 targetPosition; 
    [SerializeField] private float moveSpeed = 5.0f; 

    void Start()
    {
        GetComponent<SpriteRenderer>().color = Color.black;
        UpdateCharacterPosition(); // Đặt nhân vật vào vị trí hiện tại
        HighlightMovableCells(); // Làm sáng các ô có thể di chuyển
    }

    void Update()
    {
        // Di chuyển nhân vật từ từ tới vị trí mục tiêu
        transform.position = 
            Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Kiểm tra nhấp chuột để di chuyển đến ô mới
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
                        currentRow = row;
                        currentCol = col;
                        Debug.Log(row + "-" + col);
                        UpdateCharacterPosition(); // Cập nhật vị trí mục tiêu
                        HighlightMovableCells(); // Cập nhật các ô có thể di chuyển
                    }
                }
            }
        }
    }

    void UpdateCharacterPosition()
    {
        targetPosition = new Vector3(gridController.grid[currentRow, currentCol].transform.position.x,
                                     gridController.grid[currentRow, currentCol].transform.position.y, 0);
        transform.position = targetPosition;
        
    }

    void HighlightMovableCells()
    {
        // Reset màu sắc tất cả các ô
        foreach (GameObject cell in gridController.grid)
        {
            cell.GetComponent<SpriteRenderer>().color = new Color(238/255f, 130 / 255f, 238 / 255f);
        }

        // Làm sáng các ô liền kề
        HighlightCell(currentRow - 1, currentCol); // Ô trên
        HighlightCell(currentRow + 1, currentCol); // Ô dưới
        HighlightCell(currentRow, currentCol - 1); // Ô trái
        HighlightCell(currentRow, currentCol + 1); // Ô phải
    }

    void HighlightCell(int row, int col)
    {
        if (row >= 0 && row < gridController.rows && col >= 0 && col < gridController.cols)
        {
            gridController.grid[row, col].GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }
}
