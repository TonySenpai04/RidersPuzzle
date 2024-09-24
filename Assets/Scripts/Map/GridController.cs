using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    private IGridGenerator gridGenerator;
    public GameObject blockPrefab;
    public int rows = 5; // Số hàng
    public int cols = 5; // Số cột
    public float cellSize = 1.0f; // Kích thước mỗi ô (độ rộng và cao)
    public float spacing = 0.1f; // Khoảng cách giữa các ô
    public Vector2 centerOffset = new Vector2(0, 0); // Điểm trung tâm của lưới
    public GameObject[,] grid;
    [SerializeField] private MovementController character;
    [SerializeField] private LevelManager levelManager;
    private void Awake()
    {
        gridGenerator=new GridGenerator(this.gameObject,blockPrefab);
        gridGenerator.GenerateGrid( rows,  cols,  cellSize,  spacing, centerOffset);
        grid= gridGenerator.Grid();
 
    }
    private void FixedUpdate()
    {
        HighlightMovableCells();
        
    }

    void HighlightMovableCells()
    {
        int currentRow = character.GetPos().Item1;
        int currentCol = character.GetPos().Item2;

        foreach (GameObject cell in grid)
        {
            cell.GetComponent<SpriteRenderer>().color = new Color(238 / 255f, 130 / 255f, 238 / 255f);
        }

        // Làm sáng các ô liền kề
        HighlightCell(currentRow - 1, currentCol); // Ô trên
        HighlightCell(currentRow + 1, currentCol); // Ô dưới
        HighlightCell(currentRow, currentCol - 1); // Ô trái
        HighlightCell(currentRow, currentCol + 1); // Ô phải
        levelManager.CheckForHiddenObject(currentRow, currentCol);
    }

    void HighlightCell(int row, int col)
    {
        if (row >= 0 && row < rows && col >= 0 && col < cols)
        {
            grid[row, col].GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }
}
